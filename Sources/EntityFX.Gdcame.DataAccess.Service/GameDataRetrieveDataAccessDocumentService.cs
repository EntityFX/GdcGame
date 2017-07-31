namespace EntityFX.Gdcame.DataAccess.Service.MainServer
{
    using System.Linq;

    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.Contract.MainServer;
    using EntityFX.Gdcame.Contract.MainServer.Counters;
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData;
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData.Store;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.UserGameSnapshot;
    using EntityFX.Gdcame.Infrastructure.Common;

    public class GameDataRetrieveDataAccessDocumentService : GameDataRetrieveDataAccessBase,
                                                             IGameDataRetrieveDataAccessService
    {
        private readonly IUserGameSnapshotRepository _userGameSnapshotRepository;

        public GameDataRetrieveDataAccessDocumentService(ICache cache, IUserGameSnapshotRepository userGameSnapshotRepository,
                                                         GameRepositoryFacade gameRepositoryFacade)
            : base(cache, gameRepositoryFacade)
        {
            this._userGameSnapshotRepository = userGameSnapshotRepository;
        }

        public GameData GetGameData(string userId)
        {
            var userGameData = this._userGameSnapshotRepository.FindByUserId(new GetUserGameSnapshotByIdCriterion(userId));
            var originalItems = this.GetFundDrivers();
            var originalCounters = this.GetCounters();
            var originalCustomRules = this.GetCsutomRules();
            var cash = new Cash
                           {
                               Counters = originalCounters,
                               OnHand = 100,
                               Total = 100
                           };
            if (userGameData != null)
            {
                var originalItemsDisct = originalItems.ToDictionary(_ => _.Id, _ => _);
                var originalCountersDisct = originalCounters.ToDictionary(_ => _.Id, _ => _);
                var originalCustomRulesDisct = originalCustomRules.ToDictionary(_ => _.Id, _ => _);
                foreach (var storedItem in userGameData.Items)
                {
                    var originalItem = originalItemsDisct[storedItem.Id];
                    originalItem.Bought = storedItem.Bought;
                    originalItem.Price = storedItem.Price;
                    foreach (var storedIncrementor in storedItem.Incrementors)
                    {
                        originalItem.Incrementors[storedIncrementor.Key].Value = storedIncrementor.Value;
                    }
                }

                for (var index = 0; index < userGameData.Cash.Counters.Length; index++)
                {
                    var storedCounter = userGameData.Cash.Counters[index];
                    var originalCounter = originalCountersDisct[storedCounter.Id];
                    originalCounter.Value = storedCounter.Value;
                    switch (originalCounter.Type)
                    {
                        case 1:
                            var storedGenericCounter = (StoredGenericCounter) storedCounter;
                            var originalGenericCounter = (GenericCounter) originalCounter;
                            originalGenericCounter.BonusPercentage = storedGenericCounter.BonusPercent;
                            originalGenericCounter.CurrentSteps = storedGenericCounter.CurrentSteps;
                            originalGenericCounter.Inflation = storedGenericCounter.Inflation;
                            originalGenericCounter.SubValue = storedGenericCounter.Value;
                            break;
                        case 2:
                            var storedDelayedCounter = (StoredDelayedCounter) storedCounter;
                            var originalDelayedCounter = (DelayedCounter) originalCounter;
                            originalDelayedCounter.SecondsRemaining = storedDelayedCounter.SecondsRemaining;
                            originalCounter.Value = storedDelayedCounter.DelayedValue;
                            break;
                    }
                }
                cash.OnHand = userGameData.Cash.Balance;
                cash.Total = userGameData.Cash.TotalEarned;
            }
            return new GameData
                       {
                           Items = originalItems,
                           Cash = cash,
                           CustomRules = originalCustomRules,
                           AutomatedStepsCount = 0,
                           ManualStepsCount = 0
                       };
        }

        public RatingStatistics[] GetUserRatings()
        {
            return new RatingStatistics[0];
        }
    }
}