using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.Interfaces
{
    public interface IAssetManager
    {
        object deSerialize(string relativePath, Type t);
        void serialize(string relativePath, object f);
        string[] getDirectories(string relativePath);
        string[] getFiles(string relativePath, string patern);
        object getOpenFileDialog();
        object getSaveFileDialog();
    }
}