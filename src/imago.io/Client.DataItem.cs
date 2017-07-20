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
        public async Task<Result<List<DataItem>>> SearchForDataItem(DataItemQueryParameters parameters, CancellationToken ct)
        {
            try
            {
                if (parameters.dataSeriesTypeId == Guid.Empty || parameters.dataEntityId == Guid.Empty)
                    return new Result<List<DataItem>> { Code = ResultCode.failed };

                NameValueCollection query = new NameValueCollection();
                if (_credentials.ApiVersion == Credentials.ImagoApiVersion1)
                    query["type"] = "dataitem";

                query["dataentityid"] = parameters.dataEntityId.ToString();
                query["dstypeid"] = parameters.dataSeriesTypeId.ToString();
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

                UriBuilder builder = new UriBuilder(_apiUrl);
                builder.Path += _credentials.ApiVersion == Credentials.ImagoApiVersion2 ? "/dataitem" : "/query";
                builder.Query = BuildQueryString(query);

                HttpResponseMessage response = await _client.GetAsync(builder.ToString(),ct).ConfigureAwait(false);
                _lastResponse = response;
                string body = await response.Content.ReadAsStringAsync();
                _lastResponseBody = body;

                List<DataItem> dataItems = _jsonConverter.Deserialize<List<DataItem>>(body);
                return new Result<List<DataItem>> { Value = dataItems, Code = dataItems == null || response.StatusCode != HttpStatusCode.OK ? ResultCode.failed : ResultCode.ok };
            }
            catch
            {
                return new Result<List<DataItem>> { Code = ResultCode.failed };
            }
        }
        public class DataItemUpdateParameters
        {
            public Guid dataEntityId { get; set; } = Guid.Empty;
            public Guid dataSeriesTypeId { get; set; } = Guid.Empty;
            public string name { get; set; }
            public double? startDepth { get; set; }
            public double? endDepth { get; set; }
            public double? x { get; set; }
            public double? y { get; set; }
            public double? z { get; set; }
        }
        private class DataItemUpdateBody
        {
            public string name { get; set; }
            public double? startdepth { get; set; }
            public double? enddepth { get; set; }
            public double? x { get; set; }
            public double? y { get; set; }
            public double? z { get; set; }
        }
        public async Task<Result<DataItem>> AddDataItem(DataItemUpdateParameters parameters, CancellationToken ct)
        {
            try
            {
                NameValueCollection query = new NameValueCollection();
                if (_credentials.ApiVersion == Credentials.ImagoApiVersion1)
                    query["type"] = "dataitem";
                if (parameters.dataEntityId != Guid.Empty && parameters.dataSeriesTypeId != Guid.Empty)
                {
                    query["dataentityid"] = parameters.dataEntityId.ToString();
                    query["dstypeid"] = parameters.dataSeriesTypeId.ToString();
                }
                else
                    return new Result<DataItem> { Code = ResultCode.failed };

                DataItemUpdateBody data = new DataItemUpdateBody();

                if (!String.IsNullOrWhiteSpace(parameters.name))
                    data.name = parameters.name;
                if (parameters.startDepth != null)
                    data.startdepth = parameters.startDepth;
                if (parameters.endDepth != null)
                    data.enddepth = parameters.endDepth;
                if (parameters.x != null)
                    data.x = parameters.x;
                if (parameters.y != null)
                    data.y = parameters.y;
                if (parameters.z != null)
                    data.z = parameters.z;
                UriBuilder builder = new UriBuilder(_apiUrl);
                builder.Path += _credentials.ApiVersion == Credentials.ImagoApiVersion2 ? "/dataitem" : "/update";
                builder.Query = BuildQueryString(query);

                string body = _jsonConverter.Serialize(data);
                HttpResponseMessage response = await _client.PostAsync(builder.ToString(), new StringContent(body, Encoding.UTF8, "application/json"),ct).ConfigureAwait(false);
                _lastResponse = response;
                body = await response.Content.ReadAsStringAsync();
                _lastResponseBody = body;

                DataItem dataItem = _jsonConverter.Deserialize<DataItem>(body);
                return new Result<DataItem> { Value = dataItem, Code = dataItem == null || response.StatusCode != HttpStatusCode.OK ? ResultCode.failed : ResultCode.ok };
            }
            catch
            {
                return new Result<DataItem> { Code = ResultCode.failed };
            }
        }
    }
}
