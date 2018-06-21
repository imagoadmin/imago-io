using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.IO
{
    // TODO: This interface should be merged with Imago.Utils's interface 
    // but we couldn't do it easily because of assembly references, future nuget 
    // package requirements. It will take some planning to get an interface used 
    // everywhere in the right place
    public interface IEventLogger
    {
        void TrackError(Exception ex, IDictionary<string, string> properties = null);
        void TrackEvent(string name, IDictionary<string, string> properties = null);
    }
}
