namespace EntityFX.Gdcame.Engine.Contract.GameEngine
{
    using System;

    using EntityFX.Gdcame.Kernel.Contract;

    public class GameWithUserId
    {
        //TODO: add Login property
        public string UserId { get; set; }
        public IGame Game { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}