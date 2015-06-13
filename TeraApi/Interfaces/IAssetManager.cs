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
        /// <summary>
        /// Разбирает json файл
        /// </summary>
        /// <param name="path">файл</param>
        /// <param name="t">Тип</param>
        /// <param name="assetType">Типы ссылки</param>
        /// <returns></returns>
        object deSerialize(string path, Type t,AssetType assetType = AssetType.relative);
        /// <summary>
        /// Сериализует объект в файл
        /// </summary>
        /// <param name="path">Ссылка на файл</param>
        /// <param name="f">объект для сериализации</param>
        /// <param name="assetType">Тип ссылки</param>
        void serialize(string path, object f, AssetType assetType = AssetType.relative);
        TOut deSerialize<TOut>(string json);
        string serialize(object f);

        string[] getDirectories(string path, AssetType assetType = AssetType.relative);
        string[] getFiles(string path, string patern, AssetType assetType = AssetType.relative);
        
        string getMyFolder();
        void openFile(string path, OnOpenFile s, AssetType assetType = AssetType.relative);
        void saveFile(string path, OnSaveFile s, AssetType assetType = AssetType.relative);
        ILoggerFile createLoggerFile(string path, AssetType aseetType = AssetType.relative);
    }

    public delegate void OnOpenFile(StreamReader s);
    public delegate void OnSaveFile(StreamWriter s);
    public enum AssetType { local, relative, global }
}