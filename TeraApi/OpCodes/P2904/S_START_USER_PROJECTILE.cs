﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.OpCodes.P2904
{
    //Получем, когда рядом кто то использует ловушку
    public class S_START_USER_PROJECTILE :TeraPacket
    {
        public ulong id { get; set; }
        public ulong idPlayer { get; set; }
        public S_START_USER_PROJECTILE(TeraPacketWithData packet)
            : base(packet)
        {
            id = packet.toUInt64(20);// readUInt64(4, "id");
            idPlayer = packet.toUInt64(4);// readUInt64(49, "player id");
        }
    }
}
