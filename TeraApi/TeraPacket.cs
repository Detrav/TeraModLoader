using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi
{
    public class TeraPacket
    {
        public ushort size { get; set; }
        public ushort opCode { get; set; }


        public TeraPacket(TeraPacketWithData p)
        {
            size = p.toUInt16(0);
            opCode = p.toUInt16(2);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
