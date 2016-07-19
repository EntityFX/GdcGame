using System;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Manager.RatingManager;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.Manager
{
    public class RatingManagerClient : IRatingManager
    {
        private readonly Uri _endpointAddress;

        public RatingManagerClient(string endpointAddress)
        {
            _endpointAddress = new Uri(endpointAddress);
        }

        public UserRating[] GetUsersRatingByCount(int count)
        {
            using (var proxy = new RatingManagerProxy())
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
            using (var proxy = new RatingManagerProxy())
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
            using (var proxy = new RatingManagerProxy())
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
