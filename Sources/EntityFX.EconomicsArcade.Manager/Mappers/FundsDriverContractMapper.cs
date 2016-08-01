using System.Linq;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Contract.Common.Incrementors;
using EntityFX.EconomicsArcade.Contract.Game.Funds;
using EntityFX.EconomicsArcade.Contract.Game.Incrementors;

namespace EntityFX.EconomicsArcade.Manager.Mappers
{
    public class FundsDriverContractMapper : IMapper<FundsDriver, Contract.Common.Funds.FundsDriver>
    {
        private readonly IMapper<IncrementorBase, Incrementor> _incrementorContractMapper;
        private readonly IMapper<CustomRuleInfo, Contract.Common.Funds.CustomRuleInfo> _customRuleInfoMapper;

        public FundsDriverContractMapper(IMapper<IncrementorBase, Incrementor> incrementorContractMapper, IMapper<CustomRuleInfo, Contract.Common.Funds.CustomRuleInfo> customRuleInfoMapper)
        {
            _incrementorContractMapper = incrementorContractMapper;
            _customRuleInfoMapper = customRuleInfoMapper;
        }

        public Contract.Common.Funds.FundsDriver Map(FundsDriver source, Contract.Common.Funds.FundsDriver destination)
        {
            var destinationIncrementors = source.Incrementors.ToDictionary(
                sourceIncrementor => sourceIncrementor.Key,
                sourceIncrementor => _incrementorContractMapper.Map(sourceIncrementor.Value));
            var customRuleInfo = source.CustomRuleInfo != null ? _customRuleInfoMapper.Map(source.CustomRuleInfo) : null;
            if (customRuleInfo != null) customRuleInfo.FundsDriverId = source.Id;
            return new Contract.Common.Funds.FundsDriver()
            {
                Id = source.Id,
                BuyCount = source.BuyCount,
                Incrementors = destinationIncrementors,
                InflationPercent = source.InflationPercent,
                Name = source.Name,
                UnlockValue = source.UnlockValue,
                Value = source.CurrentValue,
                CustomRuleInfo = customRuleInfo
            };
        }
    }
}