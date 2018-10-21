using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.IO.Classes
{
    public class ImageMetadata
    {
        public DateTimeOffset? DateTaken { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Model { get; set; }
        public string ExposureProgram { get; set; }
        public string ExposureTime { get; set; }
        public string FNumber { get; set; }
        public int? ISO { get; set; }
        public string FocalLength { get; set; }
        public string WhiteBalance { get; set; }
        public string Lens { get; set; }
        public string DOF { get; set; }
        public string LightValue { get; set; }
    }
}
