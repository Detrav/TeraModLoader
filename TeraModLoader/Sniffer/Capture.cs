using Detrav.TeraModLoader.TeraApi;
using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraModLoader.Sniffer
{
    internal class Capture : IDisposable
    {
        internal event OnPacketArrival onPacketArrivalSync;
        internal event OnNewConnection onNewConnectionSync;
        internal event OnEndConnection onEndConnectionSync;

        ICaptureDevice device;
        private Dictionary<Connection, TcpClient> tcpClients = new Dictionary<Connection,TcpClient>();
        private Queue<Connection> newConnections = new Queue<Connection>();
        private Queue<Connection> endConnections = new Queue<Connection>();

        public Capture(ICaptureDevice captureDevice, string host)
        {
            device = captureDevice;
            captureDevice.OnPacketArrival += captureDevice_OnPacketArrival;
            captureDevice.Open(DeviceMode.Promiscuous, 1000);
            captureDevice.Filter = String.Format("tcp and host {0}", host);
            captureDevice.StartCapture();
        }

        void captureDevice_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            //Не люблю вары, пока оставлю так, потом переделаю
            var packet = EthernetPacket.ParsePacket(LinkLayers.Ethernet, e.Packet.Data);
            var ethPacket = packet as EthernetPacket;
            //Автор http://www.theforce.dk/hearthstone/ Описывает как откидывания не нужных пакетов
            if (packet.PayloadPacket == null || ethPacket.Type != EthernetPacketType.IpV4)
                return;
            IPv4Packet ipv4Packet = ethPacket.PayloadPacket as IPv4Packet;
            //А тут видимо отбрасываем если нет содержимого (нужного нам TCP) пакета
            if (ipv4Packet.PayloadPacket == null)
                return;
            TcpPacket tcpPacket = ipv4Packet.PayloadPacket as TcpPacket;
            if (tcpPacket == null)
                return;
            Connection connection = new Connection(tcpPacket);
            TcpClient tcpClient; bool connected = false;
            lock (tcpClients)
            {
                connected = tcpClients.TryGetValue(connection, out tcpClient);
            }
            //Проводим проверку второго из трёх сообщений для соединения, если такой есть то создаём новый клиент
            if (tcpPacket.Syn && tcpPacket.Ack && 0 == tcpPacket.PayloadData.Length && !connected)
            {
                tcpClient = new TcpClient();
                tcpClients.Add(connection, tcpClient);
                connected = true;
                Logger.log("Новое соединение: " + connection.ToString());
                lock (newConnections)
                {
                    newConnections.Enqueue(connection);
                }
            }

            if (tcpPacket.Ack && connected)
            {
                tcpClient.reConstruction(tcpPacket);
            }

            if (tcpPacket.Fin && tcpPacket.Ack && connected)
            {
                tcpClients.Remove(connection);
                Logger.log("Конец соединения: " + connection.ToString());
                lock(endConnections)
                {
                    endConnections.Enqueue(connection);
                }
            }
        }

        internal void doEventSync()
        {
            
            Connection c;
            do
            {
                c = null;
                lock (newConnections)
                {
                    if (newConnections.Count > 0) ;
                    c = newConnections.Dequeue();
                }
                if (onNewConnectionSync != null)
                    onNewConnectionSync(this, new ConnectionEventArgs(c));
            } while (c != null);

            do
            {
                lock (endConnections)
                {
                    if (endConnections.Count > 0) ;
                    c = endConnections.Dequeue();
                }
                if (onEndConnectionSync != null)
                    onEndConnectionSync(this, new ConnectionEventArgs(c));
            } while (c != null);

            TcpClient client;
            Dictionary<Connection, TcpClient>.KeyCollection keys;
            lock (tcpClients)
            {
                keys = tcpClients.Keys;
            }
            foreach (var key in keys)
            {
                bool ye = false;
                lock (tcpClients)
                {
                    ye = tcpClients.TryGetValue(key, out client);
                }
                if (ye)
                {
                    TeraPacketWithData t;
                    do
                    {
                        t = client.getPacketSync();
                        if (t != null)
                        {
                            //Logger.info("{0}", t);
                            if (onPacketArrivalSync!=null)
                                onPacketArrivalSync(this,new PacketArrivalEventArgs(key,t));
                        }

                    }
                    while (t != null);
                }
            }
        }


        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                device.StopCapture();
            }
            disposed = true;
        }
        ~Capture()
        {
            Dispose();
        }        
    }
}
