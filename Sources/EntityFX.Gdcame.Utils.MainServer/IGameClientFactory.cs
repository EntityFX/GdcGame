using System;
using EntityFX.Gdcame.Manager.Contract.MainServer.GameManager;

namespace EntityFX.Gdcame.Utils.MainServer
{
    public interface IGameClientFactory
    {
        IGameManager BuildGameClient(Guid sessionGuid);
    }
}