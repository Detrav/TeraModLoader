using Detrav.TeraApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraModLoader.Core
{
    class TeraModManager
    {
        public Data.Mod[] mods { get; private set; }
        ConfigManager config = new ConfigManager();
        static string directory = "mods";
        public TeraModManager()
        {
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            List<Data.Mod> ts = new List<Data.Mod>();
            foreach (var file in Directory.GetFiles(directory, "*.dll"))
            {
                Assembly a;
                try
                {
                    a = Assembly.LoadFrom(file);
                    Data.Mod m = new Data.Mod(a);
                    if (m.ready)
                    {
                        Logger.log("Loaded {0}", m);
                        ts.Add(m);
                    }
                }
                catch {  }
            }
            mods = ts.ToArray();
        }

        public void loadConfig()
        {
            SortedList<string, bool> modsConfig = new SortedList<string, bool>();
            modsConfig = config.load(modsConfig.GetType()) as SortedList<string,bool>;
            foreach (var mod in mods)
            {
                bool enable;
                if (modsConfig.TryGetValue(mod.name, out enable))
                    mod.enable = enable;
                else
                    mod.enable = true;
            }
        }
        public void saveConfig()
        {
            SortedList<string, bool> modsConfig = new SortedList<string, bool>();
            foreach (var mod in mods)
            {
                modsConfig.Add(mod.name,mod.enable);
            }
            config.save(modsConfig);
        }
    }
}
