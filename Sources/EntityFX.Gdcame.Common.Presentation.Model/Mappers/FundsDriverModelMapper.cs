using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Common.Presentation.Model.Mappers
{
    public class FundsDriverModelMapper : IMapper<Item, ItemModel>
    {
        public ItemModel Map(Item source, ItemModel destination = null)
        {
            destination = destination ?? new ItemModel();
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