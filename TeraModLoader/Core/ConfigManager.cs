using Detrav.TeraApi.Interfaces;
using Newtonsoft.Json;
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
            using(TextWriter tw = new StreamWriter(file))
            {
                tw.Write(JsonConvert.SerializeObject(config));
            }

        }

        public object load(Type t)
        {
           using(TextReader tr = new StreamReader(file))
           {
               return JsonConvert.DeserializeObject(tr.ReadToEnd(), t);
           }
        }
    }
}
