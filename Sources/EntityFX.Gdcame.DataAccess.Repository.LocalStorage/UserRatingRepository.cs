using EntityFX.Gdcame.DataAccess.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.UserRating;

namespace EntityFX.Gdcame.DataAccess.Repository.LocalStorage
{
    public class UserRatingRepository : IUserRatingRepository
    {
        public RatingStatistics[] GetAllUserRatings(GetAllUsersRatingsCriterion findAllUsersRatingsCriterion)
        {
            throw new NotImplementedException();
        }
    }
}
