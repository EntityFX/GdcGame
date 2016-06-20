using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.DataAccess
{
    public class GameDataRepositoryClientProxy : IGameDataRepository
    {
        private readonly GameDataRepositoryProxyFactory _gameDataRepositoryProxyFactory;

        public GameDataRepositoryClientProxy(GameDataRepositoryProxyFactory gameDataRepositoryProxyFactory)
        {
            _gameDataRepositoryProxyFactory = gameDataRepositoryProxyFactory;
        }
        
        public EntityFX.EconomicsArcade.Contract.Common.GameData GetGameData()
        {
            var proxy = _gameDataRepositoryProxyFactory.OpenChannel(new Uri("net.tcp://localhost:8777/EntityFX.EconomicsArcade.DataAccess/EntityFX.EconomicsArcade.Contract.DataAccess.GameData.IGameDataRepository"));
            var data = proxy.GetGameData();
            ((IClientChannel)proxy).Close();
                        ((IDisposable)proxy).Dispose();
            return data;
        }
    }
}
