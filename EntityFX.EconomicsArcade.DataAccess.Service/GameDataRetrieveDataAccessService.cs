using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.DataAccess.Repository;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.Counters;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.FundsDriver;

namespace EntityFX.EconomicsArcade.DataAccess.Service
{
    public class GameDataRetrieveDataAccessService : IGameDataRetrieveDataAccessService
    {
        private readonly IFundsDriverRepository _fundsDriverRepository;
        private readonly ICountersRepository _countersRepository;

        public GameDataRetrieveDataAccessService(IFundsDriverRepository fundsDriverRepository, ICountersRepository countersRepository)
        {
            _fundsDriverRepository = fundsDriverRepository;
            _countersRepository = countersRepository;
        }

        public Contract.Common.GameData GetGameData()
        {
            var fundsDrivers = _fundsDriverRepository.FindAll(new GetAllFundsDriversCriterion());
            var counters = _countersRepository.FindAll(new GetAllCountersCriterion());
            return new Contract.Common.GameData()
            {
                FundsDrivers = fundsDrivers,
                Counters = new FundsCounters()
                {
                    Counters = counters,
                    CurrentFunds = 100,
                    TotalFunds = 100
                }
            };
        }
    }
}
