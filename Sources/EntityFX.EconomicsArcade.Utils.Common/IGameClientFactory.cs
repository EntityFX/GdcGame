using System;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;

namespace EntityFX.EconomicsArcade.Utils.Common
{
    public interface IGameClientFactory
    {
        IGameManager BuildGameClient(Guid sessionGuid);
    }
}