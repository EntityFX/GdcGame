using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager.Incrementors;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using IncrementorTypeEnum = EntityFX.EconomicsArcade.Contract.Manager.GameManager.Incrementors.IncrementorTypeEnum;

namespace EntityFX.EconomicsArcade.Manager.Mappers
{
    public class IncrementorContractMapper : IMapper<IncrementorBase, Incrementor>
    {
        public Incrementor Map(IncrementorBase source)
        {
            return new Incrementor()
            {
                IncrementorType = (IncrementorTypeEnum) ((int) source.IncrementorType),
                Value = source.Value
            };
        }
    }
}