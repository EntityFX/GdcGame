using System;
using RestSharp.Portable;
using RestSharp.Portable.Authenticators.OAuth2.Infrastructure;
using RestSharp.Portable.HttpClient;

namespace EntityFX.Gdcame.Utils.WebApiClient
{
    public class GameClientFactory : IRequestFactory
    {
        private readonly Uri _baseUri;

        public Uri BaseUri { get; set; }

        public GameClientFactory(Uri baseUri)
        {
            _baseUri = baseUri;
        }

        public IRestClient CreateClient()
        {
            return new RestClient(_baseUri.AbsoluteUri);
        }

        public IRestRequest CreateRequest(string resource)
        {
            return new RestRequest(resource);
        }
    }
}