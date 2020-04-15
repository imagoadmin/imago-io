using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.IO.Interfaces
{
    public interface IDataset
    {
        Guid Id { get; set; }
        string Name { get; set; }
        [Newtonsoft.Json.JsonConverter(typeof(Converters.ImageryTypesConverter))]
        List<IImageryType> ImageryTypes { get; set; }
    }
}
