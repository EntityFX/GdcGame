﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using EntityFX.Gdcame.Common.Contract.UserRating;

namespace EntityFX.Gdcame.DataAccess.Contract.Rating
{
    [ServiceContract]
    public interface ILocalNodeRatingDataAccess : IRatingDataAccess
    {
        [OperationContract]
        void PersistUsersRatingHistory(RatingHistory[] ratingHistory);

        [OperationContract]
        RatingHistory[] ReadHistoryWithUsersIds(string[] userslds, TimeSpan period);

        [OperationContract]
        void CleanOldHistory(TimeSpan period);

        [OperationContract]
        void CreateOrUpdateUsersRatingStatistics(RatingStatistics[] ratingStatistics);

    }
}
