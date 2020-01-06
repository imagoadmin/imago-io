using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imago.IO.Classes
{
    public class Feature
    {
        public class Point
        {
            public double X { get; set; }
            public double Y { get; set; }
            public int Pen { get; set; }
        }
        public Guid Id { get; set; } = Guid.Empty;
        public Guid ImageryId { get; set; } = Guid.Empty;
        public Guid FeatureTypeId { get; set; } = Guid.Empty;
        public Guid ImageTypeId { get; set; } = Guid.Empty;
        public List<Point> Points { get; set; } = new List<Point>();
    }
}
