using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.OpCodes.P2904
{
    //Получаем каждый раз, как используется скилл, наноситься урон или блокируется
    public class S_EACH_SKILL_RESULT : TeraPacket
    {
        public ulong idWho { get; set; }
        public ulong idTarget { get; set; }
        public uint idSkill { get; set; }
        public uint damage { get; set; }
        /// <summary>
        /// 0 - block
        /// 1 - attack
        /// 2 - heal
        /// </summary>
        public ushort dType { get; set; }

        public S_EACH_SKILL_RESULT(TeraPacketWithData packet)
            : base(packet)
        {
            try
            {
                //readUInt32(4, "visualeffect+count");//4 
                idWho = packet.toUInt64(8);
                //readUInt64(8, "attacker id");//8
                idTarget = packet.toUInt64(16);
                //readUInt64(16, "target id");//16
                //readUInt32(24, "creature model id");//24
                idSkill = packet.toUInt32(28);
                //readUInt32(28, "skill id");//28
                //readUInt32("shift 1");//32
                //readUInt64(36, "attak id");//36
                //readUInt32("shift 2");//44
                damage = packet.toUInt32(48);
                //readUInt32(48, "damage");//48
                dType = packet.toUInt16(52);
                //readUInt16(52, "type");//52
                //readByte(54, "crit");//54
                //readByte(55, "електровсплеск");//55
                //readByte(56, "overturned 1");//56 //Походу крит
                //readByte(57, "overturned 2");//57
            }
            catch { }
        }
    }
}
