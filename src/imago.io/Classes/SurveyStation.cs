using Imago.IO.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.IO.Classes
{
    public class SurveyStation : ISurveyStation
    {
        public Guid Id { get; set; } = Guid.Empty;

        public string WorkspaceName { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}
