using EntityFX.Gdcame.DataAccess.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.Counters;

namespace EntityFX.Gdcame.DataAccess.Repository.Mongo
{
    class CountersRepository : ICountersRepository
    {
        public CounterBase[] FindAll(GetAllCountersCriterion criterion)
        {
            throw new NotImplementedException();
        }
    }
}
