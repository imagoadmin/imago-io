using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imago.IO.Converters
{
    public class ImageryTypesConverter : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanRead => base.CanRead;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanWrite is false. The type will skip the converter.");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);
            var results = array.ToObject<IEnumerable<Classes.ImageryType>>();
            //var results = serializer.Deserialize<IEnumerable<Classes.ImageryType>>(reader);
            return new List<Interfaces.IImageryType>(results);
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
