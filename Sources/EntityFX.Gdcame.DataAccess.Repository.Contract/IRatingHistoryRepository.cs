using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Contract.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract
{
    public interface IRatingHistoryRepository
    {
        void PersistUsersRatingHistory(RatingHistory[] usersRatingHistory);
        RatingHistory[] ReadHistoryWithUsersIds(string[] userslds, TimeSpan period);
        void CleanOldHistory(TimeSpan period);
    }
}
