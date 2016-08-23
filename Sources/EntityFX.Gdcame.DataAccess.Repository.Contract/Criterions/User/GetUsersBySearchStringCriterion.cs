using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.User
{
    public class GetUsersBySearchStringCriterion : ICriterion
    {
        public GetUsersBySearchStringCriterion(string searchString)
        {
            SearchString = searchString;
        }

        public string SearchString { get; set; }
    }
}