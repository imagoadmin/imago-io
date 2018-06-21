using System;
using System.Collections.Generic;

namespace Imago.IO
{
    public class DoNothingEventLogger : IEventLogger
    {
        public void TrackError(Exception ex, IDictionary<string, string> properties = null)
        {
        }

        public void TrackEvent(string name, IDictionary<string, string> properties = null)
        {
        }
    }
}
