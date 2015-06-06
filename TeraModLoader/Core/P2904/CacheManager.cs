﻿using Detrav.TeraApi.Core;
using Detrav.TeraApi.DataBase;
using Detrav.TeraApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraModLoader.Core.P2904
{
    class CacheManager : ICacheManager
    {
        AssetManager assetManager = new AssetManager();
        string version = "P2904";
        Dictionary<ulong, NpcDataBase> teraNpcs;

        public NpcDataBase getNpc(ushort header, uint id)
        {
            if (teraNpcs == null)
            {
                NpcDataBase[] db = (NpcDataBase[])assetManager.deSerialize(version, typeof(TeraNpc[]));
                if(db==null) return null;
                teraNpcs = new Dictionary<ulong, NpcDataBase>(db.Length);
                foreach (var npc in db)
                {
                    teraNpcs.Add(((ulong)header << 32) + id, npc);
                    //teraNpcs.Add(String.Format(""));
                }
            }
            NpcDataBase result = null;
            teraNpcs.TryGetValue(((ulong)header << 32) + id,out result);
            return result;
            //throw new NotImplementedException();
        }
    }
}
