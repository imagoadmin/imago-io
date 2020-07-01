using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.IO.Interfaces
{
    public interface IImageType
    {
        Guid Id { get; set; }
        string Name { get; set; }
    }
}
