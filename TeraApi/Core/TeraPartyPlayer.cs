using Detrav.TeraApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.Core
{
    public class TeraPartyPlayer : TeraPlayer
    {
        public uint hp { get; set; }
        public uint maxHp { get; set; }
        public uint mp { get; set; }
        public uint maxMp { get; set; }
        public uint stamina { get; set; }
        public uint maxStamina { get; set; }
        public ulong partyId { get; set; }

        public TeraPartyPlayer(ulong id,ulong partyId, string name, ushort level = 1, PlayerClass playerClass = PlayerClass.Empty)
            : base(id, name,level,playerClass)
        {
            this.partyId = partyId;
        }
    }
}
