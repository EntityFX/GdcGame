namespace EntityFX.Gdcame.DataAccess.Repository.Contract.Common.Criterions.User
{
    using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

    public class GetUsersBySearchStringCriterion : ICriterion
    {
        public GetUsersBySearchStringCriterion(string searchString)
        {
            this.SearchString = searchString;
        }

        public string SearchString { get; set; }
    }
}