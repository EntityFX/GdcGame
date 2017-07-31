using System.Linq;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.EngineTestApplication
{
    using EntityFX.Gdcame.Contract.MainServer.Incrementors;

    using EntityFX.Gdcame.Kernel.Contract.Incrementors;
    using EntityFX.Gdcame.Kernel.Contract.Items;

    public class FundsDriverContractMapper : IMapper<Item, EntityFX.Gdcame.Contract.MainServer.Items.Item>
    {
        private readonly IMapper<IncrementorBase, Incrementor> _incrementorContractMapper;

        public FundsDriverContractMapper(IMapper<IncrementorBase, Incrementor> incrementorContractMapper)
        {
            _incrementorContractMapper = incrementorContractMapper;
        }

        public EntityFX.Gdcame.Contract.MainServer.Items.Item Map(Item source,
            EntityFX.Gdcame.Contract.MainServer.Items.Item destination)
        {
            var destinationIncrementors = source.Incrementors.Select(sourceIncrementor => _incrementorContractMapper.Map(sourceIncrementor)).ToArray();
            return new EntityFX.Gdcame.Contract.MainServer.Items.Item
            {
                Id = source.Id,
                Bought = source.Bought,
                Incrementors = destinationIncrementors,
                InflationPercent = source.InflationPercent,
                Name = source.Name,
                UnlockValue = source.UnlockBalance,
                Price = source.Price
            };
        }
    }

}