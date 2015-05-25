using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.Interfaces
{
    public interface IConfigManager
    {
        void savePlayer(string playerName, object config);
        object loadPlayer(string playerName, Type t);
        void saveGlobal(object config);
        object loadGlobal(Type t);
    }
}
