using System;

namespace Detrav.TeraApi.Events
{
    public class PacketArrivalEventArgs : EventArgs
    {
        public TeraPacketWithData packet { get; private set; }
        public PacketArrivalEventArgs(TeraPacketWithData packet)
        {
            this.packet = packet;
        }
    }
    public delegate void OnPacketArrival(object sender, PacketArrivalEventArgs e);
}
