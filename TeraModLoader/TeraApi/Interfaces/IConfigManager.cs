using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraModLoader.TeraApi.Interfaces
{
    interface IConfigManager
    {
        void init(string playerName);
        void save(object config);
        object load();
    }
}
