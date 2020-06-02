using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.IO.Interfaces
{
    public interface IAttribute
    {
        Guid Id { get; set; }
        Guid ImageryId { get; set; }
        Guid AttributeTypeId { get; set; }
        Guid? ImageTypeId { get; set; }
        string Value { get; set; }
    }
}
