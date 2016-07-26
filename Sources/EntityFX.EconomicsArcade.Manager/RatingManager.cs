using System.Collections.Generic;
using System.Linq;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.Contract.Manager;
using EntityFX.EconomicsArcade.Contract.Manager.RatingManager;
using Microsoft.Practices.ObjectBuilder2;

namespace EntityFX.EconomicsArcade.Manager
{
    public class RatingManager : IRatingManager
    {
        private readonly GameSessions _gameSessions;
        private readonly IUserDataAccessService _userDataAccess;
        private readonly IGameDataRetrieveDataAccessService _gameDataRetrieveDataAccessService;

        public RatingManager(GameSessions gameSessions, IUserDataAccessService userDataAccess,
            IGameDataRetrieveDataAccessService gameDataRetrieveDataAccessService)
        {
            _gameSessions = gameSessions;
            _userDataAccess = userDataAccess;
            _gameDataRetrieveDataAccessService = gameDataRetrieveDataAccessService;
        }

        public UserRating[] GetUsersRatingByCount(int count)
        {
            var usersRatings = _gameDataRetrieveDataAccessService.GetUserRatings();
            usersRatings.ForEach(_ => _.Status = _gameSessions.GetGameSessionStatus(_.UserName));
            //foreach (var userRating in usersRatings)
            //{
            //    userRating.Status = _gameSessions.GetGameSessionStatus(userRating.UserName);
            //}
            return usersRatings.OrderByDescending(_ => _.GdcPoints).ThenByDescending(_ => _.TotalFunds).Take(count).ToArray();
            //var users = _userDataAccess.FindAll();
            ////var usersRatings = (from user in users
            ////                    let gameData = _gameDataRetrieveDataAccessService.GetGameData(user.Id)
            ////                    select new UserRating()
            ////                    {
            ////                        GdcPoints = gameData.Counters.Counters[0].Value,
            ////                        ManualStepsCount = gameData.ManualStepsCount,
            ////                        TotalFunds = gameData.Counters.TotalFunds,
            ////                        UserName = user.Email
            ////                    });
            //var usersRatings = new List<UserRating>();
            //foreach (var user in users)
            //{
            //    var gameData = _gameDataRetrieveDataAccessService.GetGameData(user.Id);
            //    usersRatings.Add(new UserRating()
            //    {
            //        GdcPoints = gameData.Counters.Counters[0].Value,
            //        ManualStepsCount = gameData.ManualStepsCount,
            //        TotalFunds = gameData.Counters.TotalFunds,
            //        UserName = user.Email, Status = _gameSessions.GetGameSessionStatus(user.Email)
            //    });
            //}
            //return usersRatings.OrderByDescending(_ => _.GdcPoints).ThenByDescending(_ => _.TotalFunds).Take(count).ToArray();
        }

        public UserRating FindUserRatingByUserName(string userName)
        {
            var user = _userDataAccess.FindByName(userName);
            var gameData = _gameDataRetrieveDataAccessService.GetGameData(user.Id);
            return new UserRating()
            {
                GdcPoints = gameData.Counters.Counters[0].Value,
                ManualStepsCount = gameData.ManualStepsCount,
                TotalFunds = gameData.Counters.TotalFunds,
                UserName = user.Email
            };
        }
        public UserRating[] FindUserRatingByUserNameAndAroundUsers(string userName, int count)
        {
            var users = _userDataAccess.FindAll();
            var usersRatings = (from user in users
                                let gameData = _gameDataRetrieveDataAccessService.GetGameData(user.Id)
                                select new UserRating()
                                {
                                    GdcPoints = gameData.Counters.Counters[0].Value,
                                    ManualStepsCount = gameData.ManualStepsCount,
                                    TotalFunds = gameData.Counters.TotalFunds,
                                    UserName = user.Email
                                });

            return usersRatings.OrderBy(_ => _.GdcPoints).Take(count).ToArray();
        }
    }
}