using System.Linq;
using EntityFX.Gdcame.Common.Contract.Incrementors;
using EntityFX.Gdcame.GameEngine.Contract.Incrementors;
using EntityFX.Gdcame.GameEngine.Contract.Items;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.EngineTestApplication
{
    public class FundsDriverContractMapper : IMapper<Item, EntityFX.Gdcame.Common.Contract.Items.Item>
    {
        private readonly IMapper<IncrementorBase, Incrementor> _incrementorContractMapper;

        public FundsDriverContractMapper(IMapper<IncrementorBase, Incrementor> incrementorContractMapper)
        {
            _incrementorContractMapper = incrementorContractMapper;
        }

        public EntityFX.Gdcame.Common.Contract.Items.Item Map(Item source,
            EntityFX.Gdcame.Common.Contract.Items.Item destination)
        {
            var destinationIncrementors = source.Incrementors.ToDictionary(
                sourceIncrementor => sourceIncrementor.Key,
                sourceIncrementor => _incrementorContractMapper.Map(sourceIncrementor.Value));
            return new EntityFX.Gdcame.Common.Contract.Items.Item
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