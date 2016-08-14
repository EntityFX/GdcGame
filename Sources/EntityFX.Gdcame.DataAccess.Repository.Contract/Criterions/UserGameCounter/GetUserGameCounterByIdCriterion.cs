﻿using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.UserGameCounter
{
    public class GetUserGameCounterByIdCriterion : ICriterion
    {
        public int UserId { get; private set; }

        public GetUserGameCounterByIdCriterion(int userId)
        {
            UserId = userId;
        }
    }
}