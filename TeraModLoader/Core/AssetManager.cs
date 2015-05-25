using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Detrav.TeraApi.Interfaces;
using Detrav.TeraApi;
using System.IO;
using Newtonsoft.Json;

namespace Detrav.TeraModLoader.Core
{
    class AssetManager : IAssetManager
    {

        static string assets = "assets";
        string modName = "TeraModLoader";

        public AssetManager(string modName = null)
        {
            if (modName != null)
                this.modName = modName;
        }


        public void serialize(string relativePath, object f)
        {
            string file = Path.Combine(assets, modName, relativePath);
            Logger.debug("Started deSerialize for {0}", file);
            if (!Directory.Exists(Path.GetDirectoryName(file))) Directory.CreateDirectory(Path.GetDirectoryName(file));
            using (TextWriter tw = new StreamWriter(file))
            {
                JsonSerializerSettings s = new JsonSerializerSettings();
                tw.Write(JsonConvert.SerializeObject(f));
            }
            Logger.debug("End deSerialize for {0}", file);
        }

        public object deSerialize(string relativePath,Type t)
        {
            string file = Path.Combine(assets, modName, relativePath);
            Logger.debug("Started deSerialize for {0}", file);
            if (File.Exists(file))
                using (TextReader tr = new StreamReader(file))
                {
                    Logger.debug("End deSerialize for {0}", file);
                    return JsonConvert.DeserializeObject(tr.ReadToEnd(), t);
                }
            Logger.debug("Error on deSerialize for {0}", file);
            return null;
        }


        public string[] getDirectories(string relativePath)
        {
            string file;
            if (relativePath != null)
                file = Path.Combine(assets, modName, relativePath);
            else
                file = Path.Combine(assets, modName, relativePath);
            return Directory.GetDirectories(file);
        }

        public string[] getFiles(string relativePath,string patern)
        {
            string file;
            if (relativePath != null)
                file = Path.Combine(assets, modName, relativePath);
            else
                file = Path.Combine(assets, modName, relativePath);
            return Directory.GetFiles(file, patern);
        }
    }
}
