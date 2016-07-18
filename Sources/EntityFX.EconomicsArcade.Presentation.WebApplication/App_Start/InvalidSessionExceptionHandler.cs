using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Presentation.WebApplication.Providers;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication
{
    public class InvalidSessionExceptionHandler : ExceptionHandler
    {
        private readonly IGameDataProvider _gameDataProvider;

        public InvalidSessionExceptionHandler(IGameDataProvider gameDataProvider)
        {
            _gameDataProvider = gameDataProvider;
        }

        /// <summary>
        /// When overridden in a derived class, handles the exception synchronously.
        /// </summary>
        /// <param name="context">The exception handler context.</param>
        public override void Handle(ExceptionHandlerContext context)
        {
            if (context.Exception is FaultException<InvalidSessionFault>)
            {
                _gameDataProvider.ClearSession();
                //_gameDataProvider.InitializeSession(context.RequestContext.Principal.Identity.Name);
            }
        }
    }
}