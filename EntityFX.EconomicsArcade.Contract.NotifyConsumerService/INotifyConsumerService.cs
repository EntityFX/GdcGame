using System.ServiceModel;
using EntityFX.EconomicsArcade.Contract.Common;

namespace EntityFX.EconomicsArcade.Contract.NotifyConsumerService
{

    [ServiceContract]
    public interface INotifyConsumerService
    {
        [OperationContract(IsOneWay = true)]
        void PushGameData(UserContext userContext, GameData gameData);
    }
}