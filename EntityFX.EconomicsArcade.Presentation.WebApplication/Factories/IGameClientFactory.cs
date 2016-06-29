using System;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication.Factories
{
    public interface IGameClientFactory
    {
        IGameManager BuildGameClient(Guid sessionGuid);
    }
}