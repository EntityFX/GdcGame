using System;

namespace EntityFX.EconomicsArcade.Contract.Manager.SessionManager
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