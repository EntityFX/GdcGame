using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.User
{
    public class GetUserByNameCriterion : ICriterion
    {
        public GetUserByNameCriterion(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}