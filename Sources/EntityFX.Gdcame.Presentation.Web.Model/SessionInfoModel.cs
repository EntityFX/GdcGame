using System;

namespace EntityFX.Gdcame.Application.Contract.Model.MainServer
{
    public class SessionInfoModel
    {
        public Guid SessionIdentifier { get; set; }

        public DateTime LastActivity { get; set; }
    }
}