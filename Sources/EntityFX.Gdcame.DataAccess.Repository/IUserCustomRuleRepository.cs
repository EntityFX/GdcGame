using EntityFX.Gdcame.Common.Contract.Funds;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserCustomRuleInfo;

namespace EntityFX.Gdcame.DataAccess.Repository
{
    public interface IUserCustomRuleRepository
    {
        CustomRuleInfo[] FindByUserId(GetUserCustomRuleInfoByUserIdCriterion criterion);
        void CreateForUser(int userId, CustomRuleInfo[] fundsDrivers);
        void UpdateForUser(int userId, CustomRuleInfo[] fundsDrivers); 
    }
}