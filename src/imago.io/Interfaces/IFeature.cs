using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.IO.Interfaces
{
    public interface IFeature
    {
        Guid Id { get; set; }
        Guid ImageryId { get; set; }
        Guid FeatureTypeId { get; set; }
        Guid ImageTypeId { get; set; }
        Guid RefImageryId { get; set; }

        [Newtonsoft.Json.JsonConverter(typeof(Converters.PointsConverter))]
        List<IPoint> Points { get; set; }
    }
}
