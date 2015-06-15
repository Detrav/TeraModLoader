using Detrav.TeraApi.Core;
using Detrav.TeraApi.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.Interfaces
{
    public interface ITeraRepository
    {
        void AddorUpdatePartyPlayerById(ulong id, TeraPartyPlayer player);
        TeraPartyPlayer getPartyPlayerByPartyId(ulong partyId);
        TeraPartyPlayer getPartyPlayerById(ulong id);

        void removePartyPlayerByPartyId(ulong partyId);
        void removePartyPlayerById(ulong id);

        TeraPartyPlayer getSelf();
        void replaceSelf(TeraPartyPlayer player);

        TeraEntity getEntityById(ulong id);
        void replaceEntityById(ulong id, TeraEntity entity);
        void removeEntityById(ulong id);

        void clearDBParty();
        void clearDBEntities();
        
        
        NpcDataBase getNpcDataBaseByUlongId(ulong id);
        TeraEntity[] getEntities();
        TeraNpc[] getNpcs();
        TeraPlayer[] getPlayers();
        TeraPartyPlayer[] getParty();
    }
}
