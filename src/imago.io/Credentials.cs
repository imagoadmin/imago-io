using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imago.IO.Classes;

namespace Imago.IO
{
    public class Credentials
    {
        public static string DefaultHostName { get; set; } = "http://imagomio:3000";
        public static string DefaultApiVersion1 { get; protected set; } = "/integrate/1";
        public Credentials()
        {
#if DEBUG
                        HostName = "http://localhost:3000";
            //HostName = "https://stagingapi.imago.live";
            // HostName = Credentials.DefaultHostName;
            //HostName = "https://portalapi.imago.live";
#else
            HostName = "https://io.imago.live";
#endif
            ApiVersion = Credentials.DefaultApiVersion1;
        }
        public string HostName { get; set; }
        public string ApiVersion { get; protected set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool IsDefined
        {
            get
            {
                return !String.IsNullOrWhiteSpace(UserName) && !String.IsNullOrWhiteSpace(Password) && !String.IsNullOrWhiteSpace(HostName);
            }
        }
    }
}
