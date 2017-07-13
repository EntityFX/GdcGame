using System;

namespace EntityFX.Gdcame.Common.Application.Model
{
    public class SessionInfoModel
    {
        public Guid SessionIdentifier { get; set; }

        public DateTime LastActivity { get; set; }
    }
}