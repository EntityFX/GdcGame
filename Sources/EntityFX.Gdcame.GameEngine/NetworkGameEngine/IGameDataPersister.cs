using System.Collections.Generic;

namespace EntityFX.Gdcame.GameEngine.NetworkGameEngine
{
    public interface IGameDataPersister
    {
        void PersistGamesData(IList<GameWithUserId> gamesWithUserIds);
    }
}