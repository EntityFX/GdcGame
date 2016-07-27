using System.ServiceModel;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Common.UserRating;

namespace EntityFX.EconomicsArcade.Contract.Manager.RatingManager
{
    [ServiceContract]
    public interface IRatingManager
    {
        [OperationContract]
        UserRating[] GetUsersRatingByCount(int count);

        [OperationContract]
        UserRating FindUserRatingByUserName(string userName);

        [OperationContract]
        UserRating[] FindUserRatingByUserNameAndAroundUsers(string userName, int count);
    }
}
