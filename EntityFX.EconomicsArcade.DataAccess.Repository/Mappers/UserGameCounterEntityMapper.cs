using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Mappers
{
    public class UserGameCounterEntityMapper : IMapper<UserGameCounter, UserGameCounterEntity>
    {
        public UserGameCounterEntity Map(UserGameCounter source, UserGameCounterEntity destination = null)
        {
            destination = destination ?? new UserGameCounterEntity();
            destination.UserId = source.UserId;
            destination.AutomaticStepsCount = source.AutomaticStepsCount;
            destination.CategoryFunds = source.CategoryFunds;
            destination.DelayedFunds = source.DelayedFunds;
            destination.ManualStepsCount = source.ManualStepsCount;
            destination.TotalFunds = source.TotalFunds;
            return destination;
        }
    }
}