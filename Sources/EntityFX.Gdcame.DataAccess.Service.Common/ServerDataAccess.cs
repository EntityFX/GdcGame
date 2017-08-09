namespace EntityFX.Gdcame.DataAccess.Service.MainServer
{
    using EntityFX.Gdcame.DataAccess.Contract.Common.Server;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common;

    public class ServerDataAccessService : IServerDataAccessService
    {
        private readonly IServerRepository _serverRepository;

        public ServerDataAccessService(IServerRepository serverRepository)
        {
            this._serverRepository = serverRepository;
        }

        public Server[] GetServers()
        {
            return this._serverRepository.FindServers();
        }

        public void UpdateServers(string[] servers)
        {
            foreach (var server in servers)
            {
                this._serverRepository.Create(server);
            }

        }

        public void RemoveServer(string server)
        {
            this._serverRepository.Delete(server);
        }
    }
}
