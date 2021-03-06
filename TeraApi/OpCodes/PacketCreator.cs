﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.OpCodes
{
    public static class PacketCreator
    {
        private static Dictionary<ushort, Type> creator;
        private static OpCodeVersion currentVersion;
        public static OpCodeVersion setVersion(OpCodeVersion version)
        {
            if (creator != null)
                return currentVersion;
            switch(version)
            {
                case OpCodeVersion.P2805:
                    Logger.debug("Created OpCodeVersion.P2805");
                    creator = new Dictionary<ushort, Type>();
                    creator.Add((ushort)OpCode2805.C_CHECK_VERSION, typeof(P2805.C_CHECK_VERSION));
                    creator.Add((ushort)OpCode2805.S_DESPAWN_PROJECTILE, typeof(P2805.S_DESPAWN_PROJECTILE));
                    creator.Add((ushort)OpCode2805.S_DESPAWN_USER, typeof(P2805.S_DESPAWN_USER));
                    creator.Add((ushort)OpCode2805.S_EACH_SKILL_RESULT, typeof(P2805.S_EACH_SKILL_RESULT));
                    creator.Add((ushort)OpCode2805.S_LOGIN, typeof(P2805.S_LOGIN));
                    creator.Add((ushort)OpCode2805.S_SPAWN_PROJECTILE, typeof(P2805.S_SPAWN_PROJECTILE));
                    creator.Add((ushort)OpCode2805.S_SPAWN_USER, typeof(P2805.S_SPAWN_USER));
                    creator.Add((ushort)OpCode2805.S_USER_STATUS, typeof(P2805.S_USER_STATUS));
                    break;
                case OpCodeVersion.P2904:
                    Logger.debug("Created OpCodeVersion.P2904");
                    creator = new Dictionary<ushort, Type>();
                    creator.Add((ushort)OpCode2904.C_CHECK_VERSION,typeof(P2904.C_CHECK_VERSION));
                    creator.Add((ushort)OpCode2904.S_DESPAWN_PROJECTILE, typeof(P2904.S_DESPAWN_PROJECTILE));
                    creator.Add((ushort)OpCode2904.S_EACH_SKILL_RESULT, typeof(P2904.S_EACH_SKILL_RESULT));
                    creator.Add((ushort)OpCode2904.S_LEAVE_PARTY, typeof(P2904.S_LEAVE_PARTY));
                    creator.Add((ushort)OpCode2904.S_LEAVE_PARTY_MEMBER, typeof(P2904.S_LEAVE_PARTY_MEMBER));
                    creator.Add((ushort)OpCode2904.S_LOGIN, typeof(P2904.S_LOGIN));
                    creator.Add((ushort)OpCode2904.S_PARTY_MEMBER_LIST, typeof(P2904.S_PARTY_MEMBER_LIST));
                    creator.Add((ushort)OpCode2904.S_SPAWN_PROJECTILE, typeof(P2904.S_SPAWN_PROJECTILE));
                    creator.Add((ushort)OpCode2904.S_DESPAWN_USER, typeof(P2904.S_DESPAWN_USER));
                    creator.Add((ushort)OpCode2904.S_SPAWN_USER, typeof(P2904.S_SPAWN_USER));
                    creator.Add((ushort)OpCode2904.S_USER_STATUS, typeof(P2904.S_USER_STATUS));

                    creator.Add((ushort)OpCode2904.S_SPAWN_NPC, typeof(P2904.S_SPAWN_NPC));
                    creator.Add((ushort)OpCode2904.S_DESPAWN_NPC, typeof(P2904.S_DESPAWN_NPC));

                    creator.Add((ushort)OpCode2904.S_PARTY_MEMBER_CHANGE_STAMINA, typeof(P2904.S_PARTY_MEMBER_CHANGE_STAMINA));
                    creator.Add((ushort)OpCode2904.S_PARTY_MEMBER_CHANGE_HP, typeof(P2904.S_PARTY_MEMBER_CHANGE_HP));
                    creator.Add((ushort)OpCode2904.S_PARTY_MEMBER_CHANGE_MP, typeof(P2904.S_PARTY_MEMBER_CHANGE_MP));
                    creator.Add((ushort)OpCode2904.S_NPC_STATUS, typeof(P2904.S_NPC_STATUS));

                    creator.Add((ushort)OpCode2904.S_START_USER_PROJECTILE, typeof(P2904.S_START_USER_PROJECTILE));
                    creator.Add((ushort)OpCode2904.S_END_USER_PROJECTILE, typeof(P2904.S_END_USER_PROJECTILE));
                    break;
            }
            currentVersion = version;
            return currentVersion;
        }

        public static TeraPacket create(TeraPacketWithData packet)
        {
            Type p;
            switch (currentVersion)
            {
                case OpCodeVersion.P2805:
                    if (creator.TryGetValue(packet.opCode, out p))
                        return (TeraPacket)Activator.CreateInstance(p, packet);
                    return new TeraPacket(packet);
                case OpCodeVersion.P2904:
                    if (creator.TryGetValue(packet.opCode, out p))
                        return (TeraPacket)Activator.CreateInstance(p, packet);
                    return new TeraPacket(packet);
            }
            return null;
        }

        public static object getOpCode(ushort opCode)
        {
            switch (currentVersion)
            {
                case OpCodeVersion.P2805:
                    return (OpCode2805)opCode;
                case OpCodeVersion.P2904:
                    return (OpCode2904)opCode;
            }
            return null;
        }
        public static OpCodeVersion getCurrentVersion()
        {
            return currentVersion;
        }


        public static System.Collections.IEnumerable getOpCodes()
        {
            switch (currentVersion)
            {
                case OpCodeVersion.P2805:
                    return Enum.GetValues(typeof(OpCode2805)).Cast<OpCode2805>();
                case OpCodeVersion.P2904:
                    return Enum.GetValues(typeof(OpCode2904)).Cast<OpCode2904>();
            }
            return null;
        }

        public static object parseOpCode(string opCode)
        {
            switch (currentVersion)
            {
                case OpCodeVersion.P2805:
                    OpCode2805 p2805;
                    if (Enum.TryParse<OpCode2805>(opCode, out p2805))
                        return p2805;
                    else
                        return null;
                case OpCodeVersion.P2904:
                    OpCode2904 p2904;
                    if (Enum.TryParse<OpCode2904>(opCode, out p2904))
                        return p2904;
                    else
                        return null;
            }
            return null;
        }

        static public System.Collections.IEnumerable getVerions()
        {
            return Enum.GetValues(typeof(OpCodeVersion)).Cast<OpCodeVersion>();
        }
    }
}