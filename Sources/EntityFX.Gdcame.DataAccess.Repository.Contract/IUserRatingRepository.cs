﻿using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.UserRating;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract
{
    public interface IUserRatingRepository
    {
        RatingStatistics[] GetAllUserRatings(GetAllUsersRatingsCriterion findAllUsersRatingsCriterion);
    }
}