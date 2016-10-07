using System;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.NotifyConsumer.Contract;
using Microsoft.AspNet.SignalR;

namespace EntityFX.Gdcame.NotifyConsumer
{
    public class NotifyConsumerService : INotifyConsumerService, IDisposable
    {
        private readonly IConnections _connections;
        private readonly IMapper<GameData, GameDataModel> _gameDataModelMapper;
        private readonly Lazy<IHubContext> _hubContext;
        private readonly ILogger _logger;

        public NotifyConsumerService(ILogger logger, IMapper<GameData, GameDataModel> gameDataModelMapper,
            IHubContextAccessor hubContext, IConnections connections)
        {
            _logger = logger;
            _gameDataModelMapper = gameDataModelMapper;
            _hubContext = new Lazy<IHubContext>(hubContext.GetHubContext);
            _connections = connections;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposeManaged)
        {

        }

        public void PushGameData(UserContext userContext, GameData gameData)
        {
            _logger.Trace("{0}.PushGameData: Data receieved for userId: {1}, userName: {2}", GetType().FullName,
                userContext.UserId, userContext.UserName);
            if (_connections.ActiveConnections.ContainsKey(userContext.UserName))
            {
                var gameDataModel = _gameDataModelMapper.Map(gameData);
                _hubContext.Value.Clients.Group(userContext.UserName).GetGameData(gameDataModel);
            }
        }
    }
}