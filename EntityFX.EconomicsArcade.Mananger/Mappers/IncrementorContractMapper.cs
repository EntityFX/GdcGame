using EntityFX.EconomicsArcade.Contract.Common.Incrementors;
using EntityFX.EconomicsArcade.Contract.Game.Incrementors;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using IncrementorTypeEnum = EntityFX.EconomicsArcade.Contract.Common.Incrementors.IncrementorTypeEnum;

namespace EntityFX.EconomicsArcade.Manager.Mappers
{
    public class IncrementorContractMapper : IMapper<IncrementorBase, Incrementor>
    {
        public Incrementor Map(IncrementorBase source, Incrementor destination)
        {
            return new Incrementor()
            {
                IncrementorType = (IncrementorTypeEnum) ((int) source.IncrementorType),
                Value = source.Value
            };
        }
    }
}