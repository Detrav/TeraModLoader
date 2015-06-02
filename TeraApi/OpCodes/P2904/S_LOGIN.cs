using Detrav.TeraApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.OpCodes.P2904
{
    public class S_LOGIN :TeraPacket
    {
        public ulong id { get; set; }
        public ushort level { get; set; }
        public string name { get; set; }
        public PlayerClass playerClass { get; set; }
        public S_LOGIN(TeraPacketWithData packet)
            : base(packet)
        {
            ushort start_name = packet.toUInt16(4);// readUInt16(4, "start name");//4
            ushort end_name = packet.toUInt16(6);// readUInt16(6, "end name");//6
                //readUInt16(8, "visual len");//8
                //readUInt32(10, "sex race class");//10
                //readUInt32(14, "model");//14
            ushort sexRaceClass = packet.toUInt16(14);
            sexRaceClass -= 10101;
            playerClass = (PlayerClass)(sexRaceClass % 100);
            id = packet.toUInt64(18);
                //readUInt64(18, "player id");//18
                //readUInt64(26, "unique id");//26
            level = packet.toUInt16(61);
                ////readUInt16(61, "level");//61
            name = packet.toDoubleString(start_name, end_name);
                //readString(start_name, "name", end_name);
           
        }
    }
}
