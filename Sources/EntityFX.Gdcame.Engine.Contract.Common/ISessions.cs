namespace EntityFX.Gdcame.Engine.Contract.GameEngine
{
    using System;
    using System.Collections.Generic;

    using EntityFX.Gdcame.Contract.Common;

    public interface ISessions
    {
        Guid AddSession(User user);
        Session GetSession(Guid sessionId);

        void RemoveAllSessions();

        void RemoveSession(Guid sessionId);

        IDictionary<Guid, Session> Sessions { get; }

        IDictionary<string, string> Identities { get; }

    }
}