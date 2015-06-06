using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraModLoader.Sniffer
{
    interface ICapture : IDisposable
    {
        event OnPacketArrival onPacketArrivalSync;
        event OnNewConnection onNewConnectionSync;
        event OnEndConnection onEndConnectionSync;
        void doEventSync();
    }
}
