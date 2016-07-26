using System;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.Contract.Manager.RatingManager;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.DataAccess
{
    public class GameDataRetrieveDataAccessClient : IGameDataRetrieveDataAccessService
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
            using (var proxy = new GameDataRetrieveDataAccessProxy())
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
            using (var proxy = new GameDataRetrieveDataAccessProxy())
            {
                var channel = proxy.CreateChannel(_endpoint);
                result = channel.GetUserRatings();
                proxy.CloseChannel();
            }
            return result;
        }
    }
}