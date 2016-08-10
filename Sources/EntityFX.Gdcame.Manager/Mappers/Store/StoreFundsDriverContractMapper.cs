using System.Linq;
using EntityFX.Gdcame.Common.Contract.Incrementors;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.GameEngine.Contract.Funds;
using EntityFX.Gdcame.GameEngine.Contract.Incrementors;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.Mappers
{
    public class StoreFundsDriverContractMapper : IMapper<FundsDriver, StoreFundsDriver>
    {
        private readonly IMapper<IncrementorBase, StoreIncrementor> _incrementorContractMapper;
        private readonly IMapper<CustomRuleInfo, StoreCustomRuleInfo> _customRuleInfoMapper;

        public StoreFundsDriverContractMapper(IMapper<IncrementorBase, StoreIncrementor> incrementorContractMapper, IMapper<CustomRuleInfo, StoreCustomRuleInfo> customRuleInfoMapper)
        {
            _incrementorContractMapper = incrementorContractMapper;
            _customRuleInfoMapper = customRuleInfoMapper;
        }

        public StoreFundsDriver Map(FundsDriver source, StoreFundsDriver destination)
        {
            var destinationIncrementors = source.Incrementors.Values.Select(
                sourceIncrementor => _incrementorContractMapper.Map(sourceIncrementor));
            var customRuleInfo = source.CustomRuleInfo != null ? _customRuleInfoMapper.Map(source.CustomRuleInfo) : null;
            return new StoreFundsDriver()
            {
                Id = source.Id,
                BuyCount = source.BuyCount,
                Incrementors = destinationIncrementors.ToArray(),
                Value = source.CurrentValue,
                CustomRule = customRuleInfo
            };
        }
    }
}