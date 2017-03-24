using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.RatingHistory
{
    public class GetUsersRatingHistoryCriterion : ICriterion
    {
        public string[] UsersIds { get; set; }

        public TimeSpan Period { get; set; }
    }
}
