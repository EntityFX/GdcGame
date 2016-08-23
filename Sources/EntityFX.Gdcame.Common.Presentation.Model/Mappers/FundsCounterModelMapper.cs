using System.Collections.Generic;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Common.Presentation.Model.Mappers
{
    public class FundsCounterModelMapper : IMapper<Cash, FundsCounterModel>
    {
        private readonly IMapper<CounterBase, CounterModelBase> _counterModelBaseMapper;

        public FundsCounterModelMapper(IMapper<CounterBase, CounterModelBase> counterModelBaseMapper)
        {
            _counterModelBaseMapper = counterModelBaseMapper;
        }

        public FundsCounterModel Map(Cash source, FundsCounterModel destination = null)
        {
            destination = destination ?? new FundsCounterModel();
            destination.TotalFunds = source.TotalEarned;
            destination.CurrentFunds = source.CashOnHand;
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