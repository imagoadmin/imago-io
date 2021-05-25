using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading;
using System.Reflection;

namespace Imago.IO
{
    public partial class Client : IClient
    {
        public enum Matching { equals, includes, startWith, endWith };

        private Credentials _credentials;
        private string _apiUrl;

        private CookieContainer _cookieJar;
        private JavaScriptSerializer _jsonConverter = new JavaScriptSerializer();
        private JsonSerializerSettings _jsonSettings = new JsonSerializerSettings();
        private string _apiToken;
        private string _uid;

        private HttpResponseMessage _lastResponse;

        public ResultCode? LastSignInResultCode { get; private set; } = null;

        public Client()
        {
        }

        public string UserName
        {
            get
            {
                return _credentials == null ? null : _credentials.UserName;
            }
        }

        public Guid UserId
        {
            get
            {
                Guid.TryParse(_uid, out var uid);
                return uid;
            }
        }
        public Guid ApiToken
        {
            get
            {
                Guid.TryParse(_apiToken, out var token);
                return token;
            }
        }

        public HttpClient Direct
        {
            get
            {
                return GetClient();
            }
        }

        public string APIUrl
        {
            get
            {
                return _apiUrl;
            }
        }

        public int MaxRetryAttempts { get; set; } = 0;
        private int _retryDelayInSeconds = 2;
        public int RetryDelayInSeconds { get { return _retryDelayInSeconds > 0 ? _retryDelayInSeconds : 4; } set { _retryDelayInSeconds = value; } }
        private int _retryDelayFactor { get; set; } = 5;
        public int RetryDelayFactor { get { return _retryDelayFactor > 0 ? _retryDelayFactor : 4; } set { _retryDelayFactor = value; } }

        public Task<bool> SignIn(Credentials credentials, TimeSpan? timeout = null)
        {
            return SignIn(credentials, null, timeout);
        }

