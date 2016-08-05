using System.Linq;
using EntityFX.Gdcame.Common.Contract.Incrementors;
using EntityFX.Gdcame.GameEngine.Contract.Funds;
using EntityFX.Gdcame.GameEngine.Contract.Incrementors;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.Mappers
{
    public class FundsDriverContractMapper : IMapper<FundsDriver, Gdcame.Common.Contract.Funds.FundsDriver>
    {
        private readonly IMapper<IncrementorBase, Incrementor> _incrementorContractMapper;
        private readonly IMapper<CustomRuleInfo, Gdcame.Common.Contract.Funds.CustomRuleInfo> _customRuleInfoMapper;

        public FundsDriverContractMapper(IMapper<IncrementorBase, Incrementor> incrementorContractMapper, IMapper<CustomRuleInfo, Gdcame.Common.Contract.Funds.CustomRuleInfo> customRuleInfoMapper)
        {
            _incrementorContractMapper = incrementorContractMapper;
            _customRuleInfoMapper = customRuleInfoMapper;
        }

        public Gdcame.Common.Contract.Funds.FundsDriver Map(FundsDriver source, Gdcame.Common.Contract.Funds.FundsDriver destination)
        {
            var destinationIncrementors = source.Incrementors.ToDictionary(
                sourceIncrementor => sourceIncrementor.Key,
                sourceIncrementor => _incrementorContractMapper.Map(sourceIncrementor.Value));
            var customRuleInfo = source.CustomRuleInfo != null ? _customRuleInfoMapper.Map(source.CustomRuleInfo) : null;
            if (customRuleInfo != null) customRuleInfo.FundsDriverId = source.Id;
            return new Gdcame.Common.Contract.Funds.FundsDriver()
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