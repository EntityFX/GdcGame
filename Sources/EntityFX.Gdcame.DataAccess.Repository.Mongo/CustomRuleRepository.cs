namespace EntityFX.Gdcame.DataAccess.Repository.Mongo.MainServer
{
    using EntityFX.Gdcame.Contract.MainServer.Items;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.CustomRule;

    public class CustomRuleRepository : ICustomRuleRepository
    {
        public CustomRule[] FindAll(GetAllCustomRulesCriterion criterion)
        {
            return new CustomRule []{ };
        }
    }
}
