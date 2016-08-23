using System;
using System.ServiceModel.Channels;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Contract.GameData.Store;
using EntityFX.Gdcame.Infrastructure.Service.Bases;

namespace EntityFX.Gdcame.Utils.ClientProxy.DataAccess
{
    public class GameDataStoreDataAccessClient<TInfrastructureProxy> : IGameDataStoreDataAccessService
        where TInfrastructureProxy : IInfrastructureProxy<IGameDataStoreDataAccessService, Binding>, new()
    {
        private readonly Uri _endpoint;
            // = "net.msmq://localhost/private/EntityFX.EconomicsArcade.Contract.DataAccess.GameData.IGameDataStoreDataAccessService";

        public GameDataStoreDataAccessClient(string endpoint)
        {
            _endpoint = new Uri(endpoint);
        }

        public void StoreGameDataForUser(string userId, StoredGameData gameData)
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpoint);
                channel.StoreGameDataForUser(userId, gameData);
                proxy.CloseChannel();
            }
        }
    }
}