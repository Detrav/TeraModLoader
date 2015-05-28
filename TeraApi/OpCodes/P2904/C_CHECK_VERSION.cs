using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.OpCodes.P2904
{
    public class C_CHECK_VERSION : TeraPacket
    {
        public uint count { get; set; }
        public TrippleInt[] ints;
        public C_CHECK_VERSION(TeraPacketWithData packet)
            : base(packet)
        {
            int count = packet.toUInt16(4);
            ints = new TrippleInt[count];
            for (int i = 0; i < count; i++)
            {
                ints[i].val1 = packet.toInt32(6 + 12 * i);
                ints[i].val2 = packet.toInt32(10 + 12 * i);
                ints[i].val3 = packet.toInt32(14 + 12 * i);
            }
        }

        public class TrippleInt
        {
            public int val1;
            public int val2;
            public int val3;
        }
    }
}
