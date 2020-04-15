using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.IO.Interfaces
{
    public interface ICollection
    {
        Guid Id { get; set; }
        Guid DatasetId { get; set; }
        string Name { get; set; }
        DateTime? CreatedOn { get; set; }
    }
}
