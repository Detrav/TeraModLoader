using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.OpCodes
{
    public enum OpCodeVersion
    {
        [Description("Тестовая версия")]
        Any,
        [Description("28 05")]
        P2805,
        [Description("29 04")]
        P2904
    }
}
