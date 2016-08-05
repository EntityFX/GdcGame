using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Contract.Funds;
using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.GameManager;

namespace EntityFX.Gdcame.Presentation.Web.Model.Mappers
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