using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi
{
    public static class Logger
    {
        private static bool notInit = true;
        private static void redirectConsole()
        {
            if (!Directory.Exists("logs")) Directory.CreateDirectory("logs");
            Random r = new Random();
            StreamWriter tw = new StreamWriter("logs\\" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + r.Next(0, int.MaxValue).ToString() + ".log");
            tw.AutoFlush = true;
            Console.SetOut(tw);
        }
        public static void log(string format,params object[] args )
        {
            if (notInit) { redirectConsole(); notInit = false; }
            Console.WriteLine("{0:yyyy MM dd HH:mm:ss} {1,8}:{2}",DateTime.Now,"[Log]",String.Format(
                format,args));
        }

        public static void debug(string format, params object[] args)
        {
            if (notInit) { redirectConsole(); notInit = false; }
            Console.WriteLine("{0:yyyy MM dd HH:mm:ss} {1,8}:{2}", DateTime.Now, "[Debug]", String.Format(
                format, args));
        }

        public static void info(string format, params object[] args)
        {
            if (notInit) { redirectConsole(); notInit = false; }
            Console.WriteLine("{0:yyyy MM dd HH:mm:ss} {1,8}:{2}", DateTime.Now, "[Info]", String.Format(
                format, args));
        }
    }
}
