using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using Microsoft.AspNet.SignalR;

namespace EntityFX.Gdcame.NotifyConsumer.Signalr
{
    [Authorize]
    public class GameDataHub : Hub
    {
        private readonly ILogger _logger;
        private readonly IMapper<GameData, GameDataModel> _gameDataModelMapper;

        public GameDataHub(ILogger logger)
        {
            _logger = logger;
            _logger.Info("{0} instantiated", GetType().FullName);
        }

        public override Task OnConnected()
        {
            _logger.Trace("Hub OnConnected {0} : {1}\n", Context.ConnectionId, Context.User.Identity.Name);
            Groups.Add(Context.ConnectionId, Context.User.Identity.Name);
            return (base.OnConnected());
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            _logger.Trace("Hub OnDisconnected {0}\n", Context.ConnectionId);
            Groups.Remove(Context.ConnectionId, Context.User.Identity.Name);
            return (base.OnDisconnected(stopCalled));
        }
    }
}
