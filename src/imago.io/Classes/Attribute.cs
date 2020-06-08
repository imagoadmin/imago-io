using Imago.IO.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.IO.Classes
{
    public class Attribute : IAttribute
    {
        public string name { get; set; }
        public IDictionary<string, object> attributes { get; set; }
    }
}
