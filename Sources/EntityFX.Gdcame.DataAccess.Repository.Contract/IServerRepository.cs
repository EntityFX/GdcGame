using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.DataAccess.Contract.Server;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract
{
    public interface IServerRepository
    {
        Server[] FindServers();

        void Create(string server);

        void Delete(string server);
    }
}
