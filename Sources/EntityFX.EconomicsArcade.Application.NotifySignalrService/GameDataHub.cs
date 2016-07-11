using System.Diagnostics;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.NotifyConsumerService;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Presentation.Models;
using Microsoft.AspNet.SignalR;

namespace EntityFX.EconomicsArcade.Application.NotifySignalrService
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
