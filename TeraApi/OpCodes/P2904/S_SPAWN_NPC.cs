using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.OpCodes.P2904
{
    public class S_SPAWN_NPC : TeraPacket
    {
        public ulong id { get; set; }
        public ulong parentId { get; set; }
        public S_SPAWN_NPC(TeraPacketWithData packet)
            : base(packet)
        {
            id = packet.toUInt64(8);
            parentId = packet.toUInt64(85);
        }
    }
}
