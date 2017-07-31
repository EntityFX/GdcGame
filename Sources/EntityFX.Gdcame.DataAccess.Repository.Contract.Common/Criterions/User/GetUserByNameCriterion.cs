namespace EntityFX.Gdcame.DataAccess.Repository.Contract.Common.Criterions.User
{
    using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

    public class GetUserByNameCriterion : ICriterion
    {
        public GetUserByNameCriterion(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}