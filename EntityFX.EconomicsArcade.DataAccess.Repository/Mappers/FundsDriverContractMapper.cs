using System.Collections.Generic;
using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.Contract.Common.Incrementors;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Mappers
{
    public class FundsDriverContractMapper : IMapper<FundsDriverEntity, FundsDriver>
    {
        private readonly IMapper<IncrementorEntity, Incrementor> _incrementorContractMapper;

        public FundsDriverContractMapper(IMapper<IncrementorEntity, Incrementor> incrementorContractMapper)
        {
            _incrementorContractMapper = incrementorContractMapper;
        }

        public FundsDriver Map(FundsDriverEntity source, FundsDriver destination = null)
        {
            destination = destination ?? new FundsDriver();
            destination.BuyCount = 0;
            destination.Value = source.InitialValue;
            destination.UnlockValue = source.UnlockValue;
            destination.Name = source.Name;
            destination.Id = source.Id;
            destination.InflationPercent = source.InflationPercent;
            destination.Incrementors = new Dictionary<int, Incrementor>();
            foreach (var incrementor in source.Incrementors)
            {
                destination.Incrementors.Add(incrementor.CounterId ?? 0, _incrementorContractMapper.Map(incrementor));
            }
            return destination;
        }
    }
}