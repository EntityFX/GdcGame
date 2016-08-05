using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.DataAccess.Service.Mappers
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