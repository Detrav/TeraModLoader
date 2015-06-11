using Detrav.TeraApi.OpCodes;
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
        public OpCodeVersion[] RequiredOpCodeVersions;

        internal bool inVersion(System.Version version)
        {
            if (RequiredVersion == null) return false;
            var ver = RequiredVersion.Split('.');
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
                    case '*':
                        break;
                    default:
                        {
                            int val;
                            if(!int.TryParse(ver[0].Trim(),out val)) return false;
                            if(val!=version.Major) return false;
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
                        case '*':
                            break;
                        default:
                            {
                                int val;
                                if (!int.TryParse(ver[1].Trim(), out val)) return false;
                                if (val != version.Minor) return false;
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
                            case '*':
                                break;
                            default:
                                {
                                    int val;
                                    if (!int.TryParse(ver[2].Trim(), out val)) return false;
                                    if (val != version.Build) return false;
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
                            case '*':
                                break;
                            default:
                                {
                                    int val;
                                    if (!int.TryParse(ver[3].Trim(), out val)) return false;
                                    if (val != version.Revision) return false;
                                }
                                break;
                        }
                    }
                }
            }
            return true;
        }

        internal bool inOpCodeVersions(OpCodeVersion opCodes)
        {
            if(RequiredOpCodeVersions==null) return false;
            foreach(var el in RequiredOpCodeVersions)
            {
                if (el == OpCodeVersion.Any) return true;
                if (el == opCodes) return true;
            }
            return false;
        }
    }
}