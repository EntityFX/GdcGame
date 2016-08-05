using EntityFX.Gdcame.NotifyConsumer.Contract;

namespace EntityFX.Gdcame.GameEngine.NetworkGameEngine
{
    public interface INotifyConsumerClientFactory
    {
        INotifyConsumerService BuildNotifyConsumerClient();
    }
}