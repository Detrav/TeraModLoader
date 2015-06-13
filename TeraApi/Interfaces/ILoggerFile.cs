using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.Interfaces
{
    public interface ILoggerFile : IDisposable
    {
        /// <summary>
        /// Записываем в лог текст, каждая запись в новой строке
        /// </summary>
        /// <param name="text">Текст для записи</param>
        void log(string text);
    }
}
