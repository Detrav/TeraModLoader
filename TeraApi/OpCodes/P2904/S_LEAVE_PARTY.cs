﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.OpCodes.P2904
{
    //Получаем пакет когда игрок выходит из пати
    public class S_LEAVE_PARTY : TeraPacket
    {
        public S_LEAVE_PARTY(TeraPacketWithData packet) : base(packet) { }
    }
}
