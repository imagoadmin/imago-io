using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.IO.Interfaces
{
    public interface IWorkspace
    {
        Guid Id { get; set; }
        string Name { get; set; }
        Boolean Readonly { get; set; }

        [Newtonsoft.Json.JsonConverter(typeof(Converters.DatasetsConverter))]
        List<IDataset> Datasets { get; set; }
    }
}
