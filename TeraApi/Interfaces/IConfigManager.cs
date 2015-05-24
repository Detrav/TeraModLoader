using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.Interfaces
{
    public interface IConfigManager
    {
        void init(string playerName);
        void save(object config);
        object load(Type t);
    }
}
