using System.Linq;
using EntityFX.Gdcame.DataAccess.Contract.GameData.Store;
using EntityFX.Gdcame.GameEngine.Contract.Incrementors;
using EntityFX.Gdcame.GameEngine.Contract.Items;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.Mappers.Store
{
    public class StoreFundsDriverContractMapper : IMapper<Item, StoredItem>
    {
        private readonly IMapper<IncrementorBase, StoredIncrementor> _incrementorContractMapper;
        private readonly IMapper<CustomRuleInfo, StoredCustomRuleInfo> _customRuleInfoMapper;

        public StoreFundsDriverContractMapper(IMapper<IncrementorBase, StoredIncrementor> incrementorContractMapper, IMapper<CustomRuleInfo, StoredCustomRuleInfo> customRuleInfoMapper)
        {
            _incrementorContractMapper = incrementorContractMapper;
            _customRuleInfoMapper = customRuleInfoMapper;
        }

        public StoredItem Map(Item source, StoredItem destination)
        {
            var destinationIncrementors = source.Incrementors.ToDictionary(_ => _.Key, _ => _.Value.Value);
            var customRuleInfo = source.CustomRuleInfo != null ? _customRuleInfoMapper.Map(source.CustomRuleInfo) : null;
            return new StoredItem()
            {
                Id = source.Id,
                BuyCount = source.BuyCount,
                Incrementors = destinationIncrementors,
                Value = source.CurrentValue,
                CustomRule = customRuleInfo
            };
        }
    }
}