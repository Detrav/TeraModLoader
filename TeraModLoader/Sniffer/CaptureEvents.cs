using Detrav.TeraModLoader.TeraApi;
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
        Connection connection;
        TeraPacketWithData packet;
        public PacketArrivalEventArgs(Connection c, TeraPacketWithData p)
        {
            connection = c;
            packet = p;
        }
    }

    internal class ConnectionEventArgs : EventArgs
    {
        Connection connection;
        public ConnectionEventArgs(Connection c)
        {
            connection = c;
        }
    }
}
