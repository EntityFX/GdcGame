namespace EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer
{
    using EntityFX.Gdcame.Contract.MainServer.Counters;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.Counters;

    public interface ICountersRepository
    {
        CounterBase[] FindAll(GetAllCountersCriterion criterion);
    }
}