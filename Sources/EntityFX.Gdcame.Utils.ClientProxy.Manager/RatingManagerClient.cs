using System;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.Infrastructure.Service.Bases;
using EntityFX.Gdcame.Manager.Contract.RatingManager;

namespace EntityFX.Gdcame.Utils.ClientProxy.Manager
{
    public class RatingManagerClient<TInfrastructureProxy> : IRatingManager
                        where TInfrastructureProxy : InfrastructureProxy<IRatingManager>, new()
    {
        private readonly Uri _endpointAddress;

        public RatingManagerClient(string endpointAddress)
        {
            _endpointAddress = new Uri(endpointAddress);
        }

        public UserRating[] GetUsersRatingByCount(int count)
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                var res = channel.GetUsersRatingByCount(count);
                proxy.CloseChannel();
                return res;
            }
        }

        public UserRating FindUserRatingByUserName(string userName)
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                var res = channel.FindUserRatingByUserName(userName);
                proxy.CloseChannel();
                return res;
            }
        }

        public UserRating[] FindUserRatingByUserNameAndAroundUsers(string userName, int count)
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                var res = channel.FindUserRatingByUserNameAndAroundUsers(userName, count);
                proxy.CloseChannel();
                return res;
            }
        }
    }
}
