using Detrav.TeraApi;
using Detrav.TeraApi.Core;
using Detrav.TeraApi.DataBase;
using Detrav.TeraApi.Enums;
using Detrav.TeraApi.Events;
using Detrav.TeraApi.Events.Party;
using Detrav.TeraApi.Events.Player;
using Detrav.TeraApi.Events.Self;
using Detrav.TeraApi.Interfaces;
using Detrav.TeraApi.OpCodes;
using Detrav.TeraApi.OpCodes.P2904;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Detrav.TeraModLoader.Core.P2904
{
    internal class TeraClient : ITeraClientWithLoader
    {
        public event OnPacketArrival onPacketArrival;
        public event OnTick onTick;
        public event OnLogin onLogin;
        public event OnUpdateCharacteristic onUpdateCharacteristic;
        public event OnNewPartyList onNewPartyList;
        public event OnSkillResult onSkillResult;

        private Dictionary<ulong, TeraPlayer> party = new Dictionary<ulong, TeraPlayer>();
        private TeraPlayer self = null;
        Dictionary<ulong, TeraEntity> entities = new Dictionary<ulong,TeraEntity>();

        CacheManager cacheManager = new CacheManager();


        private ITeraMod[] mods;

        public void unLoad()
        {
            foreach (var mod in mods)
            {
                mod.unLoad();
            }
        }

        public void load(ITeraMod[] p)
        {
            mods = p;
            foreach (var mod in mods)
            {
                mod.load(this);
            }
        }

        public void PacketArrival(TeraPacketWithData teraPacketWithData)
        {
            switch ((OpCode2904)teraPacketWithData.opCode)
            {
                case OpCode2904.S_LOGIN:
                    {
                        entities.Clear();
                        var s_login = (S_LOGIN)PacketCreator.create(teraPacketWithData);
                        Logger.debug(s_login.ToString());
                        self = new TeraPlayer(s_login.id, s_login.name, s_login.level);
                        clearParty();
                        party[self.id] = self;
                        entities[self.id] = self;
                        self.partyId = ulong.MaxValue;
                        if (onLogin != null) onLogin(this, new LoginEventArgs(self));
                        if (onNewPartyList != null) onNewPartyList(this, new NewPartyListEventArgs(party.Values.ToArray()));
                    }
                    break;
                case OpCode2904.S_PARTY_MEMBER_LIST:
                    {
                        var s_party_list = (S_PARTY_MEMBER_LIST)PacketCreator.create(teraPacketWithData);
                        Logger.debug(s_party_list.ToString());
                        clearParty();
                        foreach (var p in s_party_list.players)
                        {
                            TeraPlayer pl = getPlayer(p.id);
                            if(pl==null)
                            {
                                pl = new TeraPlayer(p.id, p.name, p.level, p.playerClass, p.partyId);
                                party[p.id] = pl;
                            }
                            else
                            {
                                //pl.name = p.name;
                                pl.partyId = p.partyId;
                                pl.playerClass = p.playerClass;
                                pl.level = p.level;
                            }
                        }
                        if (onNewPartyList != null) onNewPartyList(this, new NewPartyListEventArgs(party.Values.ToArray()));
                    }
                    break;
                case OpCode2904.S_LEAVE_PARTY:
                    {
                        Logger.debug("S_LEAVE_PARTY");
                        clearParty();
                        party[self.partyId] = self;
                        if (onNewPartyList != null) onNewPartyList(this, new NewPartyListEventArgs(party.Values.ToArray()));
                        break;
                    }
                case OpCode2904.S_LEAVE_PARTY_MEMBER:
                    {
                        var s_leave_member = (S_LEAVE_PARTY_MEMBER)PacketCreator.create(teraPacketWithData);
                        Logger.debug(s_leave_member.ToString());
                        ulong remId = 0;
                        foreach (var pair in party)
                            if (pair.Value.partyId == s_leave_member.partyId)
                            {
                                remId = pair.Key;
                                break;
                            }
                        if (remId != 0)
                        {
                            party[remId].partyId = 0;
                            party.Remove(remId);
                        }
                        if (onNewPartyList != null) onNewPartyList(this, new NewPartyListEventArgs(party.Values.ToArray()));
                    }
                    break;
                case OpCode2904.S_SPAWN_PROJECTILE:
                    {
                        Logger.debug("S_SPAWN_PROJECTILE");
                        var s_spawn_proj = (S_SPAWN_PROJECTILE)PacketCreator.create(teraPacketWithData);
                        if (entities.ContainsKey(s_spawn_proj.idPlayer))
                            entities[s_spawn_proj.id] = new TeraEntity(s_spawn_proj.id, entities[s_spawn_proj.idPlayer]);
                    }
                    break;
                case OpCode2904.S_DESPAWN_PROJECTILE:
                    {
                        Logger.debug("S_DESPAWN_PROJECTILE");
                        var s_despawn_proj = (S_DESPAWN_PROJECTILE)PacketCreator.create(teraPacketWithData);
                        if (entities.ContainsKey(s_despawn_proj.id)) entities.Remove(s_despawn_proj.id);
                    }
                    break;
                case OpCode2904.S_SPAWN_NPC:
                    {
                        var s_spawn_npc = (S_SPAWN_NPC)PacketCreator.create(teraPacketWithData);
                        Logger.debug(s_spawn_npc.ToString());
                        TeraEntity te = null;
                        if (s_spawn_npc.parentId != 0)
                            entities.TryGetValue(s_spawn_npc.parentId, out te);
                        entities[s_spawn_npc.id] = new TeraNpc(s_spawn_npc.id, cacheManager.getNpc(s_spawn_npc.header, s_spawn_npc.template), te);
                    }
                    break;
                case OpCode2904.S_DESPAWN_NPC:
                    var s_despawn_npc = (S_DESPAWN_NPC)PacketCreator.create(teraPacketWithData);
                    Logger.debug(s_despawn_npc.ToString());
                    if (entities.ContainsKey(s_despawn_npc.id)) entities.Remove(s_despawn_npc.id);
                    break;
                case OpCode2904.S_EACH_SKILL_RESULT:
                    if (onSkillResult != null)
                    {
                        var skill = (S_EACH_SKILL_RESULT)PacketCreator.create(teraPacketWithData);
                        Logger.debug(skill.ToString());
                        //Logger.debug(s_despawn_npc.ToString());
                        /*
             * Проверяем если есть такой игрок с ай ди, то делаем что нужно и выходим
             * Проверяем если есть такой ловушк с ай ди, то ищем НПС или игрока
             * Проверяем если находим НПС ищем игрока
             */
                        try
                        {
                            if (entities.ContainsKey(skill.idWho))
                            {
                                TeraEntity te = entities[skill.idWho];
                                if (entities.ContainsKey(skill.idTarget))
                                    onSkillResult(this, new SkillResultEventArgs(skill.damage, skill.dType, te, entities[skill.idTarget], skill.crit));
                                else onSkillResult(this, new SkillResultEventArgs(skill.damage, skill.dType, te, null, skill.crit));
                            }
                        }
                        catch (Exception exc)
                        {
                            Logger.debug("{0}", Core.Data.ExceptionExtended.GetExceptionDetails(exc));
                        }
                    }
                    break;
                case OpCode2904.S_PARTY_MEMBER_CHANGE_HP:
                    {

                    }
                    break;
                case OpCode2904.S_NPC_STATUS:
                    {
                        var npc_status = PacketCreator.create(teraPacketWithData) as S_NPC_STATUS;
                        Logger.debug(npc_status.ToString());
                        if(entities.ContainsKey(npc_status.npcId))
                        {
                            TeraEntity npc = entities[npc_status.npcId];
                            if(npc is TeraNpc)
                            {
                                if(entities.ContainsKey(npc_status.playerId))
                                    (npc as TeraNpc).target = entities[npc_status.playerId];
                                else
                                    (npc as TeraNpc).target = null;
                            }
                        }
                    }
                    break;
                case OpCode2904.S_SPAWN_USER:
                    {
                        var s_spawn_user = PacketCreator.create(teraPacketWithData) as S_SPAWN_USER;
                        Logger.debug(s_spawn_user.ToString());
                        var player = getPlayer(s_spawn_user.id);
                        if (player == null) player = new TeraPlayer(s_spawn_user.id, s_spawn_user.name);
                        entities[s_spawn_user.id] = player;
                    }
                    break;
                case OpCode2904.S_DESPAWN_USER:
                    {
                        var s_despawn_user = PacketCreator.create(teraPacketWithData) as S_DESPAWN_USER;
                        Logger.debug(s_despawn_user.ToString());
                        if (entities.ContainsKey(s_despawn_user.id))
                            entities.Remove(s_despawn_user.id);
                    }
                    break;
            }
            if (onPacketArrival != null)
                onPacketArrival(this, new PacketArrivalEventArgs(teraPacketWithData));
        }

        public void doEvents()
        {
            if (onTick != null)
                onTick(this, EventArgs.Empty);
        }

        public TeraPlayer getPlayerSelf()
        {
            return self;
        }

        public NpcDataBase getNpcDataBaseByUlongId(ulong id)
        {
            return cacheManager.getNpc(id);
        }

        private void clearParty()
        {
            foreach(var pair in party)
            {
                pair.Value.partyId = 0;
            }
            party.Clear();
        }

        public TeraPlayer getPlayer(ulong id)
        {
            if(id == self.id) return self;
            if (party.ContainsKey(id)) return party[id];
            if (entities.ContainsKey(id))
                if (entities[id] is TeraPlayer)
                    return entities[id] as TeraPlayer;
            return null;
        }


        public TeraEntity[] getEntities()
        {
            return entities.Values.ToArray();
        }

        public TeraNpc[] getNpcs()
        {
            List<TeraNpc> npcs = new List<TeraNpc>();
            foreach(var e in entities)
            {
                if (e.Value is TeraNpc)
                    npcs.Add(e.Value as TeraNpc);
            }
            return npcs.ToArray();
        }

        public TeraPlayer[] getPlayers()
        {
            List<TeraPlayer> npcs = new List<TeraPlayer>();
            foreach (var e in entities)
            {
                if (e.Value is TeraPlayer)
                    npcs.Add(e.Value as TeraPlayer);
            }
            return npcs.ToArray();
        }
    }
}
