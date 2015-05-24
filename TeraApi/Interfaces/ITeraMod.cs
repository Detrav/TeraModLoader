using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.Interfaces
{
    public interface ITeraMod
    {
        void load(ITeraClient parent);
        void unLoad();
        void changeVisible();
        void configManager(IConfigManager configManager);
        void show();
        void hide();
    }
}
