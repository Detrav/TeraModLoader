using Detrav.TeraModLoader.TeraApi;
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
        Data.Mod[] mods;
        static string directory = "mods";
        public TeraModManager()
        {
            if (Directory.Exists(directory)) Directory.CreateDirectory(directory);
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
    }
}
