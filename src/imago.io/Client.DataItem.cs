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
     
        public class DataItemQueryParameters
        {
            public Guid dataEntityId { get; set; }
            public Guid dataSeriesTypeId { get; set; }
            public string name { get; set; }
            public double? startDepth { get; set; }
            public double? endDepth { get; set; }
            public double? x { get; set; }
            public double? y { get; set; }
            public double? z { get; set; }
        }

        public async Task<Result<List<DataItem>>> SearchForDataItem(DataItemQueryParameters parameters, CancellationToken ct, TimeSpan? timeout = null)
        {
            try
            {
                if (parameters.dataSeriesTypeId == Guid.Empty || parameters.dataEntityId == Guid.Empty)
                    return new Result<List<DataItem>> { Code = ResultCode.failed };

                NameValueCollection query = new NameValueCollection();

                query["dataentityid"] = parameters.dataEntityId.ToString();
                query["dataseriestypeid"] = parameters.dataSeriesTypeId.ToString();
                if (!String.IsNullOrWhiteSpace(parameters.name))
                    query["name"] = parameters.name;
                if (parameters.startDepth != null)
                    query["startdepth"] = parameters.startDepth.ToString();
                if (parameters.endDepth != null)
                    query["enddepth"] = parameters.endDepth.ToString();
                if (parameters.x != null)
                    query["x"] = parameters.x.ToString();
                if (parameters.y != null)
                    query["y"] = parameters.y.ToString();
                if (parameters.z != null)
                    query["z"] = parameters.z.ToString();

                return await ClientGet("/dataitem", query, ct, timeout, (response, body) =>
                {
                    JObject responseObject = JObject.Parse(body);
                    List<DataItem> dataItems = _jsonConverter.Deserialize<List<DataItem>>(responseObject["dataItems"].ToString());
                    return dataItems;
                });
            }
            catch (Exception ex)
            {
                this.LogTracer.TrackError(ex);
                return new Result<List<DataItem>> { Code = ResultCode.failed };
            }
        }
        public class DataItemUpdateParameters
        {
            public Guid? id { get; set; }
            public Guid dataEntityId { get; set; } = Guid.Empty;
            public Guid dataSeriesTypeId { get; set; } = Guid.Empty;
            public string name { get; set; }
            public double? startDepth { get; set; }
            public double? endDepth { get; set; }
            public double? x { get; set; }
            public double? y { get; set; }
            public double? z { get; set; }
        }

        public async Task<Result<DataItem>> AddDataItem(DataItemUpdateParameters parameters, CancellationToken ct, TimeSpan? timeout = null)
        {
            try
            {
                if (parameters.dataEntityId == Guid.Empty && parameters.dataSeriesTypeId == Guid.Empty)
                    return new Result<DataItem> { Code = ResultCode.failed };

                UriBuilder builder = new UriBuilder(_apiUrl);
                builder.Path += "/dataitem";

                return await ClientPost(builder, parameters, timeout, ct, (response, body) =>
                {
                    DataItem dataItem = _jsonConverter.Deserialize<DataItem>(body);
                    return dataItem;
                });
            }
            catch (Exception ex)
            {
                this.LogTracer.TrackError(ex);
                return new Result<DataItem> { Code = ResultCode.failed };
            }
        }
    }
}
