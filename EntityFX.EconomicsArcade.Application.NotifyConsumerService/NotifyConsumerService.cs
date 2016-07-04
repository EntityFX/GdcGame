using System;
using System.Net;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.NotifyConsumerService;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Presentation.Models;
using Microsoft.AspNet.SignalR.Client;

namespace EntityFX.EconomicsArcade.Application.NotifyConsumerService
{
    public class NotifyConsumerService : INotifyConsumerService, IDisposable
    {
        private readonly ILogger _logger;

        public NotifyConsumerService(ILogger logger)
        {

            _logger = logger;
        }

        public void PushGameData(int userName, GameData gameData)
        {
            var hubConnection = new HubConnection("http://localhost:50689/")
            {
                Credentials = CredentialCache.DefaultNetworkCredentials
            };
            var hubProxy = hubConnection.CreateHubProxy("GameDataHub");

            hubConnection.Start().Wait();
            _logger.Trace("{0}.PushGameData: Data receieved for {1}", GetType().FullName, userName);
            hubProxy.Invoke("GetGameData", gameData);
        }

        public void Dispose()
        {
           
        }
    }
}