using System.ServiceModel;
using EntityFX.Gdcame.Common.Contract;

namespace EntityFX.Gdcame.NotifyConsumer.Contract
{

    [ServiceContract]
    public interface INotifyConsumerService
    {
        [OperationContract(IsOneWay = true)]
        void PushGameData(UserContext userContext, GameData gameData);
    }
}