        public async Task<bool> SignIn(Credentials credentials, string product, TimeSpan? timeout = null)
        {
            try
            {
                LastSignInResultCode = null;
                _credentials = credentials;

                _apiUrl = credentials.HostName + credentials.ApiVersion;

                _jsonSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                _jsonSettings.NullValueHandling = NullValueHandling.Ignore;

                HttpClientHandler responseHandler = null;
                if (!String.IsNullOrWhiteSpace(credentials.ProxyUri))
                    responseHandler = new HttpClientHandler { 
                        Proxy = new WebProxy {
                            Address = new Uri(credentials.ProxyUri), 
                            Credentials = CredentialCache.DefaultCredentials, 
                            BypassProxyOnLocal = false 
                        }, 
                        UseProxy = true
                    };
                else
                    responseHandler = new HttpClientHandler();

                responseHandler.CookieContainer = new CookieContainer();

                HttpClient client = new HttpClient(responseHandler);
                client.Timeout = timeout ?? new TimeSpan(0, 0, 30);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string productCode = product ?? credentials.Product;
                string productVersion = credentials.Version;
                if (String.IsNullOrWhiteSpace(productVersion))
                {
                    try
                    {
                        Version version = Assembly.GetEntryAssembly()?.GetName()?.Version;
                        if (version != null)
                            productVersion = version.Major + "." + version.Minor + "." + version.Build + "." + version.MinorRevision;
                    }
                    catch (Exception)
                    {
                    }
                }

                Uri signInURI = new Uri(_apiUrl + "/session");
                var signindata = new { username = credentials.UserName, password = credentials.Password, product = productCode, version = productVersion };
                string body = _jsonConverter.Serialize(signindata, _jsonSettings);
                HttpResponseMessage response = await client.PutAsync(signInURI, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
                this.LogHttpResponse(response);

                _lastResponse = response;

                LastSignInResultCode = _lastResponse.GetResultCode();

                Task<string> reading = response.Content.ReadAsStringAsync();
                reading.Wait();
                body = reading.Result;

                if (response.StatusCode != HttpStatusCode.OK)
                    return false;

                var data = JObject.Parse(body);
                _apiToken = (string)data["apiToken"];
                _uid = (string)data["uid"];

                client.DefaultRequestHeaders.Add("imago-api-token", _apiToken);
                return true;
            }
            catch (Exception ex)
            {
                LastSignInResultCode = ResultCode.failed;
                Telemetry.TelemetryLogger.Instance?.LogException(ex);
                return false;
            }
        }

        private HttpClient GetClient(TimeSpan? timeout = null)
        {
            if (String.IsNullOrWhiteSpace(_apiToken))
                return null;

            HttpClientHandler responseHandler = null; ;
            if (!String.IsNullOrWhiteSpace(_credentials.ProxyUri))
                responseHandler = new HttpClientHandler { Proxy = new WebProxy(_credentials.ProxyUri, false), UseProxy = true, UseDefaultCredentials = true };
            else
                responseHandler = new HttpClientHandler();

            responseHandler.CookieContainer = new CookieContainer();

            HttpClient client = new HttpClient(responseHandler);
            client.Timeout = timeout ?? new TimeSpan(0, 10, 0);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Add("imago-api-token", _apiToken);
            return client;
        }

        private Task<Result<T>> ClientGet<T>(string relativePath, NameValueCollection query, CancellationToken ct, TimeSpan? timeout, Func<HttpResponseMessage, string, T> processResponse) where T : class
        {
            UriBuilder builder = new UriBuilder(_apiUrl);
            builder.Path += relativePath;
            builder.Query = BuildQueryString(query);

            return ClientGet(builder, ct, timeout, processResponse);
        }

        private async Task<Result<T>> ClientGet<T>(UriBuilder builder, CancellationToken ct, TimeSpan? timeout, Func<HttpResponseMessage, string, T> processResponse) where T : class
        {
            int retryCount = 0, retryDelay = this.RetryDelayInSeconds;
            do
            {
                try
                {
                    using (HttpClient client = GetClient(timeout))
                    {
                        HttpResponseMessage response = await client.GetAsync(builder.ToString(), ct);
                        this.LogHttpResponse(response);

                        _lastResponse = response;
                        return await response.ConvertToResult(processResponse);
                    }
                }
                catch (OperationCanceledException ex)
                {
                    Telemetry.TelemetryLogger.Instance?.LogException(ex);
                    return Result<T>.UnknownError();
                }
                catch (Exception ex)
                {
                    Telemetry.TelemetryLogger.Instance?.LogException(ex);
                }
                Thread.Sleep(TimeSpan.FromSeconds(retryCount < MaxRetryAttempts ? retryDelay : 0));
                retryDelay *= this.RetryDelayFactor;
                retryCount++;
            }
            while (retryCount <= MaxRetryAttempts);

            return Result<T>.UnknownError();
        }


        private async Task<Result<T>> ClientPost<T, TPostBody>(UriBuilder builder, TPostBody parameters, TimeSpan? timeout, CancellationToken ct, Func<HttpResponseMessage, string, T> processResponse) where T : class
        {

            int retryCount = 0, retryDelay = this.RetryDelayInSeconds;
            do
            {
                try
                {
                    using (HttpClient client = GetClient(timeout))
                    {
                        string requestbody = _jsonConverter.Serialize(parameters);
                        HttpResponseMessage response = await client.PostAsync(builder.ToString(), new StringContent(requestbody, Encoding.UTF8, "application/json"), ct).ConfigureAwait(false);
                        this.LogHttpResponse(response);
                        _lastResponse = response;

                        return await response.ConvertToResult(processResponse);
                    }
                }
                catch (Exception ex)
                {
                    Telemetry.TelemetryLogger.Instance?.LogException(ex);
                }
                Thread.Sleep(TimeSpan.FromSeconds(retryCount < MaxRetryAttempts ? retryDelay : 0));
                retryDelay *= this.RetryDelayFactor;
                retryCount++;
            }
            while (retryCount <= MaxRetryAttempts);

            return Result<T>.UnknownError();
        }

        private async Task<Result<T>> ClientPut<T, TPostBody>(UriBuilder builder, TPostBody parameters, TimeSpan? timeout, CancellationToken ct, Func<HttpResponseMessage, string, T> processResponse) where T : class
        {

            int retryCount = 0, retryDelay = this.RetryDelayInSeconds;
            do
            {
                try
                {
                    using (HttpClient client = GetClient(timeout))
                    {
                        string body = _jsonConverter.Serialize(parameters);
                        HttpResponseMessage response = await client.PutAsync(builder.ToString(), new StringContent(body, Encoding.UTF8, "application/json"), ct).ConfigureAwait(false);
                        this.LogHttpResponse(response);

                        _lastResponse = response;

                        return await response.ConvertToResult(processResponse);
                    }
                }
                catch (Exception ex)
                {
                    Telemetry.TelemetryLogger.Instance?.LogException(ex);
                }
                Thread.Sleep(TimeSpan.FromSeconds(retryCount < MaxRetryAttempts ? retryDelay : 0));
                retryDelay *= this.RetryDelayFactor;
                retryCount++;
            }
            while (retryCount <= MaxRetryAttempts);

            return Result<T>.UnknownError();
        }

        public async Task<bool> IsSessionValid(TimeSpan? timeout = null)
        {
            try
            {
                using (HttpClient client = GetClient(timeout))
                {
                    if (client == null)
                        return false;

                    HttpResponseMessage result = await client.GetAsync(_apiUrl + "/session").ConfigureAwait(false);
                    this.LogHttpResponse(result);
                    _lastResponse = result;

                    this.LogHttpResponse(result);

                    if (result.StatusCode != HttpStatusCode.OK)
                        _apiToken = null;

                    return (result.StatusCode == HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                Telemetry.TelemetryLogger.Instance?.LogException(ex);
                return false;
            }
        }

        public async Task<bool> SignOut(TimeSpan? timeout = null)
        {
            try
            {
                using (HttpClient client = GetClient(timeout))
                {
                    if (client == null)
                        return false;

                    HttpResponseMessage result = await client.DeleteAsync(_apiUrl + "/session").ConfigureAwait(false);
                    this.LogHttpResponse(result);
                    _lastResponse = result;

                    return (result.StatusCode == HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                Telemetry.TelemetryLogger.Instance?.LogException(ex);
                return false;
            }
            finally
            {
                _apiToken = null;
                LastSignInResultCode = null;
                _uid = null;
            }
        }

        public void LogHttpResponse(HttpResponseMessage response)
        {
            string request = response.RequestMessage.Method.ToString() + ":" + response.RequestMessage.RequestUri.ToString();

            Telemetry.TelemetryLogger.Instance?.LogEvent(Telemetry.TelemetryEvents.ClientHttpResponse, new Dictionary<string, string>()
            {
                { "Request", request },
                { "Status Code", response?.StatusCode.ToString() },
                { "Message", response?.ToString() },
            });
        }

        private string BuildQueryString(NameValueCollection nvc)
        {
            return string.Join("&", nvc.AllKeys.Where(key => !string.IsNullOrWhiteSpace(nvc[key])).Select(key => string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(nvc[key]))));
        }
    }
}
