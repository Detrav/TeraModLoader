using Detrav.TeraApi;
using Detrav.TeraApi.Core;
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

        private Dictionary<ulong, TeraPlayer> party = new Dictionary<ulong, TeraPlayer>();
        private TeraPlayer self = null;
        Dictionary<ulong, ulong> projectiles = new Dictionary<ulong, ulong>();
        Dictionary<ulong, ulong> npcs = new Dictionary<ulong, ulong>();



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
                    Logger.debug("S_LOGIN");
                    var s_login = (S_LOGIN)PacketCreator.create(teraPacketWithData);
                    self = new TeraPlayer(s_login.id, s_login.name, s_login.level);
                    party.Clear();
                    party[self.id] = self;
                    if (onLogin != null) onLogin(this, new LoginEventArgs(self));
                    if (onNewPartyList != null) onNewPartyList(this, new NewPartyListEventArgs(party.Values.ToArray()));
                    break;
                case OpCode2904.S_PARTY_MEMBER_LIST:
                    Logger.debug("S_PARTY_MEMBER_LIST");
                    var s_party_list = (S_PARTY_MEMBER_LIST)PacketCreator.create(teraPacketWithData);
                    party.Clear();
                    foreach (var p in s_party_list.players)
                    {
                        if (p.id == self.id)
                        {
                            self.level = p.level;
                            self.playerClass = p.playerClass;
                            self.partyId = p.partyId;
                            party[self.id] = self;
                        }
                        else party[p.id] = new TeraPlayer(p.id, p.name, p.level, p.playerClass, p.partyId);
                    }
                    if (onNewPartyList != null) onNewPartyList(this, new NewPartyListEventArgs(party.Values.ToArray()));
                    break;
                case OpCode2904.S_LEAVE_PARTY:
                    Logger.debug("S_LEAVE_PARTY");
                    party.Clear();
                    party[self.id] = self;
                    if (onNewPartyList != null) onNewPartyList(this, new NewPartyListEventArgs(party.Values.ToArray()));
                    break;
                case OpCode2904.S_LEAVE_PARTY_MEMBER:
                    var s_leave_member = (S_LEAVE_PARTY_MEMBER)PacketCreator.create(e.packet);
                    Logger.debug("S_LEAVE_PARTY_MEMBER {0}", s_leave_member.name);
                    ulong remId = 0;
                    foreach (var pair in party)
                        if (pair.Value.partyId == s_leave_member.partyId)
                        {
                            remId = pair.Key;
                            break;
                        }
                    if (remId != 0) party.Remove(remId);
                    if (onNewPartyList != null) onNewPartyList(this, new NewPartyListEventArgs(party.Values.ToArray()));
                    break;
                case OpCode2904.S_SPAWN_PROJECTILE:
                    Logger.debug("S_SPAWN_PROJECTILE");
                    var s_spawn_proj = (S_SPAWN_PROJECTILE)PacketCreator.create(teraPacketWithData);
                    projectiles[s_spawn_proj.id] = s_spawn_proj.idPlayer;
                    break;
                case OpCode2904.S_DESPAWN_PROJECTILE:
                    Logger.debug("S_DESPAWN_PROJECTILE");
                    var s_despawn_proj = (S_DESPAWN_PROJECTILE)PacketCreator.create(teraPacketWithData);
                    if (projectiles.ContainsKey(s_despawn_proj.id))
                        projectiles.Remove(s_despawn_proj.id);
                    break;
                case OpCode2904.S_SPAWN_NPC:
                    Logger.debug("S_SPAWN_NPC");
                    var s_spawn_npc = (S_SPAWN_NPC)PacketCreator.create(e.packet);
                    if (party.ContainsKey(s_spawn_npc.parentId))
                        npcs.Add(s_spawn_npc.id, s_spawn_npc.parentId);
                    break;
                case OpCode2904.S_DESPAWN_NPC:
                    Logger.debug("S_DESPAWN_NPC");
                    var s_despawn_npc = (S_DESPAWN_NPC)PacketCreator.create(e.packet);
                    if (npcs.ContainsKey(s_despawn_proj.id))
                        npcs.Remove(s_despawn_proj.id);
                    break;
                case OpCode2904.S_EACH_SKILL_RESULT:
                    {
                        var skill = (S_EACH_SKILL_RESULT)PacketCreator.create(e.packet);
                        /*
             * Проверяем если есть такой игрок с ай ди, то делаем что нужно и выходим
             * Проверяем если есть такой ловушк с ай ди, то ищем НПС или игрока
             * Проверяем если находим НПС ищем игрока
             */
                        TeraPlayer p;
                        ulong projectile; ulong npc;
                        if (projectiles.TryGetValue(skill.idWho, out projectile))
                        {
                            if (npcs.TryGetValue(projectile, out npc))
                            {
                                if (party.TryGetValue(npc, out p)) { }
                                //p.makeSkill(damage, type);
                            }
                            else
                            {
                                if (party.TryGetValue(projectile, out p)) { }
                                //p.makeSkill(damage, type);
                            }
                        }
                        else
                        {
                            if (npcs.TryGetValue(skill.idWho, out npc))
                            {
                                if (party.TryGetValue(npc, out p)) { }
                                //p.makeSkill(damage, type);
                            }
                            else
                            {
                                if (party.TryGetValue(skill.idWho, out p)) { }
                                //p.makeSkill(damage, type);
                            }
                        }

                        if (party.TryGetValue(skill.idTarget, out p)) { }
                        //p.takeSkill(damage, type);
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

        public TeraPlayer getPlayerById(ulong id)
        {
            return self;
        }


        public TeraPlayer getPlayerSelf()
        {
            return self;
        }
    }
}
