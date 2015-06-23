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
        [Description("Тестовая версия (Устарелый)")]
        Any,
        [Description("28 05 (Устарелый)")]
        P2805,
        [Description("29 04")]
        P2904
    }
}
