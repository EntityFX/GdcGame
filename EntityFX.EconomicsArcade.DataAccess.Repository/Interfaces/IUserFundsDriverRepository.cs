using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserFundsDriver;

namespace EntityFX.EconomicsArcade.DataAccess.Repository
{
    public interface IUserFundsDriverRepository
    {
        FundsDriver[] FindByUserId(GetFundsDriverByUserIdCriterion criterion);
        void CreateForUser(int userId, FundsDriver fundsDriver);
        void UpdateForUser(int userId, FundsDriver fundsDriver);
    }
}