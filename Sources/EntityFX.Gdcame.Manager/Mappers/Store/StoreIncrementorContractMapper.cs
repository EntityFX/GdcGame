using EntityFX.Gdcame.GameEngine.Contract.Incrementors;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.MainServer.Mappers.Store
{
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData.Store;

    public class StoreIncrementorContractMapper : IMapper<IncrementorBase, StoredIncrementor>
    {
        public StoredIncrementor Map(IncrementorBase source, StoredIncrementor destination)
        {
            return new StoredIncrementor
            {
                Value = source.Value
            };
        }
    }
}