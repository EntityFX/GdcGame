using System;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.Infrastructure.Service.Bases;

namespace EntityFX.Gdcame.Utils.ClientProxy.DataAccess
{
    public class GameDataStoreDataAccessClient<TInfrastructureProxy> : IGameDataStoreDataAccessService
                where TInfrastructureProxy : InfrastructureProxy<IGameDataStoreDataAccessService>, new()
    {
        private readonly Uri _endpoint;// = "net.msmq://localhost/private/EntityFX.EconomicsArcade.Contract.DataAccess.GameData.IGameDataStoreDataAccessService";

        public GameDataStoreDataAccessClient(string endpoint)
        {
            _endpoint = new Uri(endpoint);
        }

        public void StoreGameDataForUser(int userId, GameData gameData)
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