namespace EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer
{
    using EntityFX.Gdcame.Common.Contract.Items;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.CustomRule;

    public interface ICustomRuleRepository
    {
        CustomRule[] FindAll(GetAllCustomRulesCriterion criterion);
    }
}