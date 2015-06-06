using Detrav.TeraApi.Core;
using Detrav.TeraApi.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.Interfaces
{
    public interface ICacheManager
    {
        NpcDataBase getNpc(ushort header, uint id);
    }
}
