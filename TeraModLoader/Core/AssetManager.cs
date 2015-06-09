using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Detrav.TeraApi.Interfaces;
using Detrav.TeraApi;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Win32;
using System.IO.Compression;

namespace Detrav.TeraModLoader.Core
{
    class AssetManager : IAssetManager
    {

        static string assets = "assets";
        string modName = "TeraModLoader";
        string zipFile;

        public AssetManager(string modName = null,string zipFile = null)
        {
            if (modName != null)
                this.modName = modName;
            this.zipFile = zipFile;
            Logger.debug("new AssetManager for {0}", this.modName);
        }


        public void serialize(string path, object f, AssetType assetType = AssetType.relative)
        {
            string file = "";
            switch (assetType)
            {
                case AssetType.global:
                    file = path;
                    break;
                case AssetType.relative:
                    file = Path.Combine(assets, modName, path);
                    break;
            }
            if (!Directory.Exists(Path.GetDirectoryName(file))) Directory.CreateDirectory(Path.GetDirectoryName(file));
            using (TextWriter tw = new StreamWriter(file))
            {
                JsonSerializerSettings s = new JsonSerializerSettings();
                tw.Write(JsonConvert.SerializeObject(f));
            }
        }

        public object deSerialize(string path, Type t, AssetType assetType = AssetType.relative)
        {
            string file = "";
            switch(assetType)
            {
                case AssetType.local:
                    if (!File.Exists(zipFile)) return null;
                    ZipArchive zip = ZipFile.OpenRead(zipFile);
                    var e = zip.GetEntry(Path.Combine(assets,path));
                    using (TextReader tr = new StreamReader(e.Open()))
                    {
                        JsonSerializerSettings jss = new JsonSerializerSettings();
                        jss.NullValueHandling = NullValueHandling.Ignore;
                        return JsonConvert.DeserializeObject(tr.ReadToEnd(), t, jss);
                    }
                case AssetType.global:
                    file = path;
                    break;
                case AssetType.relative:
                    file = Path.Combine(assets, modName, path);
                    break;
            }
            if (File.Exists(file))
                using (TextReader tr = new StreamReader(file))
                {
                    Logger.debug("End deSerialize for {0}", file);
                    JsonSerializerSettings jss = new JsonSerializerSettings();
                    jss.NullValueHandling = NullValueHandling.Ignore;
                    return JsonConvert.DeserializeObject(tr.ReadToEnd(), t, jss);
                }
            return null;
        }


        public string[] getDirectories(string path, AssetType assetType = AssetType.relative)
        {

            string file = "";
            switch (assetType)
            {
                case AssetType.local:
                    return new string[0];
                case AssetType.relative:
                    if (path != null)
                        file = Path.Combine(assets, modName, path);
                    else
                        file = Path.Combine(assets, modName);
                    break;
                case AssetType.global:
                    file = path;
                    break;
            }
            return Directory.GetDirectories(file);
        }

        public string[] getFiles(string path, string patern, AssetType assetType = AssetType.relative)
        {
            string file = "";
            switch (assetType)
            {
                case AssetType.local:
                    return new string[0];
                case AssetType.relative:
                    if (path != null)
                        file = Path.Combine(assets, modName, path);
                    else
                        file = Path.Combine(assets, modName);
                    break;
                case AssetType.global:
                    file = path;
                    break;
            }
            return Directory.GetFiles(file, patern);
        }

        public string getMyFolder()
        {
            Logger.debug("getMyFolder {0}", Path.GetFullPath(Path.Combine(assets, modName)));
            return Path.GetFullPath(Path.Combine(assets, modName));
        }
    }
}
