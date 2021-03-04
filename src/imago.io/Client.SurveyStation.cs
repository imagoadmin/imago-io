using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Threading;
using Imago.IO.Classes;

namespace Imago.IO
{
    public partial class Client
    {
        public class SurveyStationQueryParameters
        {
            public string workspacename { get; set; }
            public string category { get; set; }
        }

        public async Task<Result<List<SurveyStation>>> SearchForSurveyStation(SurveyStationQueryParameters parameters, CancellationToken ct, TimeSpan? timeout = null)
        {
            try
            {
                NameValueCollection query = new NameValueCollection();
                if (!string.IsNullOrWhiteSpace(parameters.workspacename))
                    query["workspacename"] = parameters.workspacename;
                if (!string.IsNullOrWhiteSpace(parameters.category))
                    query["category"] = parameters.category;
                
                return await ClientGet("/surveystation", query, ct, timeout, (response, body) =>
                {
                    this.LogHttpResponse(response);

                    JObject responseObject = JObject.Parse(body);

                    List<SurveyStation> surveystations = _jsonConverter.Deserialize<List<SurveyStation>>(responseObject["surveyStations"].ToString());

                    return surveystations;
                });
            }
            catch (Exception ex)
            {
                Telemetry.TelemetryLogger.Instance?.LogException(ex);
                return new Result<List<SurveyStation>> { Code = ResultCode.failed };
            }
        }


    }
}
