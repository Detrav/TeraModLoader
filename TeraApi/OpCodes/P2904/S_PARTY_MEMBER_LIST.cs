using Detrav.TeraApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.OpCodes.P2904
{
    //Получаем когда в пати добавляется игрок
    public class S_PARTY_MEMBER_LIST : TeraPacket
    {
        public Player[] players { get; set; }
        public S_PARTY_MEMBER_LIST(TeraPacketWithData packet)
            : base(packet)
        {
            List<Player> list = new List<Player>();
            ushort count = packet.toUInt16(4);
            int current = 53;
            for(int i = 0; i< count; i++)
            {
                ushort nameStart = packet.toUInt16(current);
                ulong _partyId = packet.toUInt64(current + 2);
                ulong _id = packet.toUInt64(current + 19);
                ushort _level = packet.toUInt16(current + 10);
                PlayerClass _playerClass = (PlayerClass)packet.toByte(current + 14);
                string _name = packet.toDoubleString(nameStart, nameStart + 100);
                current = nameStart + (_name.Length + 1) * 2 + 4;
                list.Add(new Player() { id = _id, name = _name, level = _level, playerClass = _playerClass, partyId = _partyId });
            }
            players = list.ToArray();
        }

        public class Player
        {
            public ulong id { get; set; }
            public ulong partyId { get; set; }
            public string name { get; set; }
            public ushort level { get; set; }
            public PlayerClass playerClass { get; set; }
        }
    }
}
