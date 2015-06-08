using Detrav.TeraApi.Enums;
namespace Detrav.TeraApi.Core
{
    public class TeraPlayer : TeraEntity
    {
        public ushort level { get; set; }
        public PlayerClass playerClass { get; set; }
        public ulong partyId { get; set; }
        public uint hp { get; set; }
        public uint maxHp { get; set; }
        public uint mp { get; set; }
        public uint maxMp { get; set; }
        public uint stamina { get; set; }
        public uint maxStamina { get; set; }

        public TeraPlayer(ulong id, string name, ushort level = 1, PlayerClass playerClass = PlayerClass.Empty, ulong partyId = 0)
            : base(id, name)
        {
            this.level = level;
            this.playerClass = playerClass;
            this.partyId = partyId;
        }
    }
}