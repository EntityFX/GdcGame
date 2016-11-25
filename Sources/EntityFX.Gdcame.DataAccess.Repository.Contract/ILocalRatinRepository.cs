using EntityFX.Gdcame.DataAccess.Contract.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract
{
    public interface ILocalRatingRepository: IRatingRepository
    {
        void PersistRatingHistory(RatingHistory ratingHistory);
        void ReadHistoryWithUsersIds(string[] userslds, TimeSpan period);
        void CleanOldHistory(TimeSpan period);
    }
}
