using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
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

		public async Task<Result<ImageUpdateResult>> UploadFeatures(FeatureUpdateParameters parameters, CancellationToken ct, TimeSpan? timeout = null)
		{
			Result<ImageUpdateResult> result = null;
			try
			{
				this.LogTracer.TrackEvent("Client.UploadFeatures()", new Dictionary<string, string> {
					{ "features", parameters.features.Length.ToString() },
					{ "imageryId", string.Join(",",parameters.features.Select(x=>x.imageryId)) },
					{ "featureTypeId", string.Join(",",parameters.features.Select(x=>x.featureTypeId)) },
					{ "imageTypeId", string.Join(",",parameters.features.Select(x=>x.imageTypeId)) },
				});

				if (parameters.features.Any(x => x.imageryId == Guid.Empty) || parameters.features.Any(x => x.featureTypeId == Guid.Empty) || parameters.features.Any(x => x.imageTypeId == Guid.Empty))
					return new Result<ImageUpdateResult> { Code = ResultCode.failed };
				UriBuilder builder = new UriBuilder(_apiUrl);
				builder.Path += "/feature";
				result = await ClientPost(builder, parameters, timeout, ct, (response, body) =>
				{
					return (ImageUpdateResult)null;
				});
			}
			catch (Exception ex)
			{
				this.LogTracer.TrackError(ex);
				result = new Result<ImageUpdateResult> { Code = ResultCode.failed, Message = "Exception " + ex.Message + Environment.NewLine + ex.ToString() };
			}
			return result;
		}


	}
}
