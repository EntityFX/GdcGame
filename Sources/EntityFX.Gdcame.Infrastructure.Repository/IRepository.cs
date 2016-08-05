using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

namespace EntityFX.EconomicsArcade.Infrastructure.Repository
{
    public interface IRepository<TDomain, in TFindByIdCriterion, in TFindAllCriterion>
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
