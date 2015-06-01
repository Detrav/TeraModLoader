using Detrav.TeraApi;
using Detrav.TeraApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Detrav.TeraModLoader.Core.Data
{
    internal class Mod
    {
        private Assembly assembly;
        public string name { get; private set; }
        public Version version { get; private set; }
        public BitmapImage icon { get; private set; }
        //public Guid guid { get; private set; }
        public bool enable { get; set; }
        private Type type;

        public Mod(Assembly assembly)
        {
            Logger.debug("Started mod creator");
            ready = false;
            this.assembly = assembly;
            AssemblyTitleAttribute assemblyTitle = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0] as AssemblyTitleAttribute;
            name = assemblyTitle.Title;
            version = assembly.GetName().Version;
            //guid = assembly.GetType().GUID;
            foreach (var v in assembly.GetTypes())
            {
                if (v.GetInterfaces().Contains(typeof(ITeraMod)))
                {
                    if (v.GetMethod("getModIcon") != null)
                    {
                        icon = ToImage((byte[])v.GetMethod("getModIcon").Invoke(null, null));
                        Logger.debug("ModHasIcon");
                    }
                    type = v;
                    ready = true;
                    break;
                }
            }
            Logger.debug("End mod creator, detected {0}",name);
        }

        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        public bool ready { get; private set; }

        public ITeraMod create()
        {
            Logger.debug("Construct mod {0}", name);
            ITeraMod mod = (ITeraMod)Activator.CreateInstance(type);
            mod.init(new ConfigManager(name),new AssetManager(name));
            return mod;
        }

        public override string ToString()
        {
            return String.Format("Mod {0} - Version {1}", name, version);
        }
    }
}