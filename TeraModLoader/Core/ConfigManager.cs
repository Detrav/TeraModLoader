using Detrav.TeraApi;
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
        bool Inited = false;
        string file = "config";
        string modName;

        public ConfigManager(string modName = "unknown")
        {
            this.modName = modName;
        }

        public void init(string playerName = null)
        {
            if (Inited) return;
            if (playerName == null)
            {
                file = Path.Combine(file, "config.json");
                Logger.debug("Init configManager for {0}", file);
                return;
            }

            file = Path.Combine(file, playerName, String.Format("{0}.json", modName));
            Inited = true;
        }

        public void save(object config)
        {
            Logger.debug("Started save for {0}", file);
            if (!Directory.Exists(Path.GetDirectoryName(file))) Directory.CreateDirectory(Path.GetDirectoryName(file));
            using(TextWriter tw = new StreamWriter(file))
            {
                JsonSerializerSettings s = new JsonSerializerSettings();
                tw.Write(JsonConvert.SerializeObject(config));
            }
            Logger.debug("End save for {0}", file);
        }

        public object load(Type t)
        {
            Logger.debug("Started load for {0}", file);
            if (File.Exists(file))
                using (TextReader tr = new StreamReader(file))
                {
                    Logger.debug("End load for {0}", file);
                    return JsonConvert.DeserializeObject(tr.ReadToEnd(), t);
                }
            Logger.debug("Error on load for {0}", file);
            return null;
        }
    }
}
