using System;
using Detrav.TeraApi.Core;

namespace Detrav.TeraApi.Events.Self
{
    public class LoginEventArgs : EventArgs
    {
        public TeraPlayer player { get; private set; }
        public LoginEventArgs(TeraPlayer player)
        {
            this.player = player;
        }
    }
    public delegate void OnLogin(object sender, LoginEventArgs e);
}