using System;
using EntityFX.Gdcame.GameEngine.Contract;

namespace EntityFX.Gdcame.GameEngine.NetworkGameEngine
{
    public class GameWithUserId
    {
        //TODO: add Login property
        public string UserId { get; set; }
        public IGame Game { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}