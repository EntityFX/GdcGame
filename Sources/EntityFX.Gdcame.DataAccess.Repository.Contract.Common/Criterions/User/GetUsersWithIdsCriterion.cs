namespace EntityFX.Gdcame.DataAccess.Repository.Contract.Common.Criterions.User
{
    using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

    public class GetUsersWithIdsCriterion : ICriterion
    {
        public string[] UsersIds { get; set; }
    }
}