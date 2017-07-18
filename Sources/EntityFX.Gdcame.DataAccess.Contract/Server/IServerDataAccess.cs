using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.DataAccess.Contract.Server
{
    public interface IServerDataAccessService
    {
        Server[] GetServers();

        void AddServer(string server);

        void RemoveServer(string server);
    }
}
