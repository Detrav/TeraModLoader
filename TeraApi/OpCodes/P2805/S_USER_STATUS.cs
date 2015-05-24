using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.OpCodes.P2805
{
    public class S_USER_STATUS : TeraPacket
    {
        public S_USER_STATUS(TeraPacketWithData packet)
            : base(packet)
        {
            id = packet.toUInt64(4);//     readUInt64(4, "id");
            status = packet.toByte(12);//     readByte(12, "status");
            catch { }
        }

        public ulong id { get; set; }

        public byte status { get; set; }
    }
}
