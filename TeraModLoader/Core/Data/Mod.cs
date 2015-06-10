using Detrav.TeraApi;
using Detrav.TeraApi.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
        public ModInfo modInfo { get; private set; }
        public string name { get { return modInfo.Title; } }
        public string version { get { return modInfo.Version; } }
        public BitmapImage icon { get; private set; }
        //public Guid guid { get; private set; }
        public bool enable { get; set; }
        private Type type;
        public string fullName { get { return String.Format("{0}.{1}", name, version); } }
        private string zipFile;

        public Mod(string file)
        {
            zipFile = file;
            ready = false;
            AssetManager asset = new AssetManager(null, zipFile);
            var obj = asset.deSerialize("mod.info", typeof(ModInfo), AssetType.local);
            if (obj == null) return;
            modInfo = obj as ModInfo;
            if (modInfo.Title == null) return;
            if (modInfo.Mod == null) return;
            if (modInfo.Version == null) modInfo.Version = "unk";
            if (!modInfo.inVersion(Assembly.GetExecutingAssembly().GetName().Version)) return;
            if (!modInfo.inOpCodeVersions(Detrav.TeraApi.OpCodes.PacketCreator.getCurrentVersion())) ;
            using (var zip = ZipFile.OpenRead(zipFile))
            {
                using(BinaryReader stream = new BinaryReader(zip.GetEntry(modInfo.Mod).Open()))
                {
                    try
                    {
                        assembly = Assembly.Load(stream.ReadBytes((int)stream.BaseStream.Length));
                        foreach(var t in assembly.GetTypes())
                        {
                            if(t.GetInterfaces().Contains(typeof(ITeraMod)))
                            {
                                type = t;
                                break;
                            }
                        }
                        if (type == null) return;
                    }
                    catch { return; }
                }
                if(modInfo.Icon!=null)
                {
                    using (var stream = zip.GetEntry(modInfo.Icon).Open())
                    {
                        icon = ToImage(stream);
                    }
                }
                ready = true;
            }
        }

        public BitmapImage ToImage(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream((int)stream.Length))
            {
                stream.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);
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
            Logger.debug("Construct mod {0}", fullName);
            ITeraMod mod = (ITeraMod)Activator.CreateInstance(type);
            mod.init(new ConfigManager(fullName), new AssetManager(fullName, zipFile));
            return mod;
        }

        public override string ToString()
        {
            return fullName;
        }
    }
}