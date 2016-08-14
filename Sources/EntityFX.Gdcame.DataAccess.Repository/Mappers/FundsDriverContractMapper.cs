using System.Collections.Generic;
using EntityFX.Gdcame.Common.Contract.Incrementors;
using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.DataAccess.Repository.Ef.Mappers
{
    public class FundsDriverContractMapper : IMapper<FundsDriverEntity, Item>
    {
        private readonly IMapper<IncrementorEntity, Incrementor> _incrementorContractMapper;
        private readonly IMapper<CustomRuleEntity, CustomRule> _customRuleContractMapper;

        public FundsDriverContractMapper(IMapper<IncrementorEntity, Incrementor> incrementorContractMapper, IMapper<CustomRuleEntity, CustomRule> customRuleContractMapper)
        {
            _incrementorContractMapper = incrementorContractMapper;
            _customRuleContractMapper = customRuleContractMapper;
        }

        public Item Map(FundsDriverEntity source, Item destination = null)
        {
            destination = destination ?? new Item();
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
            for (var i = 0; i < 3; i++)
            {
                if (!destination.Incrementors.ContainsKey(i))
                {
                    destination.Incrementors.Add(i, new Incrementor() { IncrementorType = IncrementorTypeEnum.ValueIncrementor, Value = 0});
                }
            }
            return destination;
        }
    }
}