namespace EntityFX.Gdcame.Contract.Common
{
    using System;

    public class InvalidSessionException : InvalidOperationException
    {
        public InvalidSessionException(string message, Guid guid)
            : base(message)
        {
            this.SessionGuid = guid;
        }

        public Guid SessionGuid { get; set; }
    }
}