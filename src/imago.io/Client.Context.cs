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
using Newtonsoft.Json;
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
      
        public async Task<Result<UserContext>> GetUserContext(TimeSpan? timeout = null)
        {
            try
            {
                string query = "/context";

                using (HttpClient client = GetClient(timeout))
                {
                    HttpResponseMessage response = await client.GetAsync(_apiUrl + query).ConfigureAwait(false);
                    _lastResponse = response;

                    string body = await response.Content.ReadAsStringAsync();
                    _lastResponseBody = body;

                    JObject context = JObject.Parse(body);

                    List<Project> projects = JsonConvert.DeserializeObject<List<Project>>(context["projects"].ToString(), _jsonSettings);
                    return new Result<UserContext> { Value = new UserContext { Projects = projects }, Code = projects == null || response.StatusCode != HttpStatusCode.OK ? ResultCode.failed : ResultCode.ok };
                }
            }
            catch(Exception ex)
            {
                return new Result<UserContext> { Code = ResultCode.failed };
            }
        }

        public async Task<Result<string>> GetProjectSASUrl(Guid id, Guid? imageId = null, string mimeType = null)
        {
            try
            {
                NameValueCollection query = new NameValueCollection();
                query["projectid"] = id.ToString();
                if (imageId != null)
                {
                    query["imageid"] = imageId.ToString();
                    query["mimetype"] = mimeType;
                }

                UriBuilder builder = new UriBuilder(_apiUrl);
                builder.Path += "/access";
                builder.Query = BuildQueryString(query);

                HttpResponseMessage response = await _client.GetAsync(builder.ToString());
                _lastResponse = response;
                string body = await response.Content.ReadAsStringAsync();
                _lastResponseBody = body;

                if (response.StatusCode != HttpStatusCode.OK)
                    return new Result<string> { Code = ResultCode.failed };

                JObject responseObject = JObject.Parse(body);
                return new Result<string> { Value = responseObject["url"].ToString(), Code = responseObject == null || response.StatusCode != HttpStatusCode.OK? ResultCode.failed : ResultCode.ok };
            }
            catch (Exception ex)
            {
                return new Result<string> { Code = ResultCode.failed };
            }
        }
    }
}
