using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.IO.Interfaces
{
    public interface IImagery
    {
        Guid Id { get; set; } 
        Guid ImageryTypeId { get; set; } 
        Guid CollectionId { get; set; } 
        string Name { get; set; }
        double? X { get; set; }
        double? Y { get; set; }
        double? Z { get; set; }

        double? StartDepth { get; set; }
        double? EndDepth { get; set; }

        [Newtonsoft.Json.JsonConverter(typeof(Converters.ImagesConverter))]
        List<Interfaces.IImage> Images { get; set; }
        [Newtonsoft.Json.JsonConverter(typeof(Converters.FeaturesConverter))]
        List<Interfaces.IFeature> Features { get; set; }
        [Newtonsoft.Json.JsonConverter(typeof(Converters.AttributesConverter))]
        List<Interfaces.IAttribute> AttributeDefinitions { get; set; }
    }
}
