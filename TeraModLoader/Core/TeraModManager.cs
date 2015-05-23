using Detrav.TeraModLoader.TeraApi.Interfaces;
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
        Assembly[] assemblyes;
        Type[] mods;
        static string directory = "mods";
        public TeraModManager()
        {
            if (Directory.Exists(directory)) Directory.CreateDirectory(directory);
            List<Assembly> assms = new List<Assembly>();
            List<Type> ts = new List<Type>();
            foreach(var file in Directory.GetFiles(directory, "*.dll"))
            {
                try
                {
                    Assembly a = Assembly.LoadFrom(file);
                    foreach (var v in a.GetTypes())
                    {
                        if (v.GetInterfaces().Contains(typeof(ITeraMod)))
                        {
                            assms.Add(a);
                            ts.Add(v);
                            break;
                        }
                    }
                }
                catch { }
            }
            assemblyes = assms.ToArray();
            mods = ts.ToArray();
        }
    }
}
