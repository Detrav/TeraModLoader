using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Detrav.TeraApi.Events;

namespace Detrav.TeraApi.Interfaces
{
    public interface ITeraClient
    {
        event OnPacketArrival onPacketArrival;
        event OnTick onTick;
        
        /*
        Думаю тут сделаю больше событий, может обработанные события??
        */
    }
}
