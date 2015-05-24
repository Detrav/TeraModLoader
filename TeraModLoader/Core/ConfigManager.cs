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
        string folder = "config";
        string configFile;
        string modName;

        public ConfigManager(string modName = "unknown")
        {
            this.modName = modName;
        }

        public void init(string playerName = null)
        {
            if (playerName == null)
            {
                configFile = Path.Combine(folder, "config.json");
                Logger.debug("Init configManager for {0}", configFile);
                return;
            }

            configFile = Path.Combine(folder, playerName, String.Format("{0}.json", modName));
        }

        public void save(object config)
        {
            Logger.debug("Started save for {0}", configFile);
            if (!Directory.Exists(Path.GetDirectoryName(configFile))) Directory.CreateDirectory(Path.GetDirectoryName(configFile));
            using(TextWriter tw = new StreamWriter(configFile))
            {
                JsonSerializerSettings s = new JsonSerializerSettings();
                tw.Write(JsonConvert.SerializeObject(config));
            }
            Logger.debug("End save for {0}", configFile);
        }

        public object load(Type t)
        {
            Logger.debug("Started load for {0}", configFile);
            if (File.Exists(configFile))
                using (TextReader tr = new StreamReader(configFile))
                {
                    Logger.debug("End load for {0}", configFile);
                    return JsonConvert.DeserializeObject(tr.ReadToEnd(), t);
                }
            Logger.debug("Error on load for {0}", configFile);
            return null;
        }
    }
}
