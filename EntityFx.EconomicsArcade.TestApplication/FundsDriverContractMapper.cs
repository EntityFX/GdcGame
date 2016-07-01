using System.Linq;
using EntityFX.EconomicsArcade.Contract.Common.Incrementors;
using EntityFX.EconomicsArcade.Contract.Game.Funds;
using EntityFX.EconomicsArcade.Contract.Game.Incrementors;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFx.EconomicsArcade.TestApplication
{
    public class FundsDriverContractMapper : IMapper<FundsDriver, EntityFX.EconomicsArcade.Contract.Common.Funds.FundsDriver>
    {
        private readonly IMapper<IncrementorBase, Incrementor> _incrementorContractMapper;

        public FundsDriverContractMapper(IMapper<IncrementorBase, Incrementor> incrementorContractMapper)
        {
            _incrementorContractMapper = incrementorContractMapper;
        }

        public EntityFX.EconomicsArcade.Contract.Common.Funds.FundsDriver Map(FundsDriver source, EntityFX.EconomicsArcade.Contract.Common.Funds.FundsDriver destination)
        {
            var destinationIncrementors = source.Incrementors.ToDictionary(
                sourceIncrementor => sourceIncrementor.Key, 
                sourceIncrementor => _incrementorContractMapper.Map(sourceIncrementor.Value));
            return new EntityFX.EconomicsArcade.Contract.Common.Funds.FundsDriver()
            {
                Id = source.Id,
                BuyCount = source.BuyCount,
                Incrementors = destinationIncrementors,
                InflationPercent = source.InflationPercent,
                Name = source.Name,
                UnlockValue = source.UnlockValue,
                Value = source.CurrentValue
            };
        }
    }
}