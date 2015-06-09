using Detrav.TeraApi.Core;
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
        string version2904 = "P2904\\npcs.json";
        internal Dictionary<ulong, NpcDataBase> teraNpcs;

        public NpcDataBase getNpc(ushort header, uint id)
        {
            return getNpc(((ulong)header << 32) + id);
        }

        public NpcDataBase getNpc(ulong id)
        {
            if (teraNpcs == null)
            {
                NpcDataBase[] db = (NpcDataBase[])assetManager.deSerialize(version2904, typeof(NpcDataBase[]));
                if (db == null) return null;
                teraNpcs = new Dictionary<ulong, NpcDataBase>(db.Length + 1000);
                foreach (var npc in db)
                {
                    //if (teraNpcs.ContainsKey(((ulong)npc.header << 32) + npc.id)) throw new ArgumentException((((ulong)header << 32) + id).ToString());
                    teraNpcs.Add(npc.ulongId, npc);
                    //teraNpcs.Add(String.Format(""));
                }
            }
            NpcDataBase result = null;
            if (!teraNpcs.TryGetValue(id, out result))
            {
                result = new NpcDataBase() { name = id.ToString() };
                result.ulongId = id;
                teraNpcs[id] = result;
            }
            return result;
            //throw new NotImplementedException();
        }
    }
}
