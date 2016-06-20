using System.Collections.Generic;
using System.Linq;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager.Incrementors;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.Manager.Mappers
{
    public class FundsDriverContractMapper : IMapper<FundsDriver, Contract.Manager.GameManager.Funds.FundsDriver>
    {
        private readonly IMapper<IncrementorBase, Incrementor> _incrementorContractMapper;

        public FundsDriverContractMapper(IMapper<IncrementorBase, Incrementor> incrementorContractMapper)
        {
            _incrementorContractMapper = incrementorContractMapper;
        }

        public Contract.Manager.GameManager.Funds.FundsDriver Map(FundsDriver source, Contract.Manager.GameManager.Funds.FundsDriver destination)
        {
            var destinationIncrementors = source.Incrementors.ToDictionary(
                sourceIncrementor => sourceIncrementor.Key, 
                sourceIncrementor => _incrementorContractMapper.Map(sourceIncrementor.Value));
            return new Contract.Manager.GameManager.Funds.FundsDriver()
            {
                BuyCount = source.BuyCount,
                Incrementors = destinationIncrementors,
                InflationPercent = source.InflationPercent,
                Name = source.Name,
                UnlockValue = source.UnlockValue,
                Value = source.UnlockValue
            };
        }
    }
}