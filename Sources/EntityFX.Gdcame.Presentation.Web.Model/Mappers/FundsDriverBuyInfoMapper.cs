using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.GameManager;

namespace EntityFX.Gdcame.Presentation.Web.Model.Mappers
{
    public class FundsDriverBuyInfoMapper : IMapper<BuyFundDriverResult, BuyDriverModel>
    {
        private readonly IMapper<Cash, CashModel> _countersMapper;
        private readonly IMapper<Item, ItemModel> _fundsDriverMapper;

        public FundsDriverBuyInfoMapper(IMapper<Cash, CashModel> countersMapper,
            IMapper<Item, ItemModel> fundsDriverMapper)
        {
            _countersMapper = countersMapper;
            _fundsDriverMapper = fundsDriverMapper;
        }

        public BuyDriverModel Map(BuyFundDriverResult source, BuyDriverModel destination = null)
        {
            destination = destination ?? new BuyDriverModel();
            destination.ModifiedCountersInfo = _countersMapper.Map(source.ModifiedCash);
            destination.FundsDriverBuyInfo = _fundsDriverMapper.Map(source.ModifiedItem);
            return destination;
        }
    }
}