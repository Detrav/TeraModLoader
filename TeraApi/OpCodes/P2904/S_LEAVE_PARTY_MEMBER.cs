using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.OpCodes.P2904
{
    public class S_LEAVE_PARTY_MEMBER : TeraPacket
    {
        public string name;
        public S_LEAVE_PARTY_MEMBER(TeraPacketWithData packet) : base(packet)
        {
            name = packet.toDoubleString(14, 200);
        }
    }
}
