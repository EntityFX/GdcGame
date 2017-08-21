namespace EntityFX.Gdcame.Contract.Common
{
    using System;

    public class GameFrozenException : InvalidOperationException
    {
        public GameFrozenException(string message, string server)
            : base(message)
        {
            this.Server = server;
        }

        public string Server { get; set; }
    }
}