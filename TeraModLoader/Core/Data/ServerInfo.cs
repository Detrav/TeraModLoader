using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraModLoader.Core.Data
{
    internal class ServerInfo
    {
        public string name;
        public string ip;
        public ServerInfo() : this(null, null) { }
        public ServerInfo(string name, string ip)
        {
            this.name = name;
            this.ip = ip;
        }
    }
}
