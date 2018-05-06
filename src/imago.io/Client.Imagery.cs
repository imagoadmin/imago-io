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
        public class ImageryQueryParameters
        {
            public Guid dataItemId { get; set; } = Guid.Empty;
            public Guid imageryTypeId { get; set; } = Guid.Empty;
            public bool overwriteExisting { get; set; } = false;
            public int? resizeWidth { get; set; } = null;
            public int? resizeHeight { get; set; } = null;
        }
        public async Task<Result<string>> DownloadImageryToFile(ImageryQueryParameters parameters, string fileName, CancellationToken ct, TimeSpan? timeout = null)
        {
            try
            {
                if (parameters.dataItemId == Guid.Empty || parameters.imageryTypeId == Guid.Empty)
                    return new Result<string> { Code = ResultCode.failed };

                NameValueCollection query = new NameValueCollection();

                query["dataitemid"] = parameters.dataItemId.ToString();
                query["imagerytypeid"] = parameters.imageryTypeId.ToString();
                if (parameters.resizeHeight != null)
                    query["height"] = parameters.resizeHeight.Value.ToString();
                if (parameters.resizeWidth != null)
                    query["width"] = parameters.resizeWidth.Value.ToString();

                UriBuilder builder = new UriBuilder(_apiUrl);
                builder.Path += "/imagery";
                builder.Query = BuildQueryString(query);

                using (HttpClient client = GetClient(timeout))
                {
                    HttpResponseMessage response = await client.GetAsync(builder.ToString(), ct).ConfigureAwait(false);
                    _lastResponse = response;

                    if (response.StatusCode != HttpStatusCode.OK || response.Content.Headers.ContentDisposition == null || String.IsNullOrWhiteSpace(response.Content.Headers.ContentDisposition.FileName) || response.StatusCode != HttpStatusCode.OK)
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

                    return new Result<string> { Value = fileName, Code = response.StatusCode != HttpStatusCode.OK ? ResultCode.failed : ResultCode.ok };
                }
            }
            catch (Exception ex)
            {
                return new Result<string> { Code = ResultCode.failed };
            }
            finally
            {

            }
        }
        public class ImageryUpdateParameters
        {
            public string imageFileName { get; set; }
            public string mimeType { get; set; }
            public Guid imageryTypeId { get; set; }
            public Guid dataItemId { get; set; }
            public Stream dataStream { get; set; }
            public bool replaceHistory { get; set; } = false;
        }

        public async Task<Result<Imagery>> AddImagery(ImageryUpdateParameters parameters, CancellationToken ct, TimeSpan? timeout = null)
        {
            Stream fs = null;
            Result<Imagery> result = null;
            try
            {
                if (parameters.dataItemId == Guid.Empty || parameters.imageryTypeId == Guid.Empty || String.IsNullOrWhiteSpace(parameters.imageFileName))
                    return new Result<Imagery> { Code = ResultCode.failed };

                NameValueCollection query = new NameValueCollection();
                query["dataitemid"] = parameters.dataItemId.ToString();
                query["imagerytypeid"] = parameters.imageryTypeId.ToString();
                query["mimetype"] = parameters.mimeType;
                if (parameters.replaceHistory)
                    query["history"] = "replace";

                UriBuilder builder = new UriBuilder(_apiUrl);
                builder.Path += "/imagery";
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
                    Imagery dataItem = _jsonConverter.Deserialize<Imagery>(body);
                    result = new Result<Imagery> { Value = dataItem, Code = dataItem == null || response.StatusCode != HttpStatusCode.OK ? ResultCode.failed : ResultCode.ok, Message = $"HTTP error {response.StatusCode} {body}" };
                }
            }
            catch (Exception ex)
            {
                result = new Result<Imagery> { Code = ResultCode.failed, Message = "Exception " + ex.Message + Environment.NewLine + ex.ToString() };
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

                UriBuilder builder = new UriBuilder(_apiUrl);
                builder.Path += "/attributes";
                builder.Query = BuildQueryString(query);

                using (HttpClient client = GetClient(timeout))
                {
                    HttpResponseMessage response = await client.GetAsync(builder.ToString(), ct).ConfigureAwait(false);
                    _lastResponse = response;

                    string body = await response.Content.ReadAsStringAsync();
                    _lastResponseBody = body;

                    if (response.StatusCode != HttpStatusCode.OK)
                        return new Result<ImageProperties> { Code = ResultCode.failed };

                    JObject responseObject = JObject.Parse(body);
                    Imagery dataItem = _jsonConverter.Deserialize<Imagery>(body);

                    ImageProperties imageProperties = _jsonConverter.Deserialize<ImageProperties>(responseObject["attributes"].ToString());

                    return new Result<ImageProperties> { Value = imageProperties, Code = responseObject == null || response.StatusCode != HttpStatusCode.OK ? ResultCode.failed : ResultCode.ok };
                }
            }
            catch
            {
                return new Result<ImageProperties> { Code = ResultCode.failed };
            }
        }
    }
}
