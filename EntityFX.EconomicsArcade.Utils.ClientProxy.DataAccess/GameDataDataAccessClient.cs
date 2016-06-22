using System;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.DataAccess
{
    public class GameDataDataAccessClient : IGameDataDataAccessService
    {
        private const string Endpoint =
            "net.tcp://localhost:8777/EntityFX.EconomicsArcade.DataAccess/EntityFX.EconomicsArcade.Contract.DataAccess.GameData.IGameDataDataAccessService";
        
        public GameData GetGameData()
        {
            GameData result;
            using (var proxy = new GameDataDataAccessProxy())
            {
                var channel = proxy.CreateChannel(new Uri(Endpoint));
                result = channel.GetGameData();
                proxy.CloseChannel();
            }
            return result;
        }
    }
}