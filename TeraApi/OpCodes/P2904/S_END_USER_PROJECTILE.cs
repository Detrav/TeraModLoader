using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.OpCodes.P2904
{
    //Пакет от сервера, когда пропадает рядом ловушка или что то в этом роде
    public class S_END_USER_PROJECTILE : TeraPacket
    {
        public ulong id { get; set; }
        public S_END_USER_PROJECTILE(TeraPacketWithData packet)
            : base(packet)
        {
            id = packet.toUInt64(4);
        }
    }
}
