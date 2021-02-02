using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
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
                    this.LogHttpResponse(response);

                    JObject context = JObject.Parse(body);

                    var workspaces = JsonConvert.DeserializeObject<List<Workspace>>(context["workspaces"].ToString(), _jsonSettings);

                    return new UserContext { Workspaces = workspaces };
                });
            }
            catch (Exception ex)
            {
                Telemetry.TelemetryLogger.Instance?.LogException(ex);
                return new Result<UserContext> { Code = ResultCode.failed };
            }
        }

        public async Task<Result<string>> GetWorkspaceSASUrl(Guid id, Guid? imageId = null, string mimeType = null, TimeSpan? timeout = null)
        {
            try
            {
                NameValueCollection query = new NameValueCollection();
                query["workspaceid"] = id.ToString();
                if (imageId != null)
                {
                    query["imageid"] = imageId.ToString();
                    query["mimetype"] = mimeType;
                }

                return await ClientGet("/access", query, new CancellationToken(false), timeout, (response, body) =>
                {
                    this.LogHttpResponse(response);
                    JObject responseObject = JObject.Parse(body);

                    var url = responseObject["url"].ToString();
                    return url;
                });
            }
            catch (Exception ex)
            {
                Telemetry.TelemetryLogger.Instance?.LogException(ex);
                return new Result<string> { Code = ResultCode.failed };
            }
        }

        public async Task<Result<string>> GetProfiles(string filter = null, TimeSpan? timeout = null)
        {
            try
            {
                var query = new NameValueCollection();
                if (!string.IsNullOrWhiteSpace(filter))
                    query.Add("filter", filter);

                var profiles = await ClientGet("/profile", query, new CancellationToken(false), timeout, (response, body) =>
                {
                    return body;
                });

                return profiles;
            }
            catch (Exception ex)
            {
                Telemetry.TelemetryLogger.Instance?.LogException(ex);
                return new Result<string> { Code = IO.ResultCode.failed };
            }
        }
    }
}
