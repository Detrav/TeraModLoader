﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.Interfaces
{
    public interface ITeraClient
    {
        void unLoad();
        void load(ITeraMod[] p);
        
    }
}
