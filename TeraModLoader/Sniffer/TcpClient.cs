using Detrav.TeraModLoader.Crypt;
using Detrav.TeraModLoader.TeraApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraModLoader.Sniffer
{
    internal class TcpClient
    {
        internal struct tcp_frag
        {
            public uint seq;
            public int len;
            public byte[] data;
        };
        //TCP
        List<tcp_frag>[] frags = new List<tcp_frag>[2];
        uint[] seq = new uint[2];
        ushort[] src_port = new ushort[2];
        bool closed = false;
        //Tera
        private int state;
        private Session session;

        private byte[] recvStream;
        private byte[] sendStream;
        private Queue<TeraPacketWithData> teraPackets = new Queue<TeraPacketWithData>();
        private static byte[] initPacket = new byte[4] { 0x01, 0x00, 0x00, 0x00 };

        public TcpClient()
        {
            // TODO: Complete member initialization
            frags[0] = new List<tcp_frag>();
            frags[1] = new List<tcp_frag>();
            reset_tcp_reassembly();
        }

        internal void reConstruction(PacketDotNet.TcpPacket tcpPacket)
        {
            if (tcpPacket.PayloadData == null || tcpPacket.PayloadData.Length == 0) return;

            reassemble_tcp(
                tcpPacket.SequenceNumber,//Номер пакета
                tcpPacket.PayloadData.Length,//Длина данных
                (byte[])tcpPacket.PayloadData.Clone(),//Копия данных пакета, т.к. я уверен могут быть проблемы
                tcpPacket.SourcePort, tcpPacket.DestinationPort//Номера портов, т.к. мне нужен реконструктор а не снифер
                );
        }

        private void reassemble_tcp(uint sequence, int length, byte[] data, ushort srcport, ushort dstport)
        {
            int src_index = -1;//Номер обрабатываемого случая
            bool first = false;//Первые пакеты, если да то присваеваем значения
            //Теперь идём по чужой фукнции и пишем свой вариант
            //сэкономим место сделаем без скобок
            for (int j = 0; j < 2; j++)
                if (src_port[j] == srcport)
                    src_index = j;
            //Если на нашли нужный случай
            if (src_index < 0)
                for (int j = 0; j < 2; j++)
                    if (src_port[j] == 0)
                    {
                        src_port[j] = srcport;
                        src_index = j;
                        first = true;
                        break;
                    }
            //оставим throw, хотя я думаю такой ситуации не может быть
            if (src_index < 0) throw new Exception("ERROR in reassemble_tcp: Too many ports!");

            //Далее возможны 4 случая
            //1)У нас пакет первый
            //2)Начало полученного пакета перекрывается с уже имеющимися
            //3)Всё ок и пакет можно добавить в поток данных
            //4)Получили окно и тогда нужно кидать пакет в список
            if (first)//1
            {
                Logger.debug("Первый пакет от {0} с номером {1} и длиной {2}", srcport, sequence, length);
                seq[src_index] = sequence + (uint)length;
                process(src_port[src_index], data);
                return;
            }
            if (sequence < seq[src_index])//2
            {
                //Нужно отрезать кусок или выкинуть пакет
                //Если пакет оказался меньше чем нужно то выкидваем его
                if (sequence + length <= seq[src_index])
                {
                    Logger.debug("Лишний пакет от {0} с номером {1} и длиной {2}", srcport, sequence, length);
                    return;
                }
                //иначе вычислваем длину перекрывающегося куска
                uint new_len = seq[src_index] - sequence;

                length -= (int)new_len;
                byte[] tmpData = new byte[length];
                for (int i = 0; i < length; i++)
                    tmpData[i] = data[i + new_len];
                //данные присваеваем
                data = tmpData;
                //Теперь нужно подправить данные, чтобы прога думала что пакет подходит
                sequence = seq[src_index];
                Logger.debug("Обрезаный пакет от {0} с номером {1} и длиной {2}", srcport, sequence, length);
            }
            if (sequence == seq[src_index])//3
            {
                //debug("Обычный пакет от {0} с номером {1} и длиной {2}",srcport,sequence,length);
                seq[src_index] += (uint)length;
                process(src_port[src_index], data);
                while (check_fragments(src_index)) ;//И опять нужно может переписать см. предыдущие комиты
                return;
            }
            //остался 1 случай if(sequence > seq[src_index])//4
            Logger.debug("Оконный пакет от {0} с номером {1} и длиной {2}", srcport, sequence, length);
            frags[src_index].Add(new tcp_frag() { data = data, len = length, seq = sequence });
        }

        bool check_fragments(int index)
        {
            tcp_frag frag;
            for (int i = 0; i < frags[index].Count; i++)
            {
                frag = frags[index][i];
                //и опять несколько случаев (3) :)
                if (frag.seq < seq[index])//1 - Перекрывает
                {
                    if (frag.seq + frag.len <= seq[index])
                    {
                        Logger.debug("Check лишний пакет от {0} с номером {1} и длиной {2}", src_port[index], frag.seq, frag.len);
                        frags[index].RemoveAt(i);
                        return true;
                    }
                    uint new_len = seq[index] - frag.seq;

                    frag.len -= (int)new_len;
                    byte[] tmpData = new byte[frag.len];
                    for (int j = 0; j < frag.len; j++)
                        tmpData[j] = frag.data[j + new_len];
                    frag.data = tmpData;
                    frag.seq = seq[index];
                    Logger.debug("Check обрезаный пакет от {0} с номером {1} и длиной {2}", src_port[index], frag.seq, frag.len);
                }
                if (frag.seq == seq[index])//2 - Подходить
                {
                    Logger.debug("Check нашёлся пакет от {0} с номером {1} и длиной {2}", src_port[index], frag.seq, frag.len);
                    seq[index] += (uint)frag.len;
                    process(src_port[index], frag.data);
                    frags[index].RemoveAt(i);
                    return true;
                }
                //3 - Окно :(
                Logger.debug("Check оконный пакет от {0} с номером {1} и длиной {2}", src_port[index], frag.seq, frag.len);
            }
            return false;
        }


        void reset_tcp_reassembly()
        {
            for (int i = 0; i < 2; i++)
            {
                seq[i] = 0;
                src_port[i] = 0;
                frags[i].Clear();
            }
            Logger.debug("Restart, что то пошло не так!");
        }

        void reStartParser()
        {
            state = 0;
            session = new Session();
            recvStream = new byte[0];
            sendStream = new byte[0];
            lock (teraPackets)
            {
                teraPackets.Clear();
            }
            Logger.debug("Первый пакет 01 00 00 00, чистимся.");
        }


        private void process(uint port, byte[] data)
        {
            // ignore empty packets
            if (data.Length == 0) return;
            //Сервер -> Клиент
            if (port == 7801) recv((byte[])data.Clone());
            else send((byte[])data.Clone());
        }

        internal void recv(byte[] data)
        {
            if (StructuralComparisons.StructuralEqualityComparer.Equals(initPacket, data))
            {
                reStartParser();
                return;
            }
            switch (state)
            {
                case 0:
                    if (data.Length != 128)
                        return;
                    session.ServerKey1 = (byte[])data.Clone(); ;
                    state++;
                    return;
                case 1:
                    if (data.Length != 128)
                        return;
                    session.ServerKey2 = (byte[])data.Clone();
                    session.Init();
                    state++;
                    return;
                default:
                    session.Encrypt(ref data);
                    Array.Resize(ref recvStream, recvStream.Length + data.Length);
                    Array.Copy(data, 0, recvStream, recvStream.Length - data.Length, data.Length);
                    while (processRecv()) ;
                    return;
            }
        }

        private bool processRecv()
        {
            if (recvStream.Length < 4)
                return false;
            ushort length = BitConverter.ToUInt16(recvStream, 0);
            if (recvStream.Length < length)
                return false;
            var packet = new TeraPacketWithData(getRecvData(length), TeraPacketWithData.Type.Recv);
            lock (teraPackets)
            {
                teraPackets.Enqueue(packet);
            }
            return true;
        }

        private byte[] getRecvData(ushort length)
        {
            byte[] result = new byte[length];
            Array.Copy(recvStream, result, length);
            byte[] reserve = (byte[])recvStream.Clone();
            recvStream = new byte[recvStream.Length - length];
            Array.Copy(reserve, length, recvStream, 0, recvStream.Length);
            return result;
        }



        internal void send(byte[] data)
        {
            switch (state)
            {
                case 0:
                    if (data.Length != 128)
                        return;
                    session.ClientKey1 = (byte[])data.Clone();
                    return;
                case 1:
                    if (data.Length != 128)
                        return;
                    session.ClientKey2 = (byte[])data.Clone();
                    return;
                default:
                    session.Decrypt(ref data);
                    Array.Resize(ref sendStream, sendStream.Length + data.Length);
                    Array.Copy(data, 0, sendStream, sendStream.Length - data.Length, data.Length);
                    while (processSend()) ;
                    return;
            }
        }

        private bool processSend()
        {
            if (sendStream.Length < 4)
                return false;
            ushort length = BitConverter.ToUInt16(sendStream, 0);
            if (sendStream.Length < length)
                return false;
            var packet = new TeraPacketWithData(getSendData(length), TeraPacketWithData.Type.Send);
            lock (teraPackets)
            {
                teraPackets.Enqueue(packet);
            }
            return true;
        }

        private byte[] getSendData(ushort length)
        {
            byte[] result = new byte[length];
            Array.Copy(sendStream, result, length);
            byte[] reserve = (byte[])sendStream.Clone();
            sendStream = new byte[sendStream.Length - length];
            Array.Copy(reserve, length, sendStream, 0, sendStream.Length);
            return result;
        }

        internal TeraPacketWithData getPacketSync()
        {
            lock (teraPackets)
            {
                if (teraPackets.Count == 0)
                    return null;
                return teraPackets.Dequeue();
            }
        }
    }
}
