using EntityFX.Gdcame.DataAccess.Contract.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.DataAccess.Repository.Contract;

namespace EntityFX.Gdcame.DataAccess.Service
{
    public class ServerDataAccessService : IServerDataAccessService
    {
        private readonly IServerRepository _serverRepository;

        public ServerDataAccessService(IServerRepository serverRepository)
        {
            _serverRepository = serverRepository;
        }

        public Server[] GetServers()
        {
            return _serverRepository.FindServers();
        }

        public void UpdateServers(string[] servers)
        {
            foreach (var server in servers)
            {
                _serverRepository.Create(server);
            }

        }

        public void RemoveServer(string server)
        {
            _serverRepository.Delete(server);
        }
    }
}
