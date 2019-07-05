using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imago.IO.Classes
{
    public class Collection
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid DatasetId { get; set; } = Guid.Empty;
        public string Name { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
