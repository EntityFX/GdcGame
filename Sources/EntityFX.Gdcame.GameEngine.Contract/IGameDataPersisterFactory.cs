namespace EntityFX.Gdcame.Engine.Contract.GameEngine
{
    public interface IGameDataPersisterFactory
    {
        IGameDataPersister BuildGameDataPersister();
    }
}