using System;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.DataAccess
{
    public class GameDataStoreDataAccessClient : IGameDataStoreDataAccessService
    {
        private const string Endpoint = "net.msmq://localhost/private/EntityFX.EconomicsArcade.Contract.DataAccess.GameData.IGameDataStoreDataAccessService";

        public void StoreGameDataForUser(int userId, GameData gameData)
        {
            using (var proxy = new GameDataStoreDataAccessProxy())
            {
                var channel = proxy.CreateChannel(new Uri(Endpoint));
                channel.StoreGameDataForUser(userId, gameData);
                proxy.CloseChannel();
            }
        }

    }
}