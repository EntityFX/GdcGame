using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.Contract.Common.Incrementors;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Mappers
{
    public class FundsDriverContractMapper : IMapper<FundsDriverEntity, FundsDriver>
    {
        private readonly IMapper<IncrementorEntity, Incrementor> _incrementorContractMapper;
        
        public FundsDriverContractMapper(IMapper<IncrementorEntity, Incrementor> incrementorContractMapper)
        {
            _incrementorContractMapper = incrementorContractMapper;
        }
        
        public FundsDriver Map(FundsDriverEntity source, FundsDriver destionation = null)
        {
            destionation = destionation ?? new FundsDriver();
            destionation.BuyCount = 0;
            destionation.Value = source.InitialValue;
            destionation.UnlockValue = source.UnlockValue;
            destionation.Id = source.Id.ToString();
            destionation.InflationPercent = source.InflationPercent;
            destionation.Incrementors = new Dictionary<int, Incrementor>();
            foreach (var incrementor in source.Incrementors)
	        {
                destionation.Incrementors.Add(incrementor.CounterId, _incrementorContractMapper.Map(incrementor));
	        }
            return destionation;
        }
    }
}
