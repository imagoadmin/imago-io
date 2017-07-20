using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imago.IO.Classes
{
    public class Dataset
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; }
        public List<DataSeriesType> DataSeriesTypes { get; set; } = new List<DataSeriesType>();
    }
}
