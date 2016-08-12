using EntityFX.Gdcame.Common.Contract.Incrementors;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.GameEngine.Contract.Incrementors;
using EntityFX.Gdcame.Infrastructure.Common;
using IncrementorTypeEnum = EntityFX.Gdcame.Common.Contract.Incrementors.IncrementorTypeEnum;

namespace EntityFX.Gdcame.Manager.Mappers
{
    public class StoreIncrementorContractMapper : IMapper<IncrementorBase, StoredIncrementor>
    {
        public StoredIncrementor Map(IncrementorBase source, StoredIncrementor destination)
        {
            return new StoredIncrementor()
            {
                Value = source.Value
            };
        }
    }
}