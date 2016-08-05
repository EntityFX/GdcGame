using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.DataAccess.Repository.Mappers
{
    public class UserGameCounterEntityMapper : IMapper<UserGameCounter, UserGameCounterEntity>
    {
        public UserGameCounterEntity Map(UserGameCounter source, UserGameCounterEntity destination = null)
        {
            destination = destination ?? new UserGameCounterEntity();
            destination.UserId = source.UserId;
            destination.AutomaticStepsCount = source.AutomaticStepsCount;
            destination.CurrentFunds = source.CurrentFunds;
            destination.DelayedFunds = source.DelayedFunds;
            destination.ManualStepsCount = source.ManualStepsCount;
            destination.TotalFunds = source.TotalFunds;
            return destination;
        }
    }
}