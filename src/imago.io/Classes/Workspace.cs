﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imago.IO.Classes
{
    public class Workspace
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; }
        public Boolean Readonly { get; set; }
        public List<Dataset> Datasets { get; set; } = new List<Dataset>();
    }
}