using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Imago.IO
{
    public partial class Client
    {
        public class FeatureUpdateParameters
        {
            public Feature[] features { get; set; }

            public class Feature
            {
                public Guid? id { get; set; } = null;
                public Guid imageryId { get; set; } = Guid.Empty;
                public Guid featureTypeId { get; set; } = Guid.Empty;
                public Guid imageTypeId { get; set; } = Guid.Empty;
                public Point[] points { get; set; } = null;

                public class Point
                {
                    public double x { get; set; }
                    public double y { get; set; }
                    public int pen { get; set; }
                }
            }
        }

        public class FeatureUpdateResult
        {
            public List<Classes.Feature> features { get; set; }
        }
        public async Task<Result<FeatureUpdateResult>> UploadFeatures(FeatureUpdateParameters parameters, CancellationToken ct, TimeSpan? timeout = null)
        {
            Result<FeatureUpdateResult> result = null;
            try
            {
                Telemetry.TelemetryLogger.Instance?.LogEvent(Telemetry.TelemetryEvents.ClientUploadFeatures,
                     new Dictionary<string, string> {
                    { "features", parameters.features.Length.ToString() },
                    { "imageryId", string.Join(",",parameters.features.Select(x=>x.imageryId)) },
                    { "featureTypeId", string.Join(",",parameters.features.Select(x=>x.featureTypeId)) },
                    { "imageTypeId", string.Join(",",parameters.features.Select(x=>x.imageTypeId)) },
                });

                if (parameters.features.Any(x => x.imageryId == Guid.Empty) || parameters.features.Any(x => x.featureTypeId == Guid.Empty) || parameters.features.Any(x => x.imageTypeId == Guid.Empty))
                    return new Result<FeatureUpdateResult> { Code = ResultCode.failed };
                UriBuilder builder = new UriBuilder(_apiUrl);
                builder.Path += "/feature";
                return await ClientPost(builder, parameters, timeout, ct, (response, body) =>
                {
                    this.LogHttpResponse(response);
                    var updateResult = _jsonConverter.Deserialize<FeatureUpdateResult>(body);
                    return updateResult;
                });
            }
            catch (Exception ex)
            {
                Telemetry.TelemetryLogger.Instance?.LogException(ex, new Dictionary<string, string> {
                    { "features", parameters.features.Length.ToString() },
                    { "imageryId", string.Join(",",parameters.features.Select(x=>x.imageryId)) },
                    { "featureTypeId", string.Join(",",parameters.features.Select(x=>x.featureTypeId)) },
                    { "imageTypeId", string.Join(",",parameters.features.Select(x=>x.imageTypeId)) },
                });
                result = new Result<FeatureUpdateResult> { Code = ResultCode.failed, Message = "Exception " + ex.Message + Environment.NewLine + ex.ToString() };
            }
            return result;
        }


    }
}
