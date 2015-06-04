using System;
using Detrav.TeraApi.Core;
using Detrav.TeraApi.Enums;

namespace Detrav.TeraApi.Events.Party
{
    public class NewPartyListEventArgs : EventArgs
    {
        public TeraPlayer[] players {get;private set;}
        public NewPartyListEventArgs(TeraPlayer[] players)
        {
            this.players = players;
        }
    }
    public delegate void OnNewPartyList(object sender, NewPartyListEventArgs e);
}