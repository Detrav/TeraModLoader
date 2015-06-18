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
    internal partial class TeraClient : ITeraGame
    {
        public event OnPacketArrival onPacketArrival;
        public event OnTick onTick;
        public event OnLogin onLogin;
        public event OnUpdateCharacteristic onUpdateCharacteristic;
        public event OnNewPartyList onNewPartyList;
        public event OnSkillResult onSkillResult;




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
                        clearDBParty();
                        clearDBEntities();
                        var s_login = (S_LOGIN)PacketCreator.create(teraPacketWithData);
                        Logger.debug(s_login.ToString());
                        replaceSelf(new TeraPartyPlayer(s_login.id,ulong.MaxValue, s_login.name));
                        if (onLogin != null) onLogin(this, new LoginEventArgs(getSelf()));
                        if (onNewPartyList != null) onNewPartyList(this, new NewPartyListEventArgs(getParty()));
                    }
                    break;
                case OpCode2904.S_PARTY_MEMBER_LIST:
                    {
                        var s_party_list = (S_PARTY_MEMBER_LIST)PacketCreator.create(teraPacketWithData);
                        Logger.debug(s_party_list.ToString());
                        //clearParty();
                        foreach (var p in s_party_list.players)
                        {
                            AddorUpdatePartyPlayerById(p.id, new TeraPartyPlayer(p.id, p.partyId, p.name, p.level, p.playerClass));
                        }
                        if (onNewPartyList != null) onNewPartyList(this, new NewPartyListEventArgs(getParty()));
                    }
                    break;
                case OpCode2904.S_LEAVE_PARTY:
                    {
                        Logger.debug("S_LEAVE_PARTY");
                        clearDBParty();
                        if (onNewPartyList != null) onNewPartyList(this, new NewPartyListEventArgs(getParty()));
                        break;
                    }
                case OpCode2904.S_LEAVE_PARTY_MEMBER:
                    {
                        var s_leave_member = (S_LEAVE_PARTY_MEMBER)PacketCreator.create(teraPacketWithData);
                        Logger.debug(s_leave_member.ToString());
                        removePartyPlayerByPartyId(s_leave_member.partyId);
                        if (onNewPartyList != null) onNewPartyList(this, new NewPartyListEventArgs(getParty()));
                    }
                    break;
                case OpCode2904.S_SPAWN_PROJECTILE:
                    {
                        var s_spawn_proj = (S_SPAWN_PROJECTILE)PacketCreator.create(teraPacketWithData);
                        Logger.debug(s_spawn_proj.ToString());
                        replaceEntityById(s_spawn_proj.id, new TeraEntity(s_spawn_proj.id, getEntityById(s_spawn_proj.idPlayer)));
                    }
                    break;
                case OpCode2904.S_DESPAWN_PROJECTILE:
                    {
                        var s_despawn_proj = (S_DESPAWN_PROJECTILE)PacketCreator.create(teraPacketWithData);
                        Logger.debug(s_despawn_proj.ToString());
                        removeEntityById(s_despawn_proj.id);
                    }
                    break;
                case OpCode2904.S_SPAWN_NPC:
                    {
                        var s_spawn_npc = (S_SPAWN_NPC)PacketCreator.create(teraPacketWithData);
                        Logger.debug(s_spawn_npc.ToString());
                        TeraEntity te = getEntityById(s_spawn_npc.parentId);
                        replaceEntityById(s_spawn_npc.id,new TeraNpc(s_spawn_npc.id, cacheManager.getNpc(s_spawn_npc.header, s_spawn_npc.template), te));
                    }
                    break;
                case OpCode2904.S_DESPAWN_NPC:
                    var s_despawn_npc = (S_DESPAWN_NPC)PacketCreator.create(teraPacketWithData);
                    Logger.debug(s_despawn_npc.ToString());
                    removeEntityById(s_despawn_npc.id);
                    break;
                case OpCode2904.S_EACH_SKILL_RESULT:
                    if (onSkillResult != null)
                    {
                        var skill = (S_EACH_SKILL_RESULT)PacketCreator.create(teraPacketWithData);
                        Logger.debug(skill.ToString());
                        try
                        {
                            TeraEntity who = getEntityById(skill.idWho);
                            TeraEntity target = getEntityById(skill.idTarget);
                            if (onSkillResult != null)
                                onSkillResult(this, new SkillResultEventArgs(skill.damage, skill.dType, who, target, skill.crit,teraPacketWithData.time));
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
                        TeraEntity te = getEntityById(npc_status.npcId);
                        if (te != null)
                        {
                            if (te is TeraNpc)
                            {
                                TeraEntity target = getEntityById(npc_status.playerId);
                                TeraNpc npc = te as TeraNpc;
                                npc.target = target;
                                replaceEntityById(npc.id, npc);
                            }
                        }
                    }
                    break;
                case OpCode2904.S_SPAWN_USER:
                    {
                        var s_spawn_user = PacketCreator.create(teraPacketWithData) as S_SPAWN_USER;
                        Logger.debug(s_spawn_user.ToString());
                        replaceEntityById(s_spawn_user.id, new TeraPlayer(s_spawn_user.id, s_spawn_user.name));
                    }
                    break;
                case OpCode2904.S_DESPAWN_USER:
                    {
                        var s_despawn_user = PacketCreator.create(teraPacketWithData) as S_DESPAWN_USER;
                        Logger.debug(s_despawn_user.ToString());
                        removeEntityById(s_despawn_user.id);
                    }
                    break;
                case OpCode2904.S_START_USER_PROJECTILE:
                    {
                        var s_spawn_proj = (S_START_USER_PROJECTILE)PacketCreator.create(teraPacketWithData);
                        Logger.debug(s_spawn_proj.ToString());
                        replaceEntityById(s_spawn_proj.id, new TeraEntity(s_spawn_proj.id, getEntityById(s_spawn_proj.idPlayer)));
                    }
                    break;
                case OpCode2904.S_END_USER_PROJECTILE:
                    {
                        var s_despawn_proj = (S_END_USER_PROJECTILE)PacketCreator.create(teraPacketWithData);
                        Logger.debug(s_despawn_proj.ToString());
                        removeEntityById(s_despawn_proj.id);
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
    }
}