namespace Detrav.TeraApi.Core
{
    public class TeraPlayer
    {
        ulong id { get; set; }
        string name { get; set; }
        ulong partyId { get; set; }
        uint hp { get; set; }
        uint maxHp { get; set; }
        uint mp { get; set; }
        uint maxMp { get; set; }
        uint stamina { get; set; }
        uint maxStamina { get; set; }

        public TeraPlayer(ulong id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}