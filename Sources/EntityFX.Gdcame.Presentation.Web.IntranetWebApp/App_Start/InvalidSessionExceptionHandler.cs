using System.ServiceModel;
using System.Web.Http.ExceptionHandling;
using EntityFX.Gdcame.Manager.Contract;
using EntityFX.Gdcame.Presentation.Web.Providers.Providers;

namespace EntityFX.Gdcame.Presentation.Web.IntranetWebApp
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
              //  _gameDataProvider.InitializeSession(context.RequestContext.Principal.Identity.Name);
            }
        }
    }
}