﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.OpCodes.P2904
{
    //Получаем когда игрок входит или выходит из боя
    public class S_USER_STATUS : TeraPacket
    {
        public ulong id { get; set; }
        public byte status { get; set; }

        public S_USER_STATUS(TeraPacketWithData packet)
            : base(packet)
        {
            id = packet.toUInt64(4);//     readUInt64(4, "id");
            status = packet.toByte(12);//     readByte(12, "status");
        }
    }
}