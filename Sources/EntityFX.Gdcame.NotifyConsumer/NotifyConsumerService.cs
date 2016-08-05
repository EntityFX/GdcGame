using System;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.NotifyConsumer.Contract;
using Microsoft.AspNet.SignalR;

namespace EntityFX.Gdcame.NotifyConsumer
{
    
    public class NotifyConsumerService : INotifyConsumerService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IMapper<GameData, GameDataModel> _gameDataModelMapper;
        private readonly IHubContext _hubContext;

        public NotifyConsumerService(ILogger logger, IMapper<GameData, GameDataModel> gameDataModelMapper, IHubContext hubContext)
        {
            _logger = logger;
            _gameDataModelMapper = gameDataModelMapper;
            _hubContext = hubContext;
        }

        public void PushGameData(UserContext userContext, GameData gameData)
        {
            _logger.Trace("{0}.PushGameData: Data receieved for userId: {1}, userName: {2}", GetType().FullName, userContext.UserId, userContext.UserName);
            var gameDataModel = _gameDataModelMapper.Map(gameData);
            _hubContext.Clients.Group(userContext.UserName).GetGameData(gameDataModel);
        }

        public void Dispose()
        {

        }
    }
}