using Detrav.TeraApi;
using Detrav.TeraApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraModLoader.Core
{
    interface ITeraClientWithLoader : ITeraClient
    {
        void load(ITeraMod[] p);
        void unLoad();
        void doEvents();
        void PacketArrival(TeraPacketWithData teraPacketWithData);
    }
}
