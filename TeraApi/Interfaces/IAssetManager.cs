using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.Interfaces
{
    public interface IAssetManager
    {
        object deSerialize(string path, Type t,AssetType assetType = AssetType.relative);
        void serialize(string path, object f, AssetType assetType = AssetType.relative);
        string[] getDirectories(string path, AssetType assetType = AssetType.relative);
        string[] getFiles(string path, string patern, AssetType assetType = AssetType.relative);
        string getMyFolder();
    }

    public enum AssetType { local, relative, global }
}