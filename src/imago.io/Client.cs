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

        private HttpClient _client;
        private CookieContainer _cookieJar;
        private JavaScriptSerializer _jsonConverter = new JavaScriptSerializer();
        private JsonSerializerSettings _jsonSettings = new JsonSerializerSettings();

        private HttpResponseMessage _lastResponse;
        private string _lastResponseBody;

        public string UserName
        {
            get
            {
                return _credentials == null ? null : _credentials.UserName;
            }
        }

        public HttpClient Direct
        {
            get
            {
                return _client;
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

                _cookieJar = new CookieContainer();

                HttpClientHandler responseHandler = null; ;
                if (!String.IsNullOrWhiteSpace(credentials.ProxyUri))
                    responseHandler = new HttpClientHandler { Proxy = new WebProxy(credentials.ProxyUri, false), UseProxy = true, UseDefaultCredentials = true };
                else
                    responseHandler = new HttpClientHandler();

                responseHandler.CookieContainer = _cookieJar;

                _client = new HttpClient(responseHandler);
                _client.Timeout = timeout ?? new TimeSpan(0, 10, 0);
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Uri signInURI = new Uri(_apiUrl + "/session");
                var signindata = new { username = credentials.UserName, password = credentials.Password };
                string body = _jsonConverter.Serialize(signindata, _jsonSettings);
                HttpResponseMessage response = await _client.PutAsync(signInURI, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);

                _lastResponse = response;

                Task<string> reading = response.Content.ReadAsStringAsync();
                reading.Wait();
                body = reading.Result;
                _lastResponseBody = body;

                if (response.StatusCode != HttpStatusCode.OK)
                    return false;

                var data = JObject.Parse(body);
                string apiToken = (string)data["apiToken"];

                _client.DefaultRequestHeaders.Add("imago-api-token", apiToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsSessionValid()
        {
            try
            {
                if (_client == null)
                    return false;

                HttpResponseMessage result = await _client.GetAsync(_apiUrl + "/session").ConfigureAwait(false);
                _lastResponse = result;

                _lastResponseBody = null;

                return (result.StatusCode == HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> SignOut()
        {
            try
            {
                if (_client == null)
                    return false;

                HttpResponseMessage result = await _client.DeleteAsync(_apiUrl + "/signout").ConfigureAwait(false);
                _lastResponse = result;

                _lastResponseBody = null;

                return (result.StatusCode == HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                _client = null;
            }
        }
        private string BuildQueryString(NameValueCollection nvc)
        {
            return string.Join("&", nvc.AllKeys.Where(key => !string.IsNullOrWhiteSpace(nvc[key])).Select(key => string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(nvc[key]))));
        }
    }
}
