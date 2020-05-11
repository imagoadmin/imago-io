using Imago.IO.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.IO.Classes
{
    public class Attribute : IAttribute
    {
        public Guid Id { get; set; }
        public Guid ImageryId { get; set; }
        public Guid AttributeTypeId { get; set; }
        public string AttributeTypeName { get; set; }
        public Guid? ImageTypeId { get; set; } = null;
        public string Value { get; set; }
    }
}
