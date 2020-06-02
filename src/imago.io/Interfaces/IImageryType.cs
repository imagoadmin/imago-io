using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.IO.Interfaces
{
    public interface IImageryType
    {
        Guid Id { get; set; }
        string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        Classes.ImageryType.GeometryTypes GeometryType { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        Classes.ImageryType.ContentTypes ContentType { get; set; }
        [Newtonsoft.Json.JsonConverter(typeof(Converters.ImageTypesConverter))]
        List<IImageType> ImageTypes { get; set; }
    }
}
