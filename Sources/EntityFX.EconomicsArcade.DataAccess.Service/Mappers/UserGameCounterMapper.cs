using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.DataAccess.Service.Mappers
{
    public class UserGameCounterMapper : IMapper<GameData, UserGameCounter>
    {
        public UserGameCounter Map(GameData source, UserGameCounter destination = null)
        {
            destination = destination  ??
            new UserGameCounter()
            {
                TotalFunds = source.Counters.TotalFunds,
                CurrentFunds = source.Counters.CurrentFunds,
                AutomaticStepsCount = source.AutomaticStepsCount,
                ManualStepsCount = source.ManualStepsCount,
                DelayedFunds = 0
            };
            return destination;
        }
    }
}