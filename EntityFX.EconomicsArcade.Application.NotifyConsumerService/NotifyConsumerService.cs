using System;
using EntityFX.EconomicsArcade.Application.NotifySignalrService;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.NotifyConsumerService;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Presentation.Models;
using Microsoft.AspNet.SignalR;

namespace EntityFX.EconomicsArcade.Application.NotifyConsumerService
{
    public class NotifyConsumerService : INotifyConsumerService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IHubContext _context;
        private readonly IMapper<GameData, GameDataModel> _gameDataModelMapper;

        public NotifyConsumerService(ILogger logger, IMapper<GameData, GameDataModel> gameDataModelMapper)
        {
            _logger = logger;
            _context = GlobalHost.ConnectionManager.GetHubContext<GameDataHub>();
            _gameDataModelMapper = gameDataModelMapper;
        }

        public void PushGameData(UserContext userContext, GameData gameData)
        {
            _logger.Trace("{0}.PushGameData: Data receieved for userId: {1}, userName: {2}", GetType().FullName, userContext.UserId, userContext.UserName);
            var gameDataModel = _gameDataModelMapper.Map(gameData, null);
            _context.Clients.All.GetGameData(gameDataModel);
        }

        public void Dispose()
        {

        }
    }
}