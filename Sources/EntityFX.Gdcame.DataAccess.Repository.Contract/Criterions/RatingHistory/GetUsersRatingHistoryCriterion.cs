namespace EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.RatingHistory
{
    using System;

    using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

    public class GetUsersRatingHistoryCriterion : ICriterion
    {
        public string[] UsersIds { get; set; }

        public TimeSpan Period { get; set; }
    }
}
