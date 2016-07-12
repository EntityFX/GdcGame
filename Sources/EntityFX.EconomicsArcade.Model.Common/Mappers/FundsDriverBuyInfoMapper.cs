using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Model.Common.Model;

namespace EntityFX.EconomicsArcade.Model.Common.Mappers
{
    public class FundsDriverBuyInfoMapper : IMapper<BuyFundDriverResult, BuyDriverModel>
    {
        private readonly IMapper<FundsCounters, FundsCounterModel> _countersMapper;
        private readonly IMapper<FundsDriver, FundsDriverModel> _fundsDriverMapper;

        public FundsDriverBuyInfoMapper(IMapper<FundsCounters, FundsCounterModel> countersMapper, IMapper<FundsDriver, FundsDriverModel> fundsDriverMapper)
        {
            _countersMapper = countersMapper;
            _fundsDriverMapper = fundsDriverMapper;
        }

        public BuyDriverModel Map(BuyFundDriverResult source, BuyDriverModel destination = null)
        {
            destination = destination ?? new BuyDriverModel();
            destination.ModifiedCountersInfo = _countersMapper.Map(source.ModifiedFundsCounters);
            destination.FundsDriverBuyInfo = _fundsDriverMapper.Map(source.ModifiedFundsDriver);
            return destination;
        }
    }
}