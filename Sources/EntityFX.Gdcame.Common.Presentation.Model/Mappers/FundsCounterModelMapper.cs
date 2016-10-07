using System.Collections.Generic;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Common.Application.Model.Mappers
{
    public class FundsCounterModelMapper : IMapper<Cash, CashModel>
    {
        private readonly IMapper<CounterBase, CounterModelBase> _counterModelBaseMapper;

        public FundsCounterModelMapper(IMapper<CounterBase, CounterModelBase> counterModelBaseMapper)
        {
            _counterModelBaseMapper = counterModelBaseMapper;
        }

        public CashModel Map(Cash source, CashModel destination = null)
        {
            destination = destination ?? new CashModel();
            destination.TotalEarned = source.Total;
            destination.OnHand = source.OnHand;
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