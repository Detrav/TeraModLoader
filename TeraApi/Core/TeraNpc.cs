using Detrav.TeraApi.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.Core
{
    public class TeraNpc
    {
        public ulong id;
        public ulong parentId;
        public NpcDataBase npc;

        public TeraNpc(ulong id, NpcDataBase npc,ulong parentId = 0)
        {
            this.id = id;
            this.npc = npc;
            this.parentId = parentId;
        }
    }
}
