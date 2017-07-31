namespace EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer
{
    using EntityFX.Gdcame.Contract.MainServer.Items;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.CustomRule;

    public interface ICustomRuleRepository
    {
        CustomRule[] FindAll(GetAllCustomRulesCriterion criterion);
    }
}