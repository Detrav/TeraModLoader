using Detrav.TeraApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraModLoader.Sniffer
{
    internal delegate void OnPacketArrival(object sender, PacketArrivalEventArgs e);
    internal delegate void OnNewConnection(object sender, ConnectionEventArgs e);
    internal delegate void OnEndConnection(object sender, ConnectionEventArgs e);

    internal class PacketArrivalEventArgs : EventArgs
    {
        public Connection connection { get; private set; }
        public TeraPacketWithData packet { get; private set; }
        public PacketArrivalEventArgs(Connection c, TeraPacketWithData p)
        {
            connection = c;
            packet = p;
        }
    }

    internal class ConnectionEventArgs : EventArgs
    {
        public Connection connection { get; private set; }
        public ConnectionEventArgs(Connection c)
        {
            connection = c;
        }
    }
}
