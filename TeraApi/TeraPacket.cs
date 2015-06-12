using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi
{
    public class TeraPacket
    {
        public PacketType type { get; set; }
        public ushort size { get; set; }
        public ushort opCode { get; set; }


        public TeraPacket(TeraPacketWithData p)
        {
            type = p.type;
            size = p.toUInt16(0);
            opCode = p.toUInt16(2);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(this.GetType().ToString());
            PrintProperties(sb, this, 1);
            return sb.ToString();
        }

        public void PrintProperties(StringBuilder sb, object obj, int indent)
        {

            if (obj == null) return;
            string indentString = new string(' ', indent);
            Type objType = obj.GetType();
            PropertyInfo[] properties = objType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object propValue = property.GetValue(obj, null);
                if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
                    sb.AppendLine(String.Format("{0}{1}: {2}", indentString, property.Name, propValue));
                else if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    sb.AppendLine(String.Format("{0}{1}:", indentString, property.Name));
                    IEnumerable enumerable = (IEnumerable)propValue;
                    foreach (object child in enumerable)
                        PrintProperties(sb, child, indent + 2);
                }
                else
                {
                    sb.AppendLine(String.Format("{0}{1}:", indentString, property.Name));
                    PrintProperties(sb, propValue, indent + 2);
                }
            }
        }
    }
}
