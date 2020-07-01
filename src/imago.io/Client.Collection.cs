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
        public static class CollectionQueryParametersMatchChoices
        {
            public const string MatchEqual = "equals";
            public const string MatchLike = "like";
        }

        public class CollectionQueryParameters
        {
            public string name { get; set; }
            public string match { get; set; }
            public Guid datasetId { get; set; }
            public string workspacename { get; set; }
            public string datasetname { get; set; }
        }

        public async Task<Result<List<Collection>>> SearchForCollection(CollectionQueryParameters parameters, CancellationToken ct, TimeSpan? timeout = null)
        {
            try
            {
                NameValueCollection query = new NameValueCollection();
                if (!string.IsNullOrWhiteSpace(parameters.workspacename))
                    query["workspacename"] = parameters.workspacename;
                if (!string.IsNullOrWhiteSpace(parameters.datasetname))
                    query["datasetname"] = parameters.datasetname;
                query["name"] = parameters.name;
                query["match"] = parameters.match;

                if (parameters.datasetId != Guid.Empty)
                    query["datasetid"] = parameters.datasetId.ToString();

                return await ClientGet("/collection", query, ct, timeout, (response, body) =>
                {
                    this.LogHttpResponse(response);

                    JObject responseObject = JObject.Parse(body);

                    List<Collection> collections = _jsonConverter.Deserialize<List<Collection>>(responseObject["collections"].ToString());

                    return collections;
                });
            }
            catch (Exception ex)
            {
                this.LogTracer.TrackError(ex);
                return new Result<List<Collection>> { Code = ResultCode.failed };
            }
        }

        public class CollectionUpdateParameters
        {
            public Guid? id { get; set; }
            public Guid datasetId { get; set; }
            public string name { get; set; }
        }
        public async Task<Result<Collection>> AddCollection(CollectionUpdateParameters parameters, CancellationToken ct, TimeSpan? timeout = null)
        {
            try
            {
                if (parameters.datasetId == Guid.Empty)
                    return new Result<Collection> { Code = ResultCode.failed };

                UriBuilder builder = new UriBuilder(_apiUrl);
                builder.Path += "/collection";

                return await ClientPost(builder, parameters, timeout, ct, (response, body) =>
                {
                    this.LogHttpResponse(response);

                    Collection collection = _jsonConverter.Deserialize<Collection>(body);
                    return collection;
                });

            }
            catch (Exception ex)
            {
                this.LogTracer.TrackError(ex);
                return new Result<Collection> { Code = ResultCode.failed, Message = ex.Message };
            }
        }
        public async Task<Result<List<Collection>>> AddCollectionBulk(List<CollectionUpdateParameters> parameters, CancellationToken ct, TimeSpan? timeout = null)
        {
            try
            {
                if (parameters.Count == 0)
                    return new Result<List<Collection>> { Code = ResultCode.ok, Value=new List<Collection>() };

                UriBuilder builder = new UriBuilder(_apiUrl);
                builder.Path += "/collection";

                return await ClientPost(builder, parameters, timeout, ct, (response, body) =>
                {
                    this.LogHttpResponse(response);

                    List<Collection> collection = _jsonConverter.Deserialize<List<Collection>>(body);
                    return collection;
                });

            }
            catch (Exception ex)
            {
                this.LogTracer.TrackError(ex);
                return new Result<List<Collection>> { Code = ResultCode.failed, Message = ex.Message };
            }
        }


    }
}
