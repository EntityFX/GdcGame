using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Contract.DataAccess.GameData
{
    [ServiceContract]
    public interface IGameDataRepository
    {
        [OperationContract]
        Common.GameData GetGameData();
    }
}
