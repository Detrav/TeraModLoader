using Detrav.TeraApi.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.Core
{
    public class TeraNpc : TeraEntity
    {
        public NpcDataBase npc;
        public TeraEntity target;
        public TeraNpc(ulong id, NpcDataBase npc, TeraEntity parent = null)
            : base(id, parent)
        {
            this.npc = npc;
            this.name = npc.saveName;
        }
    }
}
