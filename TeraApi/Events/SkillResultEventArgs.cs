﻿using Detrav.TeraApi.Core;
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
        public TeraEntity who;
        public TeraEntity target;
        public bool crit;

        public SkillResultEventArgs(uint damage, ushort type,TeraEntity who,TeraEntity target,bool crit)
        {
            this.damage = damage;
            this.type = type;
            this.who = who;
            this.target = target;
            this.crit = crit;
        }
    }

    public delegate void OnSkillResult(object sender, SkillResultEventArgs e);
}
