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
        public string safeName { get { if (name != null) return name; return id.ToString(); } }
        //public uint hp { get; set; }
        public TeraEntity(ulong id, TeraEntity parent)
        {
            this.id = id;
            this.parent = parent;
            this.name = null;
        }

        public TeraEntity(ulong id, string name = null, TeraEntity parent = null)
        {
            this.id = id;
            this.name = name;
            this.parent = parent;
        }
    }
}
