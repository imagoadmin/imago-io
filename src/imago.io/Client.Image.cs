using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.IO;
using Microsoft.Azure.Storage.Blob;

namespace Imago.IO
{
    public partial class Client
    {
        public class ImageQueryParameters
        {
            public Guid imageryId { get; set; } = Guid.Empty;
            public Guid imageTypeId { get; set; } = Guid.Empty;
            public string imageTypeName { get; set; }
            public string url { get; set; } = null;
            public int? resizeWidth { get; set; } = null;
            public int? resizeHeight { get; set; } = null;
        }
        public async Task<Result<string>> DownloadImageToFile(ImageQueryParameters parameters, string fileName, CancellationToken ct, TimeSpan? timeout = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(parameters.url))
                    return await DownloadImageToFile(parameters.url, fileName, ct);

                if (parameters.imageryId == Guid.Empty || parameters.imageTypeId == Guid.Empty)
                    return new Result<string> { Code = ResultCode.failed };

                NameValueCollection query = new NameValueCollection();

                query["imageryid"] = parameters.imageryId.ToString();
                query["imagetypeid"] = parameters.imageTypeId.ToString();
                if (!string.IsNullOrWhiteSpace(parameters.imageTypeName))
                    query["imagetypename"] = parameters.imageTypeName;
                if (parameters.resizeHeight != null)
                    query["height"] = parameters.resizeHeight.Value.ToString();
                if (parameters.resizeWidth != null)
                    query["width"] = parameters.resizeWidth.Value.ToString();

                UriBuilder builder = new UriBuilder(_apiUrl);
                builder.Path += "/image";
                builder.Query = BuildQueryString(query);

