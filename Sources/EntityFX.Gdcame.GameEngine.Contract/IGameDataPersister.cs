namespace EntityFX.Gdcame.Engine.Contract.GameEngine
{
    using System.Collections.Generic;

    public interface IGameDataPersister
    {
        void PersistGamesData(IList<GameWithUserId> gamesWithUserIds);
    }
}