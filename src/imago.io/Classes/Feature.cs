using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imago.IO.Classes
{
    public class Feature: Interfaces.IFeature
    {
        public class Point : Interfaces.IPoint
        {
            public double X { get; set; }
            public double Y { get; set; }
            public int Pen { get; set; }

            public Interfaces.IPoint Clone()
            {
                return new Point() { X = this.X, Y = this.Y, Pen = this.Pen };
            }
        }
        public Guid Id { get; set; } = Guid.Empty;
        public Guid ImageryId { get; set; } = Guid.Empty;
        public Guid FeatureTypeId { get; set; } = Guid.Empty;
        public Guid ImageTypeId { get; set; } = Guid.Empty;
        public Guid RefImageryId { get; set; } = Guid.Empty;
        public List<Interfaces.IPoint> Points { get; set; } = new List<Interfaces.IPoint>();
    }
}
