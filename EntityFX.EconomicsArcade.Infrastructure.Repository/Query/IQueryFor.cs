using EntityFX.EconomicsArcade.Infrastructure.Repository.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Infrastructure.Repository.Query
{
    public interface IQueryFor<out T>
    {
        T With<TCriterion>(TCriterion criterion) where TCriterion : ICriterion;
    }
}
