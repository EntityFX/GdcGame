using System.Linq;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.MainServer.Mappers
{
    using EntityFX.Gdcame.Contract.MainServer.Incrementors;
    using EntityFX.Gdcame.Kernel.Contract.Incrementors;
    using EntityFX.Gdcame.Kernel.Contract.Items;

    public class FundsDriverContractMapper : IMapper<Item, Gdcame.Contract.MainServer.Items.Item>
    {
        private readonly IMapper<CustomRuleInfo, Gdcame.Contract.MainServer.Items.CustomRuleInfo> _customRuleInfoMapper;
        private readonly IMapper<IncrementorBase, Incrementor> _incrementorContractMapper;

        public FundsDriverContractMapper(IMapper<IncrementorBase, Incrementor> incrementorContractMapper,
            IMapper<CustomRuleInfo, Gdcame.Contract.MainServer.Items.CustomRuleInfo> customRuleInfoMapper)
        {
            _incrementorContractMapper = incrementorContractMapper;
            _customRuleInfoMapper = customRuleInfoMapper;
        }

        public Gdcame.Contract.MainServer.Items.Item Map(Item source, Gdcame.Contract.MainServer.Items.Item destination)
        {
            var destinationIncrementors = source.Incrementors.Select(sourceIncrementor => _incrementorContractMapper.Map(sourceIncrementor)).ToArray();
            var customRuleInfo = source.CustomRuleInfo != null ? _customRuleInfoMapper.Map(source.CustomRuleInfo) : null;
            if (customRuleInfo != null) customRuleInfo.FundsDriverId = source.Id;
            return new Gdcame.Contract.MainServer.Items.Item
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