using System;

namespace EntityFX.Gdcame.Manager.Contract.MainServer.SessionManager
{
    public class InvalidSessionException : InvalidOperationException
    {
        public InvalidSessionException(string message, Guid guid)
            : base(message)
        {
            SessionGuid = guid;
        }

        public Guid SessionGuid { get; set; }
    }
}