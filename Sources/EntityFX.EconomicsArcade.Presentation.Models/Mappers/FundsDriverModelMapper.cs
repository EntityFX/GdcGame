using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.Presentation.Models
{
    public class FundsDriverModelMapper : IMapper<FundsDriver, FundsDriverModel>
    {
        public FundsDriverModel Map(FundsDriver source, FundsDriverModel destination = null)
        {
            destination = destination ?? new FundsDriverModel();
            destination.Value = source.Value;
            destination.BuyCount = source.BuyCount;
            destination.Id = source.Id;
            destination.Incrementors = source.Incrementors;
            destination.InflationPercent = source.InflationPercent;
            destination.Name = source.Name;
            destination.UnlockValue = source.UnlockValue;
            destination.IsActive = source.IsActive;
            return destination;
        }
    }
}
