using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.DataAccess.Repository.Mappers
{
    public class UserGameCounterContractMapper : IMapper<UserGameCounterEntity, UserGameCounter>
    {
        public UserGameCounter Map(UserGameCounterEntity source, UserGameCounter destination = null)
        {
            destination = destination ?? new UserGameCounter();
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