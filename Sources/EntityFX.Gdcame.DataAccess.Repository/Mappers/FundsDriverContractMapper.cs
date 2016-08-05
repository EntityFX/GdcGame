using System.Collections.Generic;
using EntityFX.Gdcame.Common.Contract.Funds;
using EntityFX.Gdcame.Common.Contract.Incrementors;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.DataAccess.Repository.Mappers
{
    public class FundsDriverContractMapper : IMapper<FundsDriverEntity, FundsDriver>
    {
        private readonly IMapper<IncrementorEntity, Incrementor> _incrementorContractMapper;
        private readonly IMapper<CustomRuleEntity, CustomRule> _customRuleContractMapper;

        public FundsDriverContractMapper(IMapper<IncrementorEntity, Incrementor> incrementorContractMapper, IMapper<CustomRuleEntity, CustomRule> customRuleContractMapper)
        {
            _incrementorContractMapper = incrementorContractMapper;
            _customRuleContractMapper = customRuleContractMapper;
        }

        public FundsDriver Map(FundsDriverEntity source, FundsDriver destination = null)
        {
            destination = destination ?? new FundsDriver();
            destination.BuyCount = 0;
            destination.InitialValue = source.InitialValue;
            destination.UnlockValue = source.UnlockValue;
            destination.Name = source.Name;
            destination.Id = source.Id;
            destination.InflationPercent = source.InflationPercent;
            destination.Picture = source.Picture;
            destination.CustomRuleInfo = source.CustomRule != null ? new CustomRuleInfo() { CustomRuleId = source.CustomRule.Id} : null;
            destination.Incrementors = new Dictionary<int, Incrementor>();
            foreach (var incrementor in source.Incrementors)
            {
                destination.Incrementors.Add(incrementor.CounterId ?? 0, _incrementorContractMapper.Map(incrementor));
            }
            return destination;
        }
    }
}