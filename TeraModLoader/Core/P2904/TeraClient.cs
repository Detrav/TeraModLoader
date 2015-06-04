using Detrav.TeraApi;
using Detrav.TeraApi.Core;
using Detrav.TeraApi.Events;
using Detrav.TeraApi.Events.Party;
using Detrav.TeraApi.Events.Player;
using Detrav.TeraApi.Events.Self;
using Detrav.TeraApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Detrav.TeraModLoader.Core.P2904
{
    internal class TeraClient : ITeraClientWithLoader
    {
        public event OnPacketArrival onPacketArrival;
        public event OnTick onTick;
        public event OnLogin onLogin;
        public event OnUpdateCharacteristic onUpdateCharacteristic;
        public event OnNewPartyList onNewPartyList;
        public event OnLeaveFromParty onLeaveFromParty;

        public Dictionary<ulong, TeraPlayer> party = new Dictionary<ulong, TeraPlayer>();
        public TeraPlayer self = null;

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

        public void PacketArrival(TeraPacketWithData teraPacketWithData)
        {
            //switch()
            if (onPacketArrival != null)
                onPacketArrival(this, new PacketArrivalEventArgs(teraPacketWithData));
        }

        public void doEvents()
        {
            if (onTick != null)
                onTick(this, EventArgs.Empty);
        }

        public TeraPlayer getPlayerById(ulong id)
        {
            return self;
        }


        public TeraPlayer getPlayerSelf()
        {
            return self;
        }
    }
}
