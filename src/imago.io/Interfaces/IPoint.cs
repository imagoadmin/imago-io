using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.IO.Interfaces
{
    public interface IPoint
    {
        double X { get; set; }
        double Y { get; set; }
        int Pen { get; set; }
    }
}
