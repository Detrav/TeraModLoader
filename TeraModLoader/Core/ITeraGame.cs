using Detrav.TeraApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraModLoader.Core
{
    interface ITeraGame : ITeraClient, ITeraClientLoader, ITeraRepository
    {
    }
}
