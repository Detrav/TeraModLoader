using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.OpCodes.P2904
{
    public class S_NPC_STATUS : TeraPacket
    {
        /*
addElement("size",0,"ushort");
addElement("opCode",2,"ushort");
addElement("npcId",4,"ulong");
addElement("unk1",12,"byte");
addElement("unk2",13,"uint");
addElement("plId",17,"ulong");
addElement("npc_status",25,"uint");
*/
        public ulong npcId { get; set; }
        public ulong playerId { get; set; }
        public S_NPC_STATUS(TeraPacketWithData packet) : base (packet)
        {
            npcId = packet.toUInt64(4);
            playerId = packet.toUInt64(17);
        }
    }
}
