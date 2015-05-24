using Detrav.TeraModLoader.TeraApi.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraModLoader.Core
{
    class ConfigManager : IConfigManager
    {
        string file = "config";
        string modName = "unknown";

        public ConfigManager(string modName)
        {
            this.modName = modName;
        }

        public void init(string playerName = null)
        {
            if(playerName == null)
            {
                file = Path.Combine(file, "config.json");
                return;
            }

            file = Path.Combine(file, playerName, String.Format("{0}.json", modName));
        }

        public void save(object config)
        {
            
        }

        public object load()
        {
            throw new NotImplementedException();
        }
    }
}
