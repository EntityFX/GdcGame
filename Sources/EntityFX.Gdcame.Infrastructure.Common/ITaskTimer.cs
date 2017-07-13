using System.Threading.Tasks;

namespace EntityFX.Gdcame.Infrastructure.Common
{
    public interface ITaskTimer
    {
        Task Start();

        void Stop();

        bool IsRunning { get; }
    }
}