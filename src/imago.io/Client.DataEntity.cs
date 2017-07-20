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
     
        public class DataEntityQueryParameters
        {
            public string name { get; set; }
            public string match { get; set; }
            public Guid datasetId { get; set; }
        }

        public async Task<Result<List<DataEntity>>> SearchForDataEntity(DataEntityQueryParameters parameters, CancellationToken ct)
        {
            try
            {
                NameValueCollection query = new NameValueCollection();
                if (_credentials.ApiVersion == Credentials.ImagoApiVersion1)
                    query["type"] = "dataentity";
                query["name"] = parameters.name;
                query["match"] = parameters.match;
                if (parameters.datasetId != Guid.Empty)
                    query["datasetid"] = parameters.datasetId.ToString();

                UriBuilder builder = new UriBuilder(_apiUrl);
                builder.Path +=  _credentials.ApiVersion == Credentials.ImagoApiVersion2 ? "/dataentity" : "/query";
                builder.Query = BuildQueryString(query);

                HttpResponseMessage response = await _client.GetAsync(builder.ToString(),ct);
                _lastResponse = response;
                string body = await response.Content.ReadAsStringAsync();
                _lastResponseBody = body;

                List<DataEntity> dataEntities = _jsonConverter.Deserialize<List<DataEntity>>(body);
                return new Result<List<DataEntity>> { Value = dataEntities, Code = dataEntities == null || response.StatusCode != HttpStatusCode.OK ? ResultCode.failed : ResultCode.ok };
            }
            catch
            {
                return new Result<List<DataEntity>> { Code = ResultCode.failed };
            }
        }

        public class DataEntityUpdateParameters
        {
            public Guid datasetId { get; set; }
            public string name { get; set; }
        }
        public class DataEntityUpdateBody
        {
            public string name { get; set; }
        }
        public async Task<Result<DataEntity>> AddDataEntity(DataEntityUpdateParameters parameters,CancellationToken ct)
        {
            try
            {
                if (parameters.datasetId == Guid.Empty)
                    return new Result<DataEntity> { Code = ResultCode.failed };

                NameValueCollection query = new NameValueCollection();
                if (_credentials.ApiVersion == Credentials.ImagoApiVersion1)
                    query["type"] = "dataentity";
                query["datasetid"] = parameters.datasetId.ToString();

                DataEntityUpdateBody data = new DataEntityUpdateBody();

                if (!String.IsNullOrWhiteSpace(parameters.name))
                    data.name = parameters.name;
                UriBuilder builder = new UriBuilder(_apiUrl);
                builder.Path += _credentials.ApiVersion == Credentials.ImagoApiVersion2 ? "/dataentity" : "/update";
                builder.Query = BuildQueryString(query);

                string body = _jsonConverter.Serialize(data);
                HttpResponseMessage response = await _client.PostAsync(builder.ToString(), new StringContent(body, Encoding.UTF8, "application/json"),ct).ConfigureAwait(false);
                _lastResponse = response;
                body = await response.Content.ReadAsStringAsync();
                _lastResponseBody = body;

                DataEntity dataEntity = _jsonConverter.Deserialize<DataEntity>(body);
                return new Result<DataEntity> { Value = dataEntity, Code = dataEntity == null || response.StatusCode != HttpStatusCode.OK ? ResultCode.failed : ResultCode.ok };
            }
            catch
            {
                return new Result<DataEntity> { Code = ResultCode.failed };
            }
        }      
    }
}
