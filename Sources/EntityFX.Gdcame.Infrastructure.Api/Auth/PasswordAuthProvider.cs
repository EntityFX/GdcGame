using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EntityFX.Gdcame.Infrastructure.Api.Exceptions;
using EntityFX.Gdcame.Infrastructure.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace EntityFX.Gdcame.Infrastructure.Api.Auth
{
    public class PasswordAuthProvider : IAuthProvider<PasswordAuthData, PasswordOAuthContext>
    {
        private readonly ILogger _logger;
        private readonly Uri _baseUri;

        public TimeSpan? Timeout { get; private set; }

        private class OAuthPasswordRequestData
        {
            [JsonProperty("grant_type")]
            public string GrantType { get; private set; }
            [JsonProperty("username")]
            public string Username { get; set; }
            [JsonProperty("password")]
            public string Password { get; set; }

            public OAuthPasswordRequestData()
            {
                GrantType = "password";
            }
        }

        private class OAuthPasswordResponseData
        {

            [JsonProperty("access_token")]
            public string AccessToken { get; set; }
            [JsonProperty("token_type")]
            public string TokenType { get; set; }
            [JsonProperty("expires_in")]
            public int ExpiresIn { get; set; }
        }

        public PasswordAuthProvider(Uri baseUri, ILogger logger = null, TimeSpan? timeout = null)
        {
            _baseUri = baseUri;
            _logger = logger;
            Timeout = timeout;
        }

        public static void HandleNotSuccessRequest(PasswordAuthData authRequest, IRestResponse response)
        {
            JToken token = null;
            try
            {
                token = JObject.Parse(response.Content);
            }
            catch (System.Exception)
            {
                throw new ClientException<ErrorData>(null, response.StatusDescription);
            }


            var exceptionType = (string)token.SelectToken("error", false);
            if (exceptionType != null)
            {
                var errorMessage = (string) token.SelectToken("error_description", false);
                throw new WrongAuthException<PasswordAuthData>(new WrongAuthData<PasswordAuthData>() { Message = errorMessage, RequestData = authRequest}, errorMessage , null);
            }
        }

        public async Task<PasswordOAuthContext> Login(IAuthRequestData<PasswordAuthData> authRequest)
        {
            var factory = new GameClientFactory(_baseUri);
            var request = factory.CreateRequest("/token");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "password");
            request.AddParameter("username", authRequest.RequestData.Usename);
            request.AddParameter("password", authRequest.RequestData.Password);

            var client = new GameClientFactory(_baseUri).CreateClient();
            client.AddHandler("application/json", CustomJsonDeserializer.Default);
            client.AddHandler("text/javascript", CustomJsonDeserializer.Default);
            IRestResponse<OAuthPasswordResponseData> stringToken = null;

            try
            {
                stringToken = await client.ExecutePostTaskAsync<OAuthPasswordResponseData>(request);

            }
            catch (Exception exception)
            {
                _logger.Error(exception);
            }

            if (stringToken.ErrorException != null)
            {
                ExceptionHandlerHelper.HandleHttpRequestException(stringToken.ErrorException);
            }
            if (
new HttpStatusCode[]
{
                        HttpStatusCode.BadRequest, HttpStatusCode.Forbidden, HttpStatusCode.Unauthorized, HttpStatusCode.BadGateway, HttpStatusCode.InternalServerError

}.Contains(stringToken.StatusCode))
            {
                HandleNotSuccessRequest(authRequest.RequestData, stringToken);
            }

            return new PasswordOAuthContext()
            {
                Context = new OAuth2AuthorizationRequestHeaderAuthenticator(stringToken.Data.AccessToken, stringToken.Data.TokenType),
                OAuthToken = stringToken.Data.AccessToken,
                BaseUri = _baseUri
            };
        }


    }
}