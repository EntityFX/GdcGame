using System.ServiceModel;

namespace EntityFX.Gdcame.NotifyConsumer.Contract
{
    using EntityFX.Gdcame.Contract.MainServer;

    [ServiceContract]
    public interface INotifyConsumerService
    {
        [OperationContract(IsOneWay = true)]
        void PushGameData(UserContext userContext, GameData gameData);
    }
}