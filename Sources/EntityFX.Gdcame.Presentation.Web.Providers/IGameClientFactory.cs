using System;
using EntityFX.Gdcame.Manager.Contract.MainServer.GameManager;

namespace EntityFX.Gdcame.Application.Providers.MainServer
{
    public interface IGameClientFactory
    {
        IGameManager BuildGameClient(Guid sessionGuid);
    }
}