using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.Presentation.Models
{
    public class FundsCounterModelMapper : IMapper<FundsCounters, FundsCounterModel>
    {
        private readonly IMapper<CounterBase, CounterModelBase> _counterModelBaseMapper;

        public FundsCounterModelMapper()
        {
            _counterModelBaseMapper = new CounterModelMapper();
        }

        public FundsCounterModel Map(FundsCounters source, FundsCounterModel destination = null)
        {
            destination = destination ?? new FundsCounterModel();
            destination.TotalFunds = source.TotalFunds;
            destination.CurrentFunds = source.CurrentFunds;
            var counters = new List<CounterModelBase>();
            foreach (var counter in source.Counters)
            {
                counters.Add(_counterModelBaseMapper.Map(counter, null));
            }
            destination.Counters = counters.ToArray();
            return destination;
        }
    }
}
