using System;
using System.Configuration;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Presentation.Models;
using EntityFX.EconomicsArcade.Utils.ServiceStarter.NotifyConsumer;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;

namespace EntityFX.EconomicsArcade.Utils.ServiceHost.NotifyConsumer.ConsoleApp
{

    class Program
    {
        static void Main(string[] args)
        {
            var containerBootstrapper = new ContainerBootstrapper();
            var ss = new NotifyConsumerStarter(
                containerBootstrapper, ConfigurationManager.AppSettings["NotifyConsumerEndpoint_BaseAddressServiceUrl"]
                );
            ss.StartService();

            string url = "http://localhost:8080";
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }
    }
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
    //public class GameDataHub : Hub
    //{
    //    private readonly IMapper<GameData, GameDataModel> _gameDataModelMapper;

    //    public GameDataHub(IMapper<GameData, GameDataModel> gameDataModelMapper)
    //    {
    //        _gameDataModelMapper = gameDataModelMapper;
    //    }

    //    public void GetGameData(GameData gameData)
    //    {
    //        var gameDataModel = _gameDataModelMapper.Map(gameData, null);
    //        Clients.All.getGameData(gameDataModel);
    //    }
    //}
}