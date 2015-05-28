using System;
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