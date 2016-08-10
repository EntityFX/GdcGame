using EntityFX.Gdcame.NotifyConsumer.Signalr;
using Microsoft.AspNet.SignalR;

namespace EntityFX.Gdcame.NotifyConsumer
{
    public interface IHubContextAccessor
    {
        IHubContext GetHubContext();
    }

    class HubContextAccessor : IHubContextAccessor
    {
        public IHubContext GetHubContext()
        {
            return GlobalHost.ConnectionManager.GetHubContext<GameDataHub>();
        }
    }
}