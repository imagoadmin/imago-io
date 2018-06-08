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
using System.Globalization;
using System.Threading;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using Imago.IO.Classes;

namespace Imago.IO
{
    public partial class Client
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
        private string _lastResponseBody;

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

        public async Task<bool> SignIn(Credentials credentials, TimeSpan? timeout = null)
        {
            try
            {
                _credentials = credentials;

                _apiUrl = credentials.HostName + credentials.ApiVersion;

                _jsonSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                _jsonSettings.NullValueHandling = NullValueHandling.Ignore;

                HttpClientHandler responseHandler = null; ;
                if (!String.IsNullOrWhiteSpace(credentials.ProxyUri))
                    responseHandler = new HttpClientHandler { Proxy = new WebProxy(credentials.ProxyUri, false), UseProxy = true, UseDefaultCredentials = true };
                else
                    responseHandler = new HttpClientHandler();

                responseHandler.CookieContainer = new CookieContainer();

                HttpClient client = new HttpClient(responseHandler);
                client.Timeout = timeout ?? new TimeSpan(0, 10, 0);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Uri signInURI = new Uri(_apiUrl + "/session");
                var signindata = new { username = credentials.UserName, password = credentials.Password };
                string body = _jsonConverter.Serialize(signindata, _jsonSettings);
                HttpResponseMessage response = await client.PutAsync(signInURI, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);

                _lastResponse = response;

                Task<string> reading = response.Content.ReadAsStringAsync();
                reading.Wait();
                body = reading.Result;
                _lastResponseBody = body;

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
            using (HttpClient client = GetClient(timeout))
            {
                HttpResponseMessage response = await client.GetAsync(builder.ToString(), ct);
                _lastResponse = response;
                string body = await response.Content.ReadAsStringAsync();
                _lastResponseBody = body;

                if (response.StatusCode != HttpStatusCode.OK)
                    return new Result<T> { Code = response.GetResultCode() };

                var result = processResponse(response, body);

                return new Result<T> { Value = result, Code = result != null ? ResultCode.ok : ResultCode.failed };
            }
        }


        private async Task<Result<T>> ClientPost<T, TPostBody>(UriBuilder builder, TPostBody parameters, TimeSpan? timeout, CancellationToken ct, Func<HttpResponseMessage, string, T> processResponse) where T : class
        {
            string body = _jsonConverter.Serialize(parameters);

            using (HttpClient client = GetClient(timeout))
            {
                HttpResponseMessage response = await client.PostAsync(builder.ToString(), new StringContent(body, Encoding.UTF8, "application/json"), ct).ConfigureAwait(false);
                _lastResponse = response;

                body = await response.Content.ReadAsStringAsync();
                _lastResponseBody = body;

                if (response.StatusCode != HttpStatusCode.OK)
                    return new Result<T> { Code = response.GetResultCode(), Message = response.StatusCode.ToString() };

                var result = processResponse(response, body);
                return new Result<T> { Value = result, Code = result != null ? ResultCode.ok : ResultCode.failed };
            }
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
                    _lastResponse = result;

                    _lastResponseBody = null;
                    if (result.StatusCode != HttpStatusCode.OK)
                        _apiToken = null;

                    return (result.StatusCode == HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
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

                    HttpResponseMessage result = await client.DeleteAsync(_apiUrl + "/signout").ConfigureAwait(false);
                    _lastResponse = result;
                    _apiToken = null;

                    _lastResponseBody = null;

                    return (result.StatusCode == HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
            }
        }

        private string BuildQueryString(NameValueCollection nvc)
        {
            return string.Join("&", nvc.AllKeys.Where(key => !string.IsNullOrWhiteSpace(nvc[key])).Select(key => string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(nvc[key]))));
        }
    }
}
