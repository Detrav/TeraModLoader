using Detrav.TeraApi.OpCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraModLoader.Core.Data
{
    internal class MyConfig
    {
        public SortedList<string, bool> modEnable = new SortedList<string,bool>();
        public int deviceIndex = 0;
        public int serverIndex = 0;
        public OpCodeVersion version;
        public int driverType;
    }

    
}
