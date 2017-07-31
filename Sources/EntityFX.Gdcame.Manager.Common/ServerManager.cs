using System.Linq;

using EntityFX.Gdcame.Manager.Contract.Common.ServerManager;

namespace EntityFX.Gdcame.Manager.Common
{
    using EntityFX.Gdcame.DataAccess.Contract.Common.Server;

    public class ServerManager : IServerManager
    {
        private readonly IServerDataAccessService _serverDataAccessService;

        public ServerManager(IServerDataAccessService serverDataAccessService)
        {
            _serverDataAccessService = serverDataAccessService;
        }

        public string[] GetServers()
        {
            return _serverDataAccessService.GetServers().Select(s => s.Address).ToArray();
        }
    }
}
