using System;
using RestSharp;



namespace EntityFX.Gdcame.Utils.WebApiClient
{
    public class GameClientFactory 
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