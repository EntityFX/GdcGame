using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Infrastructure.Api;


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
        private readonly IAuthProviderFactory<PasswordAuthRequestData, TAuthContext> _authProviderFactory;

        public NodeRatingClientFactory(IAuthProviderFactory<PasswordAuthRequestData, TAuthContext> authProvider, IApiClientFactory<TAuthContext> apiClientFactory)
        {
            _apiClientFactory = apiClientFactory;
            _authProviderFactory = authProvider;
        }

        public IRatingDataRetrieve BuildClient(Uri nodeUri)
        {

            var res = _authProviderFactory.Build(nodeUri).Login(new PasswordAuthRequest()
            {
                RequestData = new PasswordAuthRequestData() { Usename = "system", Password = "P@ssw0rd" }
            }).Result;
            return new NodeRatingApiClient(_apiClientFactory.Build(res));
        }
    }
}
