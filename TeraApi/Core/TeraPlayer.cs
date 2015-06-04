namespace Detrav.TeraApi.Core
{
    public class TeraPlayer
    {
        public ulong id { get; set; }
        public string name { get; set; }
        public ushort level { get; set; }
        public ulong partyId { get; set; }
        public uint hp { get; set; }
        public uint maxHp { get; set; }
        public uint mp { get; set; }
        public uint maxMp { get; set; }
        public uint stamina { get; set; }
        public uint maxStamina { get; set; }

        public TeraPlayer(ulong id, string name, ushort level)
        {
            this.id = id;
            this.name = name;
            this.level = level;
        }
    }
}