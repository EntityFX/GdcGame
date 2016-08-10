using System.Collections.Generic;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.NotifyConsumer.Contract;
using Microsoft.AspNet.SignalR;

namespace EntityFX.Gdcame.NotifyConsumer.Signalr
{
    [Authorize]
    public class GameDataHub : Hub
    {
        private readonly ILogger _logger;
        private readonly IConnections _connections;
        private readonly IMapper<GameData, GameDataModel> _gameDataModelMapper;

        public GameDataHub(ILogger logger, IConnections connections)
        {
            _logger = logger;
            _connections = connections;
            _logger.Info("{0} instantiated", GetType().FullName);
        }

        public override Task OnConnected()
        {
            _logger.Trace("Hub OnConnected {0} : {1}\n", Context.ConnectionId, Context.User.Identity.Name);
            if (!_connections.ActiveConnections.ContainsKey(Context.User.Identity.Name))
            {
                _connections.ActiveConnections[Context.User.Identity.Name] = new List<string>();
            }
            _connections.ActiveConnections[Context.User.Identity.Name].Add(Context.ConnectionId);
            Groups.Add(Context.ConnectionId, Context.User.Identity.Name);
            return (base.OnConnected());
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            _logger.Trace("Hub OnDisconnected {0}\n", Context.ConnectionId);
            if (_connections.ActiveConnections.ContainsKey(Context.User.Identity.Name))
            {
                _connections.ActiveConnections[Context.User.Identity.Name].Remove(Context.ConnectionId);
                if (_connections.ActiveConnections[Context.User.Identity.Name].Count == 0)
                {
                    _connections.ActiveConnections.Remove(Context.User.Identity.Name);
                }
            }
            Groups.Remove(Context.ConnectionId, Context.User.Identity.Name);
            return (base.OnDisconnected(stopCalled));
        }
    }
}
