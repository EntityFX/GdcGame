using EntityFx.EconomicsArcade.TestApplication.UssrSimulator;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Manager
{
    public class GameSessions
    {
        private static IDictionary<string, IGame> _gameSessions = new Dictionary<string, IGame>();

        private static IDictionary<Guid, Session> _sessions = new Dictionary<Guid, Session>();


        public IGame GetGame(Guid sessionId)
        {
            var session = GetSession(sessionId);
            if (session == null) return null;

            if (!_gameSessions.ContainsKey(session.Login))
            {
                var gameInOtherSessions = _sessions.Values.FirstOrDefault(_ => _.Login == session.Login);
                if (gameInOtherSessions == null)
                {
                    _gameSessions.Add(session.Login, new UssrSimulatorGame());
                }
                return _gameSessions[gameInOtherSessions.Login];
            }
            return _gameSessions[session.Login];
        }

        public Guid AddSession(string login)
        {
            var session = new Session()
            {
                Login = login,
                SessionIdentifier = Guid.NewGuid()
            };
            _sessions.Add(session.SessionIdentifier, session);
            return session.SessionIdentifier;
        }

        public Session GetSession(Guid sessionId)
        {
            return !_sessions.ContainsKey(sessionId) ? null : _sessions[sessionId];
        }
    }
}
