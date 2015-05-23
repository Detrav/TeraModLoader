using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraModLoader.TeraApi
{
    public static class Logger
    {
        public static void log(string format,params object[] args )
        {
            Console.WriteLine("{0:yyyy MM dd HH:mm:ss} {1,8}:{2}",DateTime.Now,"[Log]",String.Format(
                format,args));
        }

        public static void debug(string format, params object[] args)
        {
            Console.WriteLine("{0:yyyy MM dd HH:mm:ss} {1,8}:{2}", DateTime.Now, "[Debug]", String.Format(
                format, args));
        }

        public static void info(string format, params object[] args)
        {
            Console.WriteLine("{0:yyyy MM dd HH:mm:ss} {1,8}:{2}", DateTime.Now, "[Info]", String.Format(
                format, args));
        }
    }
}
