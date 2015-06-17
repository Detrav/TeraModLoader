using Detrav.TeraApi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.Events
{
    public class SkillResultEventArgs :EventArgs
    {
        public uint damage { get; private set; }
        public ushort type { get; private set; }
        public TeraEntity who { get; private set; }
        public TeraEntity target { get; private set; }
        public bool crit { get; private set; }
        public DateTime time { get; private set; }

        public SkillResultEventArgs(uint damage, ushort type,TeraEntity who,TeraEntity target,bool crit, DateTime time)
        {
            this.damage = damage;
            this.type = type;
            this.who = who;
            this.target = target;
            this.crit = crit;
            this.time = time;
        }
    }

    public delegate void OnSkillResult(object sender, SkillResultEventArgs e);
}
