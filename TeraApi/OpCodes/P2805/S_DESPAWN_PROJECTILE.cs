using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.OpCodes.P2805
{
    public class S_DESPAWN_PROJECTILE : TeraPacket
    {
        public ulong id { get; set; }
        public S_DESPAWN_PROJECTILE(TeraPacketWithData packet)
            : base(packet)
        {
            id = packet.toUInt64(4);
        }
    }
}
