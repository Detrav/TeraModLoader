using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraModLoader.Core.Data
{
    public class ModInfo
    {
        public string Title;
        public string Version;
        public string RequiredVersion;
        public string Icon;
        public string Description;
        public string Author;
        public string URL;
        public string Company;
        public string Mod;

        internal bool inVersion(System.Version version)
        {
            throw new NotImplementedException();
        }
    }
}