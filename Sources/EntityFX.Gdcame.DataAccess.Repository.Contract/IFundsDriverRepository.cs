using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.FundsDriver;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract
{
    public interface IFundsDriverRepository
    {
        Item[] FindAll(GetAllFundsDriversCriterion criterion);
    }
}