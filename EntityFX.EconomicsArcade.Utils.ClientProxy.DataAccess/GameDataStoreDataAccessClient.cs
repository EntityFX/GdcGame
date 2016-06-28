using System;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.DataAccess
{
    public class GameDataStoreDataAccessClient : IGameDataStoreDataAccessService
    {
        private readonly Uri _endpoint;// = "net.msmq://localhost/private/EntityFX.EconomicsArcade.Contract.DataAccess.GameData.IGameDataStoreDataAccessService";

        public GameDataStoreDataAccessClient(string endpoint)
        {
            _endpoint = new Uri(endpoint);
        }

        public void StoreGameDataForUser(int userId, GameData gameData)
        {
            using (var proxy = new GameDataStoreDataAccessProxy())
            {
                var channel = proxy.CreateChannel(_endpoint);
                channel.StoreGameDataForUser(userId, gameData);
                proxy.CloseChannel();
            }
        }

    }
}