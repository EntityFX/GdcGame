using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Contract.Manager.RatingManager;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserRating;
using EntityFX.EconomicsArcade.Infrastructure.Repository;

namespace EntityFX.EconomicsArcade.DataAccess.Repository
{
    public interface IUserRatingRepository
    {
        UserRating[] GetAllUserRatings(GetAllUsersRatingsCriterion findAllUsersRatingsCriterion);
    }
}
