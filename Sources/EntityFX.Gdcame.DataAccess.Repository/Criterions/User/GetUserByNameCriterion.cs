using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

namespace EntityFX.Gdcame.DataAccess.Repository.Criterions.User
{
    public class GetUserByNameCriterion  : ICriterion
    {
        public string Name { get; set; }

        public GetUserByNameCriterion(string name)
        {
            Name = name;
        }
    }
}