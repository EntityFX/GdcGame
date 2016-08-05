using System;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.Infrastructure.Service.Bases;

namespace EntityFX.Gdcame.Utils.ClientProxy.DataAccess
{
    public class GameDataRetrieveDataAccessClient<TInfrastructureProxy> : IGameDataRetrieveDataAccessService
        where TInfrastructureProxy : InfrastructureProxy<IGameDataRetrieveDataAccessService>, new()
    {
        private readonly Uri _endpoint;// =
        //"net.tcp://localhost:8777/EntityFX.EconomicsArcade.DataAccess/EntityFX.EconomicsArcade.Contract.DataAccess.GameData.IGameDataRetrieveDataAccessService";

        public GameDataRetrieveDataAccessClient(string endpoint)
        {
            _endpoint = new Uri(endpoint);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public GameData GetGameData(int userId)
        {
            GameData result;
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpoint);
                result = channel.GetGameData(userId);
                proxy.CloseChannel();
            }
            return result;
        }

        public UserRating[] GetUserRatings()
        {
            UserRating[] result;
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpoint);
                result = channel.GetUserRatings();
                proxy.CloseChannel();
            }
            return result;
        }
    }
}