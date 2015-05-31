using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.OpCodes.P2904
{
    public class S_PARTY_MEMBER_LIST : TeraPacket
    {
        Player[] players;
        public S_PARTY_MEMBER_LIST(TeraPacketWithData packet)
            : base(packet)
        {
            List<Player> list = new List<Player>();
            ushort count = packet.toUInt16(4);
            int current = 53;
            for(int i = 0; i< count; i++)
            {
                ushort nameStart = packet.toUInt16(current);
                ulong _id = packet.toUInt64(current + 19);
                string _name = packet.toDoubleString(nameStart, nameStart + 100);
                current = nameStart + (_name.Length + 1) * 2 + 4;
                list.Add(new Player() { id = _id, name = _name });
            }
            players = list.ToArray();
        }

        public class Player
        {
            public ulong id;
            public string name;
        }
    }
}
