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
using System.Web.Script.Serialization;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Threading;
using System.Net.Http.Formatting;
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
        private PluralizationService _jsonPluralizer = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en"));
        private HttpResponseMessage _lastResponse;
        private string _lastResponseBody;

        public string UserName
        {
            get
            {
                return _credentials == null ? null : _credentials.UserName;
            }
        }
        public async Task<bool> SignIn(Credentials credentials)
        {
            try
            {
                _credentials = credentials;

                _apiUrl = credentials.HostName + credentials.ApiVersion;

                _cookieJar = new CookieContainer();

                HttpClientHandler responseHandler = null; ;
                if (!String.IsNullOrWhiteSpace(credentials.ProxyUri))
                    responseHandler = new HttpClientHandler { Proxy = new WebProxy(credentials.ProxyUri, false), UseProxy = true };
                else
                    responseHandler = new HttpClientHandler();

                responseHandler.CookieContainer = _cookieJar;

                _client = new HttpClient(responseHandler);
                _client.Timeout = new TimeSpan(0, 10, 0);
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Uri signInURI = new Uri(_apiUrl + "/signin");
                dynamic data = new { username=credentials.UserName, password=credentials.Password };
                string body = _jsonConverter.Serialize(data);
                HttpResponseMessage response = await _client.PutAsync(signInURI, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);

                _lastResponse = response;

                Task<string> reading = response.Content.ReadAsStringAsync();
                reading.Wait();
                body = reading.Result;
                _lastResponseBody = body;

                if (response.StatusCode != HttpStatusCode.OK)
                    return false;

                data = JObject.Parse(body);
                string apiToken = data.apiToken;

                CookieCollection cookies = _cookieJar.GetCookies(signInURI);
                Cookie sessionCookie = cookies["connect.sid"];

                _client.DefaultRequestHeaders.GetCookies().Add(new CookieHeaderValue(sessionCookie.Name, sessionCookie.Value));
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

                HttpResponseMessage result = await _client.GetAsync(_apiUrl + "/signedin").ConfigureAwait(false);
                _lastResponse = result;

                _lastResponseBody = null;

                return (result.StatusCode == HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        private string BuildQueryString(NameValueCollection nvc)
        {
            return string.Join("&", nvc.AllKeys.Where(key => !string.IsNullOrWhiteSpace(nvc[key])).Select(key => string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(nvc[key]))));
        }    
    }
}
