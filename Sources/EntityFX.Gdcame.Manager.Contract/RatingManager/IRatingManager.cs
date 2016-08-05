using System.ServiceModel;
using EntityFX.Gdcame.Common.Contract.UserRating;

namespace EntityFX.Gdcame.Manager.Contract.RatingManager
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
