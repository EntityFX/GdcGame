using EntityFX.Gdcame.Common.Contract.Funds;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserFundsDriver;

namespace EntityFX.Gdcame.DataAccess.Repository
{
    public interface IUserFundsDriverRepository
    {
        FundsDriver[] FindByUserId(GetUserFundsDriverByUserIdCriterion criterion);
        void CreateForUser(int userId, FundsDriver[] fundsDrivers);
        void UpdateForUser(int userId, FundsDriver[] fundsDrivers);
    }
}