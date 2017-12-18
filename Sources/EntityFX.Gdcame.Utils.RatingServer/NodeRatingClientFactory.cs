using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Infrastructure.Api;
using EntityFX.Gdcame.Infrastructure.Common;


namespace EntityFX.Gdcame.Utils.RatingServer
{
    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.Engine.Contract.RatingServer;
    using EntityFX.Gdcame.Infrastructure;
    using EntityFX.Gdcame.Infrastructure.Api.Auth;
    using EntityFX.Gdcame.Utils.WebApiClient.RatingServer;

    public class NodeRatingClientFactory<TAuthContext> : INodeRatingClientFactory
        where TAuthContext : class
    {
        private readonly IApiClientFactory<TAuthContext> _apiClientFactory;
        private readonly ILogger _Logger;
        private readonly IAuthProviderFactory<PasswordAuthRequestData, TAuthContext> _authProviderFactory;

        public NodeRatingClientFactory(IAuthProviderFactory<PasswordAuthRequestData, TAuthContext> authProvider, IApiClientFactory<TAuthContext> apiClientFactory, ILogger _logger)
        {
            _apiClientFactory = apiClientFactory;
            _Logger = _logger;
            _authProviderFactory = authProvider;
        }

        public IRatingDataRetrieve BuildClient(Uri nodeUri)
        {
            try
            {
                var res = _authProviderFactory.Build(nodeUri).Login(new PasswordAuthRequest()
                {
                    RequestData = new PasswordAuthRequestData() { Usename = "system", Password = "P@ssw0rd" }
                }).Result;
                return new NodeRatingApiClient(_apiClientFactory.Build(res));
            }
            catch (Exception e)
            {
                _Logger.Error(e);
                throw;
            }
        }
    }
}
