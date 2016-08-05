using System.Linq;
using EntityFX.Gdcame.Common.Contract.Incrementors;
using EntityFX.Gdcame.GameEngine.Contract.Funds;
using EntityFX.Gdcame.GameEngine.Contract.Incrementors;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFx.Gdcame.TestApplication
{
    public class FundsDriverContractMapper : IMapper<FundsDriver, EntityFX.Gdcame.Common.Contract.Funds.FundsDriver>
    {
        private readonly IMapper<IncrementorBase, Incrementor> _incrementorContractMapper;

        public FundsDriverContractMapper(IMapper<IncrementorBase, Incrementor> incrementorContractMapper)
        {
            _incrementorContractMapper = incrementorContractMapper;
        }

        public EntityFX.Gdcame.Common.Contract.Funds.FundsDriver Map(FundsDriver source, EntityFX.Gdcame.Common.Contract.Funds.FundsDriver destination)
        {
            var destinationIncrementors = source.Incrementors.ToDictionary(
                sourceIncrementor => sourceIncrementor.Key, 
                sourceIncrementor => _incrementorContractMapper.Map(sourceIncrementor.Value));
            return new EntityFX.Gdcame.Common.Contract.Funds.FundsDriver()
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