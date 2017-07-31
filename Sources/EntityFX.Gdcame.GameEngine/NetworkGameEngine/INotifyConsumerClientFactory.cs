namespace EntityFX.Gdcame.Engine.GameEngine.NetworkGameEngine
{
    using EntityFX.Gdcame.NotifyConsumer.Contract;

    public interface INotifyConsumerClientFactory
    {
        INotifyConsumerService BuildNotifyConsumerClient();
    }
}