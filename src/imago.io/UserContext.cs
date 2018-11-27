using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imago.IO.Classes;

namespace Imago.IO
{
    /// <summary>
    /// Each imago user has access to a list of projects with their corresponding datasets and definitions.
    /// The context describes what data they may potentially read or modify (depending on their permissions).
    /// It also contains identifiers that are used to 
    /// 
    /// </summary>
    public class UserContext
    {
        public List<Workspace> Workspaces { get; set; } = new List<Workspace>();
    }
}
