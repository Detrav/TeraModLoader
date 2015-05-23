using Detrav.TeraModLoader.TeraApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraModLoader.Core.Data
{
    internal class Mod
    {
        private Assembly assembly;
        public string name { get; private set; }
        public Version version { get; private set; }
        //icon;
        private Type type;

        public Mod(Assembly assembly)
        {
            ready = false;
            this.assembly = assembly;
            name = assembly.GetName().Name;
            version = assembly.GetName().Version;
            foreach (var v in assembly.GetTypes())
            {
                if (v.GetInterfaces().Contains(typeof(ITeraMod)))
                {
                    type = v;
                    ready = true;
                    break;
                }
            }
        }

        public bool ready { get; private set; }
    }
}
