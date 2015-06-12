using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.OpCodes.P2904
{
    //Получаем когда рядом появляется игрок
    public class S_SPAWN_USER : TeraPacket
    {
        public ulong id { get; set; }
        public string name { get; set; }

        public S_SPAWN_USER(TeraPacketWithData packet)
            : base(packet)
        {
            ushort name_start = packet.toUInt16(8);
            //ushort guild_start = readUInt16(10, "guild start");
            //ushort guild_rank_start = readUInt16(12, "guild rank start");
            //ushort guild_title_start = readUInt16(20, "guild title start");
            id = packet.toUInt64(34);
            //readUInt64(34, "id");
            //readUInt64(38, "unique id");
            name = packet.toDoubleString(name_start, name_start+ 100);
            //readString(name_start, "name");
            //readString(guild_start, "guild");
            //readString(guild_rank_start, "guild rank");
            //readString(guild_title_start, "guild title");
        }
    }
}
