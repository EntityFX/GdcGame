using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Utils.WebApiClient;
using EntityFX.Gdcame.Utils.WebApiClient.Auth;

namespace EntityFX.Gdcame.Presentation.WebApiConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {

            var p = new PasswordAuthProvider(new Uri("http://localhost:9001"));
            var res = p.Login(new PasswordAuthRequest<PasswordAuthData>()
            {
                RequestData = new PasswordAuthData() {Password = "!Biohazard1989", Usename = "EntityFX"}
            }).Result;

            var gameApi = new GameApiClient(res);
            var stepResult = gameApi.PerformManualStepAsync().Result;
            var gameResult = gameApi.GetGameDataAsync().Result;
            var gameCountersResult = gameApi.GetCountersAsync().Result;
            var buyResult = gameApi.BuyFundDriverAsync(1).Result;
        }
    }
}
