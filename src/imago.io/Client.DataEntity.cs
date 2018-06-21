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
        public static class DataEntityQueryParametersMatchChoices
        {
            public const string MatchEqual = "equals";
            public const string MatchLike = "like";
        }

        public class DataEntityQueryParameters
        {
            public string name { get; set; }
            public string match { get; set; }
            public Guid datasetId { get; set; }
        }

        public async Task<Result<List<DataEntity>>> SearchForDataEntity(DataEntityQueryParameters parameters, CancellationToken ct, TimeSpan? timeout = null)
        {
            try
            {
                NameValueCollection query = new NameValueCollection();
                query["name"] = parameters.name;
                query["match"] = parameters.match;
                if (parameters.datasetId != Guid.Empty)
                    query["datasetid"] = parameters.datasetId.ToString();

                return await ClientGet("/dataentity", query, ct, timeout, (response, body) =>
                {
                    JObject responseObject = JObject.Parse(body);

                    List<DataEntity> dataEntities = _jsonConverter.Deserialize<List<DataEntity>>(responseObject["dataEntities"].ToString());

                    return dataEntities;
                });
            }
            catch (Exception ex)
            {
                this.LogTracer.TrackError(ex);
                return new Result<List<DataEntity>> { Code = ResultCode.failed };
            }
        }

        public class DataEntityUpdateParameters
        {
            public Guid datasetId { get; set; }
            public string name { get; set; }
        }
        public async Task<Result<DataEntity>> AddDataEntity(DataEntityUpdateParameters parameters, CancellationToken ct, TimeSpan? timeout = null)
        {
            try
            {
                if (parameters.datasetId == Guid.Empty)
                    return new Result<DataEntity> { Code = ResultCode.failed };

                UriBuilder builder = new UriBuilder(_apiUrl);
                builder.Path += "/dataentity";

                return await ClientPost(builder, parameters, timeout, ct, (response, body) =>
                {
                    DataEntity dataEntity = _jsonConverter.Deserialize<DataEntity>(body);
                    return dataEntity;
                });

            }
            catch (Exception ex)
            {
                this.LogTracer.TrackError(ex);
                return new Result<DataEntity> { Code = ResultCode.failed, Message = ex.Message };
            }
        }


    }
}
