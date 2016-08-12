using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.FundsDriver;

namespace EntityFX.Gdcame.DataAccess.Repository
{
    public interface IFundsDriverRepository
    {
        Item[] FindAll(GetAllFundsDriversCriterion criterion);
    }
}