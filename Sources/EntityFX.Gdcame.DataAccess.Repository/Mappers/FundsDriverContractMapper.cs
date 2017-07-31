namespace EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Mappers
{
    using EntityFX.Gdcame.Common.Contract.Incrementors;
    using EntityFX.Gdcame.Common.Contract.Items;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Entities;
    using EntityFX.Gdcame.Infrastructure.Common;

    public class FundsDriverContractMapper : IMapper<FundsDriverEntity, Item>
    {
        private readonly IMapper<CustomRuleEntity, CustomRule> _customRuleContractMapper;
        private readonly IMapper<IncrementorEntity, Incrementor> _incrementorContractMapper;

        public FundsDriverContractMapper(IMapper<IncrementorEntity, Incrementor> incrementorContractMapper,
            IMapper<CustomRuleEntity, CustomRule> customRuleContractMapper)
        {
            this._incrementorContractMapper = incrementorContractMapper;
            this._customRuleContractMapper = customRuleContractMapper;
        }

        public Item Map(FundsDriverEntity source, Item destination = null)
        {
            destination = destination ?? new Item();
            destination.Bought = 0;
            destination.InitialValue = source.InitialValue;
            destination.UnlockValue = source.UnlockValue;
            destination.Name = source.Name;
            destination.Id = source.Id;
            destination.InflationPercent = source.InflationPercent;
            destination.Picture = source.Picture;
            destination.CustomRuleInfo = source.CustomRule != null
                ? new CustomRuleInfo {CustomRuleId = source.CustomRule.Id}
                : null;
            destination.Incrementors = new Incrementor[source.Incrementors.Count];
            foreach (var incrementor in source.Incrementors)
            {
                destination.Incrementors[incrementor.Id] = incrementor.CounterId == null 
                    ? new Incrementor { IncrementorType = IncrementorTypeEnum.ValueIncrementor, Value = 0 } 
                : this._incrementorContractMapper.Map(incrementor);
            }
            return destination;
        }
    }
}