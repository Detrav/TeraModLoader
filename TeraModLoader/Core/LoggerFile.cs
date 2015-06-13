using Detrav.TeraApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Detrav.TeraModLoader.Core
{
    class LoggerFile : ILoggerFile
    {
        private string file;

        public LoggerFile(string file)
        {
            // TODO: Complete member initialization
            this.file = file;
        }

        public void log(string text)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
