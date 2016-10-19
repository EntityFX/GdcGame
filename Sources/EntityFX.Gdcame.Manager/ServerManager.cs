using EntityFX.Gdcame.DataAccess.Contract.Server;
using EntityFX.Gdcame.Manager.Contract.ServerManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Manager
{
    public class ServerManager : IServerManager
    {
        private readonly IServerDataAccessService _serverDataAccessService;

        public ServerManager(IServerDataAccessService serverDataAccessService)
        {
            _serverDataAccessService = serverDataAccessService;
        }

        public string[] GetServers()
        {
            return _serverDataAccessService.GetServers();
        }
    }
}
