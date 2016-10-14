using System.Linq;
using EntityFX.Gdcame.Common.Contract.Incrementors;
using EntityFX.Gdcame.GameEngine.Contract.Incrementors;
using EntityFX.Gdcame.GameEngine.Contract.Items;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.Mappers
{
    public class FundsDriverContractMapper : IMapper<Item, Common.Contract.Items.Item>
    {
        private readonly IMapper<CustomRuleInfo, Common.Contract.Items.CustomRuleInfo> _customRuleInfoMapper;
        private readonly IMapper<IncrementorBase, Incrementor> _incrementorContractMapper;

        public FundsDriverContractMapper(IMapper<IncrementorBase, Incrementor> incrementorContractMapper,
            IMapper<CustomRuleInfo, Common.Contract.Items.CustomRuleInfo> customRuleInfoMapper)
        {
            _incrementorContractMapper = incrementorContractMapper;
            _customRuleInfoMapper = customRuleInfoMapper;
        }

        public Common.Contract.Items.Item Map(Item source, Common.Contract.Items.Item destination)
        {
            var destinationIncrementors = source.Incrementors.Select(sourceIncrementor => _incrementorContractMapper.Map(sourceIncrementor)).ToArray();
            var customRuleInfo = source.CustomRuleInfo != null ? _customRuleInfoMapper.Map(source.CustomRuleInfo) : null;
            if (customRuleInfo != null) customRuleInfo.FundsDriverId = source.Id;
            return new Common.Contract.Items.Item
            {
                Id = source.Id,
                Bought = source.Bought,
                Incrementors = destinationIncrementors,
                InflationPercent = source.InflationPercent,
                Name = source.Name,
                UnlockValue = source.UnlockBalance,
                Price = source.Price,
                CustomRuleInfo = customRuleInfo
            };
        }
    }
}