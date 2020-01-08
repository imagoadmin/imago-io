using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.IO.Classes
{
    public class AttributeType
    {

        public enum ValueType { String, Note, Date, Number, Colour };

        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ValueType DataType { get; set; }
    }
}
