using EntityFX.EconomicsArcade.Infrastructure.Repository.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Infrastructure.Repository
{
    public interface IRepository<TDomain, TFindByIdCriterion, TFindAllCriterion>
        where TDomain : class
        where TFindByIdCriterion : ICriterion
        where TFindAllCriterion : ICriterion
    {

        int Create(TDomain user);

        void Update(TDomain user);

        void Delete(int id);

        TDomain FindById(TFindByIdCriterion findByIdCriterion);

        TDomain[] FindAll(TFindAllCriterion finalAllCriterion);
    }
}
