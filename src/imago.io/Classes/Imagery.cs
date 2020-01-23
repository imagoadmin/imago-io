using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imago.IO.Classes
{
    public class Imagery
    {
        public class Image
        {
            public Guid ImageTypeId { get; set; }
            public string url { get; set; }
            public DateTime? uploadedOn { get; set; }
            public string uploadedBy { get; set; }
            public int? width { get; set; }
            public int? height { get; set; }
            public Int64? fileSize { get; set; }
        }

        public Guid Id { get; set; } = Guid.Empty;
        public Guid ImageryTypeId { get; set; } = Guid.Empty;
        public Guid CollectionId { get; set; } = Guid.Empty;
        public string Name { get; set; }
        public double? X { get; set; }
        public double? Y { get; set; }
        public double? Z { get; set; }

        public double? StartDepth { get; set; }
        public double? EndDepth { get; set; }

        public List<Image> Images { get; set; } = new List<Image>();
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (!String.IsNullOrWhiteSpace(Name))
                sb.Append(Name);
            if (StartDepth.HasValue)
                sb.Append((sb.Length > 0 ? "_" : "") + StartDepth.Value.ToString("F2"));
            if (EndDepth.HasValue)
                sb.Append((sb.Length > 0 ? "_" : "") + EndDepth.Value.ToString("F2"));
            if (X.HasValue)
                sb.Append((sb.Length > 0 ? "_" : "") + X.Value.ToString("F4"));
            if (Y.HasValue)
                sb.Append((sb.Length > 0 ? "_" : "") + Y.Value.ToString("F4"));
            if (Z.HasValue)
                sb.Append((sb.Length > 0 ? "_" : "") + Z.Value.ToString("F4"));

            return sb.ToString();
        }
    }
}
