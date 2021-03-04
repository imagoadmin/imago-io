using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.IO.Interfaces
{
    public interface ISurveyStation
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Category { get; set; }
        double X { get; set; }
        double Y { get; set; }
        double Z { get; set; }
    }
}
