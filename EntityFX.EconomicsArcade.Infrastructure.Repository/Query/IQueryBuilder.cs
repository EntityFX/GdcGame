using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Infrastructure.Repository.Query
{
    public interface IQueryBuilder
    {
        IQueryFor<TResult> For<TResult>();
    }
}
