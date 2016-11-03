namespace EntityFX.Gdcame.Infrastructure.Common
{
    public interface IWorker
    {
        void Run();

        string Name { get; }

        bool IsRunning { get; }
    }
}