using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.OpCodes.P2904
{
   //Получаем когда рядом появляется npc
    public class S_SPAWN_NPC : TeraPacket
    {
        public ulong id { get; set; }
        public ulong parentId { get; set; }
        public uint template { get; set; }
        public ushort header { get; set; }
        public S_SPAWN_NPC(TeraPacketWithData packet)
            : base(packet)
        {
            id = packet.toUInt64(10);
            parentId = packet.toUInt64(85);
            template = packet.toUInt32(44);
            header = packet.toUInt16(48);
        }
    }
}
