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
        public class ImageQueryParameters
        {
            public Guid imageryId { get; set; } = Guid.Empty;
            public Guid imageTypeId { get; set; } = Guid.Empty;
            public bool overwriteExisting { get; set; } = false;
            public int? resizeWidth { get; set; } = null;
            public int? resizeHeight { get; set; } = null;
        }
        public async Task<Result<string>> DownloadImageToFile(ImageQueryParameters parameters, string fileName, CancellationToken ct, TimeSpan? timeout = null)
        {
            try
            {
                if (parameters.imageryId == Guid.Empty || parameters.imageTypeId == Guid.Empty)
                    return new Result<string> { Code = ResultCode.failed };

                NameValueCollection query = new NameValueCollection();

                query["imageryid"] = parameters.imageryId.ToString();
                query["imagetypeid"] = parameters.imageTypeId.ToString();
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
                    _lastResponse = response;


                    if (response.StatusCode != HttpStatusCode.OK)
                        return new Result<string> { Code = response.GetResultCode(), Message = response.StatusCode.ToString() };

                    if (response.Content.Headers.ContentDisposition == null || String.IsNullOrWhiteSpace(response.Content.Headers.ContentDisposition.FileName) || response.StatusCode != HttpStatusCode.OK)
                        return new Result<string> { Code = ResultCode.failed };

                    string fsExt = Path.GetExtension(response.Content.Headers.ContentDisposition.FileName.Replace("\"", ""));
                    if (String.IsNullOrWhiteSpace(fsExt))
                        return new Result<string> { Code = ResultCode.failed };

                    fileName += fsExt;
                    using (FileStream imageFileStream = new FileStream(fileName, FileMode.Create))
                    using (var httpStream = await response.Content.ReadAsStreamAsync())
                    {
                        httpStream.CopyTo(imageFileStream);
                        imageFileStream.Flush();
                    }

                    return new Result<string> { Value = fileName, Code = response.GetResultCode() };
                }
            }
            catch (Exception ex)
            {
                this.LogTracer.TrackError(ex);
                return new Result<string> { Code = ResultCode.failed };
            }
            finally
            {

            }
        }
        public class ImageUpdateParameters
        {
            public string imageFileName { get; set; }
            public string mimeType { get; set; }

            public Stream dataStream { get; set; }

            public Guid? imageryId { get; set; }
            public Guid? collectionId { get; set; }
            public Guid? imageryTypeId { get; set; }
            public Guid imageTypeId { get; set; }
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

                this.LogTracer.TrackEvent("Client.AddImagery()", new Dictionary<string, string> {
                    { "imageFileName", parameters.imageFileName },
                    { "imageryId", parameters.imageryId?.ToString() },
                    { "collectionId", parameters.collectionId?.ToString() },
                    { "imageryTypeId", parameters.imageryTypeId?.ToString() },
                    { "imageTypeId", parameters.imageTypeId.ToString() },
                    { "mimeType", parameters.mimeType },
                    { "size", parameters.dataStream?.Length.ToString() },
                    { "x", parameters.x?.ToString() },
                    { "y", parameters.y?.ToString() },
                    { "z", parameters.z?.ToString() },
                });

                if (parameters.imageryId == Guid.Empty || parameters.imageTypeId == Guid.Empty)
                    return new Result<ImageUpdateResult> { Code = ResultCode.failed };

                NameValueCollection query = new NameValueCollection();

                if (parameters.imageryId.HasValue) query["imageryid"] = parameters.imageryId.ToString();
                query["collectionid"] = parameters.collectionId?.ToString();
                query["imagerytypeid"] = parameters.imageryTypeId?.ToString();
                query["imagetypeid"] = parameters.imageTypeId.ToString();
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
                    _lastResponse = response;
                    string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    _lastResponseBody = body;
                    ImageUpdateResult imageResult = _jsonConverter.Deserialize<ImageUpdateResult>(body);
                    result = new Result<ImageUpdateResult> { Value = imageResult, Code = imageResult == null || response.StatusCode != HttpStatusCode.OK ? response.GetResultCode() : ResultCode.ok, Message = $"HTTP error {response.StatusCode} {body}" };
                }
            }
            catch (Exception ex)
            {
                this.LogTracer.TrackError(ex);
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
                this.LogTracer.TrackError(ex);
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

                    var result =  responseObject["attributes"].ToString();

                    return result;
                });
            }
            catch (Exception ex)
            {
                this.LogTracer.TrackError(ex);
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
                this.LogTracer.TrackError(ex);
                return new Result<object> { Code = ResultCode.failed };
            }
        }
    }
}
