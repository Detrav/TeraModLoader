using Detrav.TeraApi;
using Detrav.TeraApi.Events;
using Detrav.TeraApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Detrav.TeraModLoader.Core
{
    class TeraClient : ITeraClient
    {
        public event OnPacketArrival onPacketArrival;
        private ITeraMod[] mods;

        public void unLoad()
        {
            foreach (var mod in mods)
            {
                mod.unLoad();
            }
        }

        public void load(ITeraMod[] p)
        {
            mods = p;
            foreach(var mod in mods)
            {
                mod.load(this);
            }
        }

        internal void PacketArrival(TeraPacketWithData teraPacketWithData)
        {
            if (onPacketArrival != null)
                onPacketArrival(this, new PacketArrivalEventArgs(teraPacketWithData));
        }
    }
}
