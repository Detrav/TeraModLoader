using Detrav.TeraApi;
using Detrav.WinpkFilterWrapper;
using PacketDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraModLoader.Sniffer
{
    internal class CaptureWinpkFilter : ICapture
    {
        public event OnPacketArrival onPacketArrivalSync;
        public event OnNewConnection onNewConnectionSync;
        public event OnEndConnection onEndConnectionSync;
        private Dictionary<Connection, TcpClient> tcpClients = new Dictionary<Connection,TcpClient>();
        private Queue<Connection> newConnections = new Queue<Connection>();
        private Queue<Connection> endConnections = new Queue<Connection>();
        TcpFilter tcpFilter;
        public CaptureWinpkFilter(TcpFilter tcpFilter, int deviceNum , string host)
        {
            Logger.debug("new Caputer with {0}", host);
            this.tcpFilter = tcpFilter;
            tcpFilter.onFilterPacketArrival += tcpFilter_onFilterPacketArrival;
            tcpFilter.host = host;
            Logger.debug("tcp and host {0}", host);
            tcpFilter.startASync(deviceNum);
            Logger.debug("StartCapture");
        }

        void tcpFilter_onFilterPacketArrival(object sender, FilterPacketArrivalEventArgs e)
        {
            //Не люблю вары, пока оставлю так, потом переделаю
            var packet = EthernetPacket.ParsePacket(LinkLayers.Ethernet, e.payloadData);
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
                lock (endConnections)
                {
                    endConnections.Enqueue(connection);
                }
            }
        }

        public void doEventSync()
        {
            
            Connection c;
            do
            {
                c = null;
                lock (newConnections)
                {
                    if (newConnections.Count > 0) 
                    c = newConnections.Dequeue();
                }
                if(c!=null)
                    if (onNewConnectionSync != null)
                        onNewConnectionSync(this, new ConnectionEventArgs(c));
            } while (c != null);

            do
            {
                c = null;
                lock (endConnections)
                {
                    if (endConnections.Count > 0) 
                    c = endConnections.Dequeue();
                }
                if(c!=null)
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
            Logger.debug("Dispose Capture");
            if (disposing)
            {
                tcpFilter.Dispose();
            }
            disposed = true;
        }
        ~CaptureWinpkFilter()
        {
            Dispose();
        }        
    }
}
