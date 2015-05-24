using Detrav.TeraApi;
using Detrav.TeraApi.Interfaces;
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
                        bool containts = false;
                        foreach(var mod in ts)
                        {
                            if(mod.guid == m.guid)
                            {
                                containts = true;
                                break;
                            }
                        }
                        if (!containts)
                        {
                            Logger.log("Loaded {0}", m);
                            ts.Add(m);
                        }
                    }
                }
                catch {  }
            }
            mods = ts.ToArray();
        }
        public void loadConfig()
        {
            config.init();
            SortedList<string, bool> modsConfig = new SortedList<string, bool>();
            modsConfig = config.load(modsConfig.GetType()) as SortedList<string,bool>;
            foreach (var mod in mods)
            {
                bool enable;
                if (modsConfig != null)
                    if (modsConfig.TryGetValue(mod.guid.ToString(), out enable))
                    {
                        mod.enable = enable;
                        continue;
                    }
                mod.enable = true;
            }
        }
        public void saveConfig()
        {
            SortedList<string, bool> modsConfig = new SortedList<string, bool>();
            foreach (var mod in mods)
            {
                modsConfig.Add(mod.guid.ToString(),mod.enable);
            }
            config.save(modsConfig);
        }

        internal ITeraMod[] initializeMods()
        {
            List<ITeraMod> teraMods = new List<ITeraMod>();
            foreach(var mod in mods)
            {
                if (mod.enable)
                    teraMods.Add(mod.create());
            }
            return teraMods.ToArray();
        }
    }
}
