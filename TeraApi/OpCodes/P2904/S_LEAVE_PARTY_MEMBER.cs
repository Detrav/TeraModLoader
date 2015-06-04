using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.OpCodes.P2904
{
    //Получаем пакет, когда кто то выходит из пати
    public class S_LEAVE_PARTY_MEMBER : TeraPacket
    {
        public string name;
        public ulong partyId;
        public S_LEAVE_PARTY_MEMBER(TeraPacketWithData packet) : base(packet)
        {
            partyId = packet.toUInt64(6);
            name = packet.toDoubleString(14, 200);
        }
    }
}
