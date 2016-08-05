using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserRating;

namespace EntityFX.Gdcame.DataAccess.Repository
{
    public interface IUserRatingRepository
    {
        UserRating[] GetAllUserRatings(GetAllUsersRatingsCriterion findAllUsersRatingsCriterion);
    }
}
