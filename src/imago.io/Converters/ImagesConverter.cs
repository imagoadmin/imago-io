﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imago.IO.Converters
{
    public class ImagesConverter : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanRead => base.CanRead;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanWrite is false. The type will skip the converter.");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var results = serializer.Deserialize<IEnumerable<Classes.Imagery.Image>>(reader);
            return new List<Interfaces.IImage>(results);
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
