using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.IO.Interfaces
{
    public interface IAttributeDefinition
    {
        string name { get; set; }
        IDictionary<string,object> attributes { get; set; }
    }
}
