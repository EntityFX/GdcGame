namespace EntityFX.Gdcame.DataAccess.Service.MainServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.Rating;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common.Criterions.User;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.RatingHistory;

    class LocalNodeRatingDataAccess : ILocalNodeRatingDataAccess
    {
        private readonly ILocalRatingStatisticsRepository _ratingStatisticsRepository;
        private readonly IRatingHistoryRepository _ratingHistoryRepository;

        private readonly IUserRepository userRepository;

        public LocalNodeRatingDataAccess(
            ILocalRatingStatisticsRepository ratingStatisticsRepository, IRatingHistoryRepository ratingHistoryRepository, IUserRepository userRepository)
        {
            this._ratingStatisticsRepository = ratingStatisticsRepository;
            this._ratingHistoryRepository = ratingHistoryRepository;
            this.userRepository = userRepository;
        }

        public void PersistUsersRatingHistory(RatingHistory[] ratingHistory)
        {
            this._ratingHistoryRepository.PersistUsersRatingHistory(ratingHistory);
        }

        public RatingHistory[] ReadHistoryWithUsersIds(string[] userslds, TimeSpan period)
        {
            return this._ratingHistoryRepository.ReadHistoryWithUsersIds(new GetUsersRatingHistoryCriterion() { UsersIds = userslds, Period = period });
        }

        public void CleanOldHistory(TimeSpan period)
        {
            this._ratingHistoryRepository.CleanOldHistory(period);
        }

        public void CreateOrUpdateUsersRatingStatistics(RatingStatistics[] ratingStatistics)
        {
            this._ratingStatisticsRepository.CreateOrUpdateUsersRatingStatistics(ratingStatistics);
        }

        public Task<TopRatingStatistics> GetRaiting(int top = 500)
        {
            return Task.Run(
                () =>
                    {
                        var topRatingStatistics = this._ratingStatisticsRepository.GetRaiting(top);
                        var counter = topRatingStatistics.ManualStepsCount.Day.FirstOrDefault();
                        if (counter != null && counter.Login == null)
                        {
                            var counters =
                                (new Dictionary<string, TopStatisticsCounter>()).Concat(
                                    topRatingStatistics.ManualStepsCount.Day.ToDictionary(k => k.UserId, e => e))
                                .Concat(topRatingStatistics.ManualStepsCount.Week.ToDictionary(k => k.UserId, e => e))
                                .Concat(topRatingStatistics.ManualStepsCount.Total.ToDictionary(k => k.UserId, e => e))
                                .Concat(topRatingStatistics.TotalEarned.Day.ToDictionary(k => k.UserId, e => e))
                                .Concat(topRatingStatistics.TotalEarned.Week.ToDictionary(k => k.UserId, e => e))
                                .Concat(topRatingStatistics.TotalEarned.Total.ToDictionary(k => k.UserId, e => e))
                                .Concat(topRatingStatistics.RootCounter.Day.ToDictionary(k => k.UserId, e => e))
                                .Concat(topRatingStatistics.RootCounter.Week.ToDictionary(k => k.UserId, e => e))
                                .Concat(topRatingStatistics.RootCounter.Total.ToDictionary(k => k.UserId, e => e))
                                .ToArray();

                            
                            var uniqueUserIds = counters.Select(i => i.Key).Distinct().ToArray();


                            var users =
                                this.userRepository.FindWithIds(
                                        new GetUsersWithIdsCriterion() { UsersIds = uniqueUserIds })
                                    .ToDictionary(k => k.Id, v => v);

                            foreach (var c in counters)
                            {
                                c.Value.Login = users.ContainsKey(c.Key) ? users[c.Key].Login : null;
                            }


                        }     


                        return topRatingStatistics;
                    });
        }

    }

}

