using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraModLoader.Core.Data
{
    internal class MyConfig
    {
        public SortedList<string, bool> modEnable = new SortedList<string,bool>();
        public List<ServerInfo> servers = new List<ServerInfo>();
        public int deviceIndex = 0;
        public int serverIndex = 0;
    }

    internal class ServerInfo
    {
        public string name;
        public string ip;
        public ServerInfo() : this(null, null) { }
        public ServerInfo(string name,string ip)
        {
            this.name = name;
            this.ip = ip;
        }
    }
}
