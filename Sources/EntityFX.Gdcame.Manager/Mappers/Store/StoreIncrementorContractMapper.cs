using EntityFX.Gdcame.Common.Contract.Incrementors;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.GameEngine.Contract.Incrementors;
using EntityFX.Gdcame.Infrastructure.Common;
using IncrementorTypeEnum = EntityFX.Gdcame.Common.Contract.Incrementors.IncrementorTypeEnum;

namespace EntityFX.Gdcame.Manager.Mappers
{
    public class IncrementorContractMapper : IMapper<IncrementorBase, StoreIncrementor>
    {
        public StoreIncrementor Map(IncrementorBase source, StoreIncrementor destination)
        {
            return new StoreIncrementor()
            {
                IncrementorType = (IncrementorTypeEnum) ((int) source.IncrementorType),
                Value = source.Value
            };
        }
    }
}