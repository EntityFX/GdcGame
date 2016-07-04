using System;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.NotifyConsumerService;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Presentation.Models;
using Microsoft.AspNet.SignalR.Client;

namespace EntityFX.EconomicsArcade.Application.NotifyConsumerService
{
    public class NotifyConsumerService : INotifyConsumerService, IDisposable
    {
        private readonly HubConnection _hubConnection;
        private readonly IHubProxy _hubProxy;
        private readonly ILogger _logger;

        public NotifyConsumerService(ILogger logger)
        {
            _hubConnection = new HubConnection("http://localhost:50689/");
            _hubProxy = _hubConnection.CreateHubProxy("GameDataHub");

            _hubConnection.Start().Wait();
            _logger = logger;
        }

        public void PushGameData(int userName, GameData gameData)
        {
            _logger.Trace("{0}.PushGameData: Data receieved for {1}", GetType().FullName, userName);
            _hubProxy.Invoke("GetGameData", gameData);
        }

        public void Dispose()
        {
            _hubConnection.Dispose();
        }
    }
}