using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.Infrastructure.Service.Bases;
using EntityFX.Gdcame.NotifyConsumer.Contract;

namespace EntityFX.Gdcame.Utils.ServiceStarter.Collapsed
{
    internal class NotifyConsumerServiceClient<TInfrastructureProxy> : INotifyConsumerService
        where TInfrastructureProxy : IInfrastructureProxy<INotifyConsumerService, Binding>, new()
    {
        public void PushGameData(UserContext userContext, GameData gameData)
        {
            throw new NotImplementedException();
        }
    }
}
