using System;
using EntityFX.Gdcame.Manager.Contract.GameManager;

namespace EntityFX.Gdcame.Utils.Common
{
    public interface IGameClientFactory
    {
        IGameManager BuildGameClient(Guid sessionGuid);
    }
}