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
        public static string ImagoApiVersion1 = "/io/1";
        public static string ImagoApiVersion2 = "/integrate/2";

        public static string DefaultHostName { get; set; } = "http://imagomio:3000";
        public static string DefaultApiVersion { get; set; } = Credentials.ImagoApiVersion1;
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
            ApiVersion = Credentials.DefaultApiVersion;
        }
        public string HostName { get; set; }
        public string ApiVersion { get; set; }
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
