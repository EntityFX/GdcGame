using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

namespace EntityFX.Gdcame.Infrastructure.Repository
{
    using System.Collections;
    using System.Collections.Generic;

    public interface IRepository<TDomain, in TFindByIdCriterion, in TFindAllCriterion>
        where TDomain : class
        where TFindByIdCriterion : ICriterion
        where TFindAllCriterion : ICriterion
    {
        int Create(TDomain user);

        void Update(TDomain user);

        void Delete(string id);

        TDomain FindById(TFindByIdCriterion findByIdCriterion);

        IEnumerable<TDomain> FindAll(TFindAllCriterion finalAllCriterion);
    }
}