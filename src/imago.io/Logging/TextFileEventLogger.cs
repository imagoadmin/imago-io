using System;
using System.Collections.Generic;
using System.IO;

namespace Imago.IO
{
    public class TextFileEventLogger : IEventLogger
    {
        private StreamWriter writer;

        public TextFileEventLogger(string fileName)
        {
            writer = new StreamWriter(fileName, true);
        }

        public void TrackError(Exception ex, IDictionary<string, string> properties = null)
        {
            writer.WriteLine("exception|" + DateTime.Now.ToString() + "|" + ex.Message + "|" + PropertiesAsString(properties));
        }
        public void TrackError(string err, IDictionary<string, string> properties = null)
        {
            writer.WriteLine("error|" + DateTime.Now.ToString() + "|" + err + "|" + PropertiesAsString(properties));
        }

        public void TrackEvent(string name, IDictionary<string, string> properties = null)
        {
            writer.WriteLine("event|" + DateTime.Now.ToString() + "|" + name + "|" + PropertiesAsString(properties));
        }

        private string PropertiesAsString(IDictionary<string,string> properties)
        {
            string details = "";
            foreach (string key in properties.Keys)
                details += (details.Length == 0 ? "" : "|") + key + "=" + properties[key];
            return details;
        }
    }
}
