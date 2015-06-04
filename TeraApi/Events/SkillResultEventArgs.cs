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
        public uint damage;
        public ushort type;
        public TeraPlayer player;

        public SkillResultEventArgs(uint damage, ushort type,TeraPlayer player)
        {
            this.damage = damage;
            this.type = type;
            this.player = player;
        }
    }

    public delegate void OnMakeSkillResult(object sender, SkillResultEventArgs e);
    public delegate void OnTakeSkillResult(object sender, SkillResultEventArgs e);
}
