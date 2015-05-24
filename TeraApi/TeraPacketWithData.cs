using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi
{
    public enum PacketType { Any, Send, Recv }
    public class TeraPacketWithData
    {
        public PacketType type { get; set; }
        public ushort size { get; set; }
        public ushort opCode { get; set; }
        public byte[] data { get; private set; }
        public TeraPacketWithData(byte[] _data, PacketType _type)
        {
            data = (byte[])_data.Clone();
            size = toUInt16(0);//Размер
            opCode = toUInt16(2);//id пакета или OpCode
            type = _type;
        }


        static public byte toByte(byte[] data, int b)
        {
            return data[b];
        }
        static public sbyte toSByte(byte[] data, int b)
        {
            return (sbyte)data[b];
        }
        static public ushort toUInt16(byte[] data, int b)
        {
            return BitConverter.ToUInt16(data, b);
        }
        static public short toInt16(byte[] data, int b)
        {
            return BitConverter.ToInt16(data, b);
        }
        static public uint toUInt32(byte[] data, int b)
        {
            return BitConverter.ToUInt32(data, b);
        }
        static public int toInt32(byte[] data, int b)
        {
            return BitConverter.ToInt32(data, b);
        }
        static public ulong toUInt64(byte[] data, int b)
        {
            return BitConverter.ToUInt64(data, b);
        }
        static public long toInt64(byte[] data, int b)
        {
            return BitConverter.ToInt64(data, b);
        }
        static public float toSingle(byte[] data, int b)
        {
            return BitConverter.ToSingle(data, b);
        }
        static public double toDouble(byte[] data, int b)
        {
            return BitConverter.ToDouble(data, b);
        }
        static public char toSingleChar(byte[] data, int b)
        {
            return BitConverter.ToChar(data, b);
        }
        static public char toDoubleChar(byte[] data, int b)
        {
            return Convert.ToChar(BitConverter.ToUInt16(data, b));
        }
        static public string toSingleString(byte[] data, int b, int e)
        {
            StringBuilder result = new StringBuilder();
            for (int i = b; i < e; i++)
            {
                char c = toSingleChar(data, i);
                if (char.IsControl(c)) result.Append('.');
                else result.Append(c);
            }
            return result.ToString();
        }
        static public string toDoubleString(byte[] data, int b, int e)
        {
            StringBuilder result = new StringBuilder();
            for (int i = b; i < e; i += 2)
            {
                char c = toDoubleChar(data, i);
                if (c == '\0') break;
                else result.Append(c);
            }
            return result.ToString();
        }
        static public bool toBoolean(byte[] data, int b)
        {
            return data[b] > 0;
        }
        static public string toHex(byte[] data, int b, int e, string split = "")
        {
            StringBuilder result = new StringBuilder();
            for (int i = b; i < e; i++)
            {
                result.AppendFormat("{0:X2}", data[i]);
                if (split.Length > 0)
                    result.Append(split);
            }
            return result.ToString();
        }

        public byte toByte(int b)
        {
            return toByte(data,b);
        }
        public sbyte toSByte(int b)
        {
            return toSByte(data,b);
        }
        public ushort toUInt16(int b)
        {
            return toUInt16(data, b);
        }
        public short toInt16(int b)
        {
            return toInt16(data, b);
        }
        public uint toUInt32(int b)
        {
            return toUInt32(data, b);
        }
        public int toInt32(int b)
        {
            return toInt32(data,b);
        }
        public ulong toUInt64(int b)
        {
            return toUInt64(data, b);
        }
        public long toInt64(int b)
        {
            return toInt64(data, b);
        }
        public float toSingle(int b)
        {
            return toSingle(data, b);
        }
        public double toDouble(int b)
        {
            return toDouble(data, b);
        }
        public char toSingleChar( int b)
        {
            return toSingleChar(data,b);
        }
        public char toDoubleChar(int b)
        {
            return toDoubleChar(data, b);
        }
        public string toSingleString(int b, int e)
        {
            return toSingleString(data, b, e);
        }
        public string toDoubleString(int b, int e)
        {
            return toDoubleString(data, b, e);
        }
        public bool toBoolean(int b)
        {
            return toBoolean(data, b);
        }
        public string toHex(int b, int e, string split = "")
        {
            return toHex(data, b, e, split);
        }



        public override string ToString()
        {
            return String.Format("{0,6} {1,6} {2}", size, opCode, toHex(0, data.Length));
        }
    }
}
