using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.OpCodes.P2805
{
    public class S_SPAWN_PROJECTILE :TeraPacket
    {
        public S_SPAWN_PROJECTILE(TeraPacketWithData packet)
            : base(packet)
        {
            id = packet.toUInt64(4);// readUInt64(4, "id");
            idPlayer = packet.toUInt64(49);// readUInt64(49, "player id");
        }

        public ulong id { get; set; }

        public ulong idPlayer { get; set; }
    }
}
