using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.DataAccess.Repository;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.Counters;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.FundsDriver;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserGameCounter;

namespace EntityFX.EconomicsArcade.DataAccess.Service
{
    public class GameDataRetrieveDataAccessService : IGameDataRetrieveDataAccessService
    {
        private readonly IFundsDriverRepository _fundsDriverRepository;
        private readonly ICountersRepository _countersRepository;
        private readonly IUserGameCounterRepository _userGameCounterRepository;

        public GameDataRetrieveDataAccessService(IFundsDriverRepository fundsDriverRepository, ICountersRepository countersRepository, IUserGameCounterRepository userGameCounterRepository)
        {
            _fundsDriverRepository = fundsDriverRepository;
            _countersRepository = countersRepository;
            _userGameCounterRepository = userGameCounterRepository;
        }

        public Contract.Common.GameData GetGameData(int userId)
        {
            var fundsDrivers = _fundsDriverRepository.FindAll(new GetAllFundsDriversCriterion());
            var counters = _countersRepository.FindAll(new GetAllCountersCriterion());
            var userGameCounters = _userGameCounterRepository.FindById(new GetUserGameCounterByIdCriterion(userId));
            return new Contract.Common.GameData()
            {
                FundsDrivers = fundsDrivers,
                Counters = new FundsCounters()
                {
                    Counters = counters,
                    CurrentFunds = userGameCounters != null ? userGameCounters.CurrentFunds : 100,
                    TotalFunds = userGameCounters != null ? userGameCounters.TotalFunds : 100
                },
                AutomaticStepsCount = userGameCounters != null ? userGameCounters.AutomaticStepsCount : 0,
                ManualStepsCount = userGameCounters != null ?  userGameCounters.ManualStepsCount : 0
            };
        }
    }
}
