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
            if (Version == null) return false;
            var ver = Version.Split('.');
            if(ver.Length>0)
            {
                switch(ver[0].Trim()[0])
                {
                    case '<':
                        { 
                        int val;
                        if (!int.TryParse(ver[0].Trim().Substring(1).Trim(), out val)) return false;
                        if (version.Major >= val) return false;
                        }
                        break;
                    case '>':
                        {
                            int val;
                            if (!int.TryParse(ver[0].Trim().Substring(1).Trim(), out val)) return false;
                            if (version.Major <= val) return false;
                        }
                        break;
                }
                if(ver.Length>1)
                {
                    switch (ver[1].Trim()[0])
                    {
                        case '<':
                            {
                                int val;
                                if (!int.TryParse(ver[1].Trim().Substring(1).Trim(), out val)) return false;
                                if (version.Minor >= val) return false;
                            }
                            break;
                        case '>':
                            {
                                int val;
                                if (!int.TryParse(ver[1].Trim().Substring(1).Trim(), out val)) return false;
                                if (version.Minor <= val) return false;
                            }
                            break;
                    }
                    if (ver.Length > 2)
                    {
                        switch (ver[2].Trim()[0])
                        {
                            case '<':
                                {
                                    int val;
                                    if (!int.TryParse(ver[2].Trim().Substring(1).Trim(), out val)) return false;
                                    if (version.Build >= val) return false;
                                }
                                break;
                            case '>':
                                {
                                    int val;
                                    if (!int.TryParse(ver[2].Trim().Substring(1).Trim(), out val)) return false;
                                    if (version.Build <= val) return false;
                                }
                                break;
                        }
                    }
                    if (ver.Length > 3)
                    {
                        switch (ver[3].Trim()[0])
                        {
                            case '<':
                                {
                                    int val;
                                    if (!int.TryParse(ver[3].Trim().Substring(1).Trim(), out val)) return false;
                                    if (version.Revision >= val) return false;
                                }
                                break;
                            case '>':
                                {
                                    int val;
                                    if (!int.TryParse(ver[3].Trim().Substring(1).Trim(), out val)) return false;
                                    if (version.Revision <= val) return false;
                                }
                                break;
                        }
                    }
                }
            }
            return true;
        }
    }
}