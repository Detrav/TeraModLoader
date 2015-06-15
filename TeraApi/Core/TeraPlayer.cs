using Detrav.TeraApi.Enums;
namespace Detrav.TeraApi.Core
{
    public class TeraPlayer : TeraEntity
    {
        public ushort level { get; set; }
        public PlayerClass playerClass { get; set; }

        public TeraPlayer(ulong id, string name, ushort level = 1, PlayerClass playerClass = PlayerClass.Empty)
            : base(id, name)
        {
            this.level = level;
            this.playerClass = playerClass;
        }
    }
}