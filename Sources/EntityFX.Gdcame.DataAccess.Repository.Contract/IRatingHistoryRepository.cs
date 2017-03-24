using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Contract.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.RatingHistory;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract
{
    public interface IRatingHistoryRepository
    {
        void PersistUsersRatingHistory(RatingHistory[] usersRatingHistory);
        RatingHistory[] ReadHistoryWithUsersIds(GetUsersRatingHistoryCriterion usersRatingHistoryCriterion);
        void CleanOldHistory(TimeSpan period);
    }
}
