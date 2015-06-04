using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Detrav.TeraApi.Events;
using Detrav.TeraApi.Events.Self;
using Detrav.TeraApi.Events.Player;
using Detrav.TeraApi.Events.Party;
using Detrav.TeraApi.Core;

namespace Detrav.TeraApi.Interfaces
{
    public interface ITeraClient
    {
        event OnPacketArrival onPacketArrival;
        event OnTick onTick;
        event OnLogin onLogin;
        event OnUpdateCharacteristic onUpdateCharacteristic;
        event OnNewPartyList onNewPartyList;
        event OnLeaveFromParty onLeaveFromParty;
        TeraPlayer getPlayerById(ulong id);
        /*
        Думаю тут сделаю больше событий, может обработанные события??
        event OnLogin(ссылка на своего игрока)
        event OnPartyList(TeraPlayer[])
        event OnPartyLeave()
        event OnPartyMemberLeave()
        event OnPartyUpdateHp()
        event OnPartyUpdateMp()
        event OnPartyUpdateStamina()
        .......
        */

        void unLoad();
        void doEvents();
        void PacketArrival(TeraPacketWithData teraPacketWithData);
    }
}
