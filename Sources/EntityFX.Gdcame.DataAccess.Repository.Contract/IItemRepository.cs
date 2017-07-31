namespace EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer
{
    using EntityFX.Gdcame.Common.Contract.Items;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.FundsDriver;

    public interface IItemRepository
    {
        Item[] FindAll(GetAllFundsDriversCriterion criterion);
    }
}