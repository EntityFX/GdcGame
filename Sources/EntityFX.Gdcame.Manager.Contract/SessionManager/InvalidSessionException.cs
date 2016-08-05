using System;

namespace EntityFX.Gdcame.Manager.Contract.SessionManager
{
    public class InvalidSessionException : InvalidOperationException
    {
        public Guid SessionGuid { get; set; }

        public InvalidSessionException(string message, Guid guid)
            : base(message)
        {
            SessionGuid = guid;
        }
    }
}