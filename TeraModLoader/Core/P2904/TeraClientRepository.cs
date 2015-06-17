using Detrav.TeraApi.Core;
using Detrav.TeraApi.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraModLoader.Core.P2904
{
    internal partial class TeraClient : ITeraGame
    {
        private Dictionary<ulong, TeraPartyPlayer> party = new Dictionary<ulong, TeraPartyPlayer>();
        private TeraPartyPlayer self = null;
        Dictionary<ulong, TeraEntity> entities = new Dictionary<ulong, TeraEntity>();
        CacheManager cacheManager = new CacheManager();

        public NpcDataBase getNpcDataBaseByUlongId(ulong id)
        {
            return cacheManager.getNpc(id);
        }

        public void AddorUpdatePartyPlayerById(ulong id, TeraPartyPlayer player)
        {
            if (id == self.id)
                replaceSelf(player);
            party[id] = player;
        }
        public TeraPartyPlayer getPartyPlayerByPartyId(ulong partyId)
        {
            if (partyId == self.partyId) return getSelf();
            foreach(var el in party)
            {
                if (el.Value.partyId == partyId)
                    return el.Value;
            }
            return null;
        }
        public TeraPartyPlayer getPartyPlayerById(ulong id)
        {
            if (id == self.id) return getSelf();
            TeraPartyPlayer tpp;
            if (party.TryGetValue(id, out tpp))
                return tpp;
            return null;
        }

        public void removePartyPlayerByPartyId(ulong partyId)
        {
            ulong key = 0;
            foreach (var el in party)
            {
                if (el.Value.partyId == partyId)
                    key = el.Key;
            }
            if (key != 0)
                party.Remove(key);
        }
        public void removePartyPlayerById(ulong id)
        {
            if (party.ContainsKey(id))
                party.Remove(id);
        }

        public TeraPartyPlayer getSelf()
        {
            return self;
        }
        public void replaceSelf(TeraPartyPlayer player)
        {
            self = player;
        }

        public TeraEntity getEntityById(ulong id)
        {
            if (id == self.id)
                return self;
            TeraPartyPlayer tpp;
            if (party.TryGetValue(id, out tpp))
                return tpp;
            TeraEntity te;
            if (entities.TryGetValue(id, out te))
                return te;
            return null;
        }
        public void replaceEntityById(ulong id, TeraEntity entity)
        {
            entities[id] = entity;
        }
        public void removeEntityById(ulong id)
        {
            if (entities.ContainsKey(id))
                entities.Remove(id);
        }

        public void clearDBParty()
        {
            party.Clear();
        }
        public void clearDBEntities()
        {
            entities.Clear();
        }


        public TeraEntity[] getEntities()
        {
            return entities.Values.ToArray();
        }
        public TeraNpc[] getNpcs()
        {
            List<TeraNpc> npcs = new List<TeraNpc>();
            foreach (var pair in entities)
            {
                if (pair.Value is TeraNpc)
                    npcs.Add(pair.Value as TeraNpc);
            }
            return npcs.ToArray();
        }
        public TeraPlayer[] getPlayers()
        {
            List<TeraPlayer> players = new List<TeraPlayer>();
            foreach (var pair in entities)
            {
                if (pair.Value is TeraPlayer)
                    players.Add(pair.Value as TeraPlayer);
            }
            return players.ToArray();
        }
        public TeraPartyPlayer[] getParty()
        {
            List<TeraPartyPlayer> tpps = new List<TeraPartyPlayer>();
            if(self!=null)
            tpps.Add(self);
            foreach(var pair in party)
            {
                tpps.Add(pair.Value);
            }
            return tpps.ToArray();
        }
    }
}