                using (HttpClient client = GetClient(timeout))
                {
                    HttpResponseMessage response = await client.GetAsync(builder.ToString(), ct).ConfigureAwait(false);
                    this.LogHttpResponse(response);
                    _lastResponse = response;

                    return await response.ConvertToResult(async (httpResponse, body) =>
                    {
                        if (httpResponse.Content.Headers.ContentDisposition == null || string.IsNullOrWhiteSpace(response.Content.Headers.ContentDisposition.FileName))
                        {
                            return null;
                        }

                        string fsExt = Path.GetExtension(response.Content.Headers.ContentDisposition.FileName.Replace("\"", ""));
                        if (string.IsNullOrWhiteSpace(fsExt))
                        {
                            return null;
                        }

                        // file name comes from a closure.
                        fileName += fsExt;
                        using (FileStream imageFileStream = new FileStream(fileName, FileMode.Create))
                        using (var httpStream = await response.Content.ReadAsStreamAsync())
                        {
                            httpStream.CopyTo(imageFileStream);
                            imageFileStream.Flush();
                        }

                        return fileName;
                    });
                }
            }
            catch (Exception ex)
            {
                Telemetry.TelemetryLogger.Instance?.LogException(ex);
                return new Result<string> { Code = ResultCode.failed };
            }
            finally
            {

            }
        }
        public async Task<Result<string>> DownloadImageToFile(string path, string fileName, CancellationToken ct, TimeSpan? timeout = null)
        {
            try
            {
                CloudBlockBlob blob = new CloudBlockBlob(new Uri(path));
                string uri = blob.Uri.AbsolutePath;
                int ind = uri.LastIndexOf('.');
                if (ind == -1)
                    return new Result<string> { Code = ResultCode.failed };

                string fsExt = uri.Substring(ind);
                if (!fileName.EndsWith(fsExt))
                    fileName += fsExt;

                await blob.DownloadToFileAsync(fileName, FileMode.Create, ct);
                return new Result<string> { Value = fileName, Code = ResultCode.ok };
            }
            catch (Exception ex)
            {
                Telemetry.TelemetryLogger.Instance?.LogException(ex);
                return new Result<string> { Code = ResultCode.failed };
            }
        }
        public class ImageUpdateParameters
        {
            public string imageFileName { get; set; }
            public string mimeType { get; set; }

            public Stream dataStream { get; set; }

            public Guid? imageryId { get; set; }
            public string workspaceName { get; set; }
            public string datasetName { get; set; }
            public Guid? collectionId { get; set; }
            public string collectionName { get; set; }
            public Guid? imageryTypeId { get; set; }
            public string imageryTypeName { get; set; }
            public Guid imageTypeId { get; set; }
            public string imageTypeName { get; set; }
            public string name { get; set; }
            public double? startDepth { get; set; }
            public double? endDepth { get; set; }
            public double? x { get; set; }
            public double? y { get; set; }
            public double? z { get; set; }
        }
        public class ImageUpdateResult
        {
            public Guid ImageId { get; set; } = Guid.Empty;
            public Guid ImageryId { get; set; } = Guid.Empty;
        }
        public async Task<Result<ImageUpdateResult>> UploadImage(ImageUpdateParameters parameters, CancellationToken ct, TimeSpan? timeout = null)
        {
            Stream fs = null;
            Result<ImageUpdateResult> result = null;
            try
            {
                if (String.IsNullOrWhiteSpace(parameters.imageFileName))
                    parameters.imageFileName = "unspecified.blob";
                if (String.IsNullOrWhiteSpace(parameters.mimeType))
                    parameters.mimeType = "image/jpeg";

                Telemetry.TelemetryLogger.Instance?.LogEvent(Telemetry.TelemetryEvents.ClientAddImagery, new Dictionary<string, string> {
                    { "imageFileName", parameters.imageFileName },
                    { "imageryId", parameters.imageryId?.ToString() },
                    { "workspaceName", parameters.workspaceName },
                    { "datasetName", parameters.datasetName },
                    { "collectionId", parameters.collectionId?.ToString() },
                    { "collectionName", parameters.collectionName },
                    { "imageryTypeId", parameters.imageryTypeId?.ToString() },
                    { "imageryTypeName", parameters.imageryTypeName },
                    { "imageTypeId", parameters.imageTypeId.ToString() },
                    {"imageTypeName", parameters.imageryTypeName },
                    { "mimeType", parameters.mimeType },
                    { "size", parameters.dataStream?.Length.ToString() },
                    { "x", parameters.x?.ToString() },
                    { "y", parameters.y?.ToString() },
                    { "z", parameters.z?.ToString() }
                });

                var hasWorkspace = !string.IsNullOrEmpty(parameters.workspaceName);
                var hasDataset = !string.IsNullOrEmpty(parameters.datasetName);
                var hasCollection = !string.IsNullOrEmpty(parameters.collectionName);
                var hasImageryType = !string.IsNullOrEmpty(parameters.imageryTypeName);
                var hasImageType = !string.IsNullOrEmpty(parameters.imageTypeName);

                var byId = parameters.imageryId != null && parameters.imageryId.Value != default;
                var byName = hasWorkspace && hasDataset && hasCollection && hasImageryType && hasImageType;

                if (!byId && !byName) return new Result<ImageUpdateResult> { Code = ResultCode.failed };

                NameValueCollection query = new NameValueCollection();

                if (byId) query["imageryid"] = parameters.imageryId.ToString();
                if (hasWorkspace) query["workspaceName"] = parameters.workspaceName;
                if (hasDataset) query["datasetName"] = parameters.datasetName;
                if (hasCollection) query["collectionName"] = parameters.collectionName;
                if (hasImageryType) query["imageryTypeName"] = parameters.imageryTypeName;
                if (hasImageType) query["imageTypeName"] = parameters.imageTypeName;
                if (parameters.collectionId != default) query["collectionid"] = parameters.collectionId?.ToString();
                if (parameters.imageryTypeId != default) query["imagerytypeid"] = parameters.imageryTypeId?.ToString();
                if (parameters.imageryTypeId != default) query["imagetypeid"] = parameters.imageTypeId.ToString();

                query["mimetype"] = parameters.mimeType;
                query["name"] = parameters.name?.ToString();
                query["startdepth"] = parameters.startDepth?.ToString();
                query["enddepth"] = parameters.endDepth?.ToString();
                query["x"] = parameters.x?.ToString();
                query["y"] = parameters.y?.ToString();
                query["z"] = parameters.z?.ToString();

                UriBuilder builder = new UriBuilder(_apiUrl);
                builder.Path += "/image";
                builder.Query = BuildQueryString(query);

                fs = parameters.dataStream;
                if (fs == null)
                    fs = new FileStream(parameters.imageFileName, FileMode.Open);

                // Don't pass in a custom boundary, because passing a custom boundary 
                // caused an exception when running on Android.
                MultipartFormDataContent content = new MultipartFormDataContent();
                HttpContent streamedContent = new StreamContent(fs);
                content.Add(streamedContent, "file", parameters.imageFileName);

                using (HttpClient client = GetClient(timeout))
                {
                    HttpResponseMessage response = await client.PostAsync(builder.ToString(), content, ct).ConfigureAwait(false);
                    this.LogHttpResponse(response);
                    _lastResponse = response;

                    return await response.ConvertToResult((httpResponse, body) => _jsonConverter.Deserialize<ImageUpdateResult>(body));
                }
            }
            catch (Exception ex)
            {
                Telemetry.TelemetryLogger.Instance?.LogException(ex, new Dictionary<string, string> {
                    { "imageFileName", parameters.imageFileName },
                    { "imageryId", parameters.imageryId?.ToString() },
                    { "workspaceName", parameters.workspaceName },
                    { "datasetName", parameters.datasetName },
                    { "collectionId", parameters.collectionId?.ToString() },
                    { "collectionName", parameters.collectionName },
                    { "imageryTypeId", parameters.imageryTypeId?.ToString() },
                    { "imageryTypeName", parameters.imageryTypeName },
                    { "imageTypeId", parameters.imageTypeId.ToString() },
                    {"imageTypeName", parameters.imageryTypeName },
                    { "mimeType", parameters.mimeType },
                    { "size", parameters.dataStream?.Length.ToString() },
                    { "x", parameters.x?.ToString() },
                    { "y", parameters.y?.ToString() },
                    { "z", parameters.z?.ToString() }
                });
                result = new Result<ImageUpdateResult> { Code = ResultCode.failed, Message = "Exception " + ex.Message + Environment.NewLine + ex.ToString() };
            }
            finally
            {
                if (fs != null)
                    fs.Close();

            }
            return result;
        }

        public class ImageProperties
        {
            public string mimeType { get; set; }
            public Guid id { get; set; }
            public string fileName { get; set; }
            public string fileExt { get; set; }
            public Int64 fileSize { get; set; } = 0;
            public String checksum { get; set; }
        }

        public async Task<Result<ImageProperties>> GetImageProperties(Guid id, CancellationToken ct, TimeSpan? timeout = null)
        {
            try
            {
                if (id == Guid.Empty)
                    return new Result<ImageProperties> { Code = ResultCode.failed };

                NameValueCollection query = new NameValueCollection();

                query["id"] = id.ToString();
                query["type"] = "image";
                query["group"] = "properties";

                return await ClientGet("/attributes", query, ct, timeout, (response, body) =>
                {
                    JObject responseObject = JObject.Parse(body);

                    ImageProperties imageProperties = _jsonConverter.Deserialize<ImageProperties>(responseObject["attributes"].ToString());

                    return imageProperties;
                });
            }
            catch (Exception ex)
            {
                Telemetry.TelemetryLogger.Instance?.LogException(ex);
                return new Result<ImageProperties> { Code = ResultCode.failed };
            }
        }


        public async Task<Result<string>> GetImageAttributes(Guid id, string groupName, CancellationToken ct, TimeSpan? timeout = null)
        {
            try
            {
                if (id == Guid.Empty)
                    return new Result<string> { Code = ResultCode.failed };

                var query = new NameValueCollection();

                query["id"] = id.ToString();
                query["type"] = "image";
                query["group"] = groupName;

                return await ClientGet("/attributes", query, ct, timeout, (response, body) =>
                {
                    var responseObject = JObject.Parse(body);

                    var result = responseObject["attributes"].ToString();

                    return result;
                });
            }
            catch (Exception ex)
            {
                Telemetry.TelemetryLogger.Instance?.LogException(ex);
                return new Result<string> { Code = ResultCode.failed };
            }
        }


        public async Task<Result<object>> SetImageAttributes(Guid id, string groupName, object attributeData, CancellationToken ct, TimeSpan? timeout = null)
        {
            try
            {
                if (id == Guid.Empty)
                    return new Result<object> { Code = ResultCode.failed };

                var query = new NameValueCollection();

                query["id"] = id.ToString();
                query["type"] = "image";
                query["group"] = groupName;
                UriBuilder builder = new UriBuilder(_apiUrl);
                builder.Path += "/attributes";
                builder.Query = BuildQueryString(query);

                return await ClientPost(builder, attributeData, timeout, ct, (response, body) =>
                {
                    return (object)true;
                });
            }
            catch (Exception ex)
            {
                Telemetry.TelemetryLogger.Instance?.LogException(ex);
                return new Result<object> { Code = ResultCode.failed };
            }
        }
    }
}
