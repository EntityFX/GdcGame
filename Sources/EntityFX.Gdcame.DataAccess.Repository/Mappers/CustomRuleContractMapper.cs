namespace EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Mappers
{
    using EntityFX.Gdcame.Contract.MainServer.Items;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Entities;
    using EntityFX.Gdcame.Infrastructure.Common;

    public class CustomRuleContractMapper : IMapper<CustomRuleEntity, CustomRule>
    {
        public CustomRule Map(CustomRuleEntity source, CustomRule destination = null)
        {
            destination = destination ?? new CustomRule();
            destination.Id = source.Id;
            destination.Name = source.Name;
            return destination;
        }
    }
}