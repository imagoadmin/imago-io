using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.IO.Interfaces
{
    public interface IImageMetadata
    {
        DateTimeOffset? DateTaken { get; set; }
        double? Latitude { get; set; }
        double? Longitude { get; set; }
        string Model { get; set; }
        string ExposureProgram { get; set; }
        string ExposureTime { get; set; }
        string FNumber { get; set; }
        int? ISO { get; set; }
        string FocalLength { get; set; }
        string WhiteBalance { get; set; }
        string Lens { get; set; }
        string DOF { get; set; }
        string LightValue { get; set; }
    }
}
