using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.Core
{
    public class TeraEntity
    {
        public ulong id { get; set; }
        public TeraEntity parent { get; set; }
        public string name { get; set; }
        public TeraEntity(ulong id, TeraEntity parent = null,string name = null)
        {
            this.id = id;
            this.parent = parent;
            this.name = name;
        }

        public TeraEntity(ulong id, string name = null, TeraEntity parent = null)
        {
            this.id = id;
            this.name = name;
            this.parent = parent;
        }
    }
}
