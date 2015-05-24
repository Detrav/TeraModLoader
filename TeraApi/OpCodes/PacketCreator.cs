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
            }
            currentVersion = version;
            return currentVersion;
        }

        public static TeraPacket create(TeraPacketWithData packet)
        {
            switch (currentVersion)
            {
                case OpCodeVersion.P2805:
                    Type p;
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
            }
            return null;
        }
        public static System.Collections.IEnumerable getOpCodes()
        {
            switch (currentVersion)
            {
                case OpCodeVersion.P2805:
                    return Enum.GetValues(typeof(OpCode2805)).Cast<OpCode2805>();
            }
            return null;
        }
    }
}