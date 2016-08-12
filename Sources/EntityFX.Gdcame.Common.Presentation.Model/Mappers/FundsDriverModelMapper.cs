using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Common.Presentation.Model.Mappers
{
    public class FundsDriverModelMapper : IMapper<Item, FundsDriverModel>
    {
        public FundsDriverModel Map(Item source, FundsDriverModel destination = null)
        {
            destination = destination ?? new FundsDriverModel();
            destination.Value = source.Price;
            destination.BuyCount = source.BuyCount;
            destination.Id = source.Id;
            destination.Incrementors = source.Incrementors;
            destination.InflationPercent = source.InflationPercent;
            destination.Name = source.Name;
            destination.UnlockValue = source.UnlockValue;
            destination.IsActive = source.IsActive;
            destination.Picture = source.Picture;
            return destination;
        }
    }
}
