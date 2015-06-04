using System;
using Detrav.TeraApi.Core;
using Detrav.TeraApi.Enums;

namespace Detrav.TeraApi.Events.Party
{
    public class LeaveMemberFromPartyEventArgs : EventArgs
    {
        public TeraPlayer player {get;private set;}
        public LeaveMemberFromPartyEventArgs(TeraPlayer player)
        {
            this.player = player;
        }
    }
    public delegate void OnLeaveFromParty(object sender, LeaveMemberFromPartyEventArgs e);
}