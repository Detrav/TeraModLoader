﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.OpCodes.P2904
{
    //Получаем пакет когда у игрока в пати меняется стамина
    public class S_PARTY_MEMBER_CHANGE_STAMINA :TeraPacket
    {
        ulong partyId;
        uint value;
        uint max;
        public S_PARTY_MEMBER_CHANGE_STAMINA(TeraPacketWithData packet) : base(packet)
        {
            partyId = packet.toUInt64(4);
            value = packet.toUInt32(12);
            max = packet.toUInt32(16);
        }
    }
}
