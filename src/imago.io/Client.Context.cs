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
      
        public async Task<Result<UserContext>> GetUserContext()
        {
            try
            {
                string query = (_credentials.ApiVersion == Credentials.ImagoApiVersion2 ? "/context" : "/query?type=project");

                    HttpResponseMessage response = await _client.GetAsync(_apiUrl + query).ConfigureAwait(false);
                _lastResponse = response;

                string body = await response.Content.ReadAsStringAsync();
                _lastResponseBody = body;

                List<Project> projects = _jsonConverter.Deserialize<List<Project>>(body);
                return new Result<UserContext> { Value = new UserContext { Projects = projects }, Code = projects == null || response.StatusCode != HttpStatusCode.OK ? ResultCode.failed : ResultCode.ok };
            }
            catch
            {
                return new Result<UserContext> { Code = ResultCode.failed };
            }
        }
    }
}
