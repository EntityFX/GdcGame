using System;
using RestSharp.Portable;
using RestSharp.Portable.Authenticators.OAuth2.Infrastructure;
using RestSharp.Portable.HttpClient;

namespace EntityFX.Gdcame.Utils.WebApiClient
{
    public class GameClientFactory : IRequestFactory
    {
        public Uri BaseUri { get; private set; }

        public GameClientFactory(Uri baseUri)
        {
            BaseUri = baseUri;
        }

        public IRestClient CreateClient()
        {
            return new RestClient(BaseUri.AbsoluteUri);
        }

        public IRestRequest CreateRequest(string resource)
        {
            return new RestRequest(resource);
        }
    }
}