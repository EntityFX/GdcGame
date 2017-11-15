using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Authenticators;

namespace EntityFX.Gdcame.Utils.RatingServer
{
    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.Engine.Contract.RatingServer;
    using EntityFX.Gdcame.Infrastructure;
    using EntityFX.Gdcame.Infrastructure.Api.Auth;
    using EntityFX.Gdcame.Utils.WebApiClient.RatingServer;

    public class NodeRatingClientFactory  : INodeRatingClientFactory
    {
        private readonly IAuthProviderFactory<PasswordOAuth2RequestData, IAuthenticator> _authProviderFactory;

        public NodeRatingClientFactory(IAuthProviderFactory<PasswordOAuth2RequestData, IAuthenticator> authProviderFactory)
        {
            _authProviderFactory = authProviderFactory;
        }

        public IRatingDataRetrieve BuildClient(Uri nodeUri)
        {
            var res = _authProviderFactory.Build(nodeUri).Login(new PasswordOAuth2Request()
            {
                RequestData = new PasswordOAuth2RequestData() { Usename = "system", Password = "P@ssw0rd" }
            }).Result;
            return new NodeRatingApiClient(new RestsharpApiAdapter(res));
        }
    }
}
