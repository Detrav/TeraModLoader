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
        string configFolder = "config";
        string configMod = "TeraModLoader";
        byte type = 0;

        public ConfigManager(string modName = null)
        {
            if (modName != null)
                configMod = modName;
        }

        private void save(string file,object config)
        {
            Logger.debug("Started save for {0}", file);
            if (!Directory.Exists(Path.GetDirectoryName(file))) Directory.CreateDirectory(Path.GetDirectoryName(file));
            using (TextWriter tw = new StreamWriter(file))
            {
                JsonSerializerSettings s = new JsonSerializerSettings();
                tw.Write(JsonConvert.SerializeObject(config));
            }
            Logger.debug("End save for {0}", file);
        }

        private object load(string file, Type t)
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

        public void savePlayer(string playerName, object config)
        {
            string file = Path.Combine(configFolder, playerName, configMod + ".json");
            save(file, config);
        }

        public object loadPlayer(string playerName, Type t)
        {
            string file = Path.Combine(configFolder, playerName, configMod + ".json");
            return load(file, t);
        }

        public void saveGlobal(object config)
        {
            string file = Path.Combine(configFolder, configMod + ".json");
            save(file, config);
        }

        public object loadGlobal(Type t)
        {
            string file = Path.Combine(configFolder, configMod + ".json");
            return load(file, t);
        }
    }
}
