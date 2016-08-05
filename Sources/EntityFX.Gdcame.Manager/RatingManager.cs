using System.Linq;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.Manager.Contract.RatingManager;
using Microsoft.Practices.ObjectBuilder2;

namespace EntityFX.Gdcame.Manager
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