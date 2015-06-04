using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.OpCodes.P2904
{
    //Пакет от сервера, когда рядом пропадает NPC
    public class S_DESPAWN_NPC : TeraPacket
    {
        public ulong id { get; set; }
        public S_DESPAWN_NPC(TeraPacketWithData packet) : base(packet)
        {
            id = packet.toUInt64(4);
        }
    }
}
