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

        public async Task<Result<UserContext>> GetUserContext(TimeSpan? timeout = null)
        {
            try
            {
                return await ClientGet("/context", new NameValueCollection(), new CancellationToken(false), timeout, (response, body) =>
                {
                    JObject context = JObject.Parse(body);

                    List<Project> projects = JsonConvert.DeserializeObject<List<Project>>(context["projects"].ToString(), _jsonSettings);

                    return new UserContext { Projects = projects };
                });
            }
            catch (Exception ex)
            {
                this.LogTracer.TrackError(ex);
                return new Result<UserContext> { Code = ResultCode.failed };
            }
        }

        public async Task<Result<string>> GetProjectSASUrl(Guid id, Guid? imageId = null, string mimeType = null, TimeSpan? timeout = null)
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

                return await ClientGet("/access", query, new CancellationToken(false), timeout, (response, body) =>
                {
                    JObject responseObject = JObject.Parse(body);

                    var url = responseObject["url"].ToString();
                    return url;
                });
            }
            catch (Exception ex)
            {
                this.LogTracer.TrackError(ex);
                return new Result<string> { Code = ResultCode.failed };
            }
        }
    }
}
