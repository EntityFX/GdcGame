using System.Collections.Generic;
using EntityFX.Gdcame.Contract.MainServer.Store;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.MainServer.Mappers.Store
{
    using EntityFX.Gdcame.Kernel.Contract.Incrementors;
    using EntityFX.Gdcame.Kernel.Contract.Items;

    public class StoreFundsDriverContractMapper : IMapper<Item, StoredItem>
    {
        private readonly IMapper<CustomRuleInfo, StoredCustomRuleInfo> _customRuleInfoMapper;
        private readonly IMapper<IncrementorBase, StoredIncrementor> _incrementorContractMapper;

        public StoreFundsDriverContractMapper(IMapper<IncrementorBase, StoredIncrementor> incrementorContractMapper,
            IMapper<CustomRuleInfo, StoredCustomRuleInfo> customRuleInfoMapper)
        {
            _incrementorContractMapper = incrementorContractMapper;
            _customRuleInfoMapper = customRuleInfoMapper;
        }

        public StoredItem Map(Item source, StoredItem destination)
        {
            var destinationIncrementors = new Dictionary<int, int>();
            for (int i = 0; i < source.Incrementors.Length; i++)
            {
                destinationIncrementors.Add(i, source.Incrementors[i].Value);
            }

            var customRuleInfo = source.CustomRuleInfo != null ? _customRuleInfoMapper.Map(source.CustomRuleInfo) : null;
            return new StoredItem
            {
                Id = source.Id,
                Bought = source.Bought,
                Incrementors = destinationIncrementors,
                Price = source.Price,
                CustomRule = customRuleInfo
            };
        }
    }
}