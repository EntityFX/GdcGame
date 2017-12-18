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
using EntityFX.Gdcame.Infrastructure.Api.Auth;
using EntityFX.Gdcame.Infrastructure.Api;

namespace EntityFX.Gdcame.Infrastructure
{
    public class RestsharpPasswordAuthProvider : IAuthProvider<PasswordAuthRequestData, IAuthenticator>
    {
        private readonly ILogger _logger;
        private readonly Uri _baseUri;

        public TimeSpan? Timeout { get; private set; }


        private class TokenResponseData
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }
            [JsonProperty("token_type")]
            public string TokenType { get; set; }
            [JsonProperty("expires_in")]
            public int ExpiresIn { get; set; }
        }

        public RestsharpPasswordAuthProvider(Uri baseUri, ILogger logger = null, TimeSpan? timeout = null)
        {
            _baseUri = baseUri;
            _logger = logger;
            Timeout = timeout;
        }

        public static void HandleNotSuccessRequest(PasswordAuthRequestData authRequest, IRestResponse response)
        {
            JToken token = null;
            try
            {
                token = JToken.Parse(response.Content);
            }
            catch (System.Exception exception)
            {
                throw new ClientException<ErrorData>(null, response.StatusDescription, exception);
            }

            if (token.Type == JTokenType.String)
            {
                throw new WrongAuthException<PasswordAuthRequestData>(
                    new WrongAuthData<PasswordAuthRequestData>() { Message = token.Value<string>(), RequestData = authRequest }, token.Value<string>(), null);

            }


            var exceptionType = (string)token.SelectToken("error", false);
            if (exceptionType != null)
            {
                var errorMessage = (string) token.SelectToken("error_description", false);
                throw new WrongAuthException<PasswordAuthRequestData>(new WrongAuthData<PasswordAuthRequestData>() { Message = errorMessage, RequestData = authRequest}, errorMessage , null);
            }
        }

        public async Task<IAuthContext<IAuthenticator>> Login(IAuthRequest<PasswordAuthRequestData> authRequest)
        {
            var factory = new GameClientFactory(_baseUri);
            var request = factory.CreateRequest("/api/auth/token");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("login", authRequest.RequestData.Usename);
            request.AddParameter("password", authRequest.RequestData.Password);

            var client = new GameClientFactory(_baseUri).CreateClient();
            client.AddHandler("application/json", CustomJsonDeserializer.Default);
            client.AddHandler("text/javascript", CustomJsonDeserializer.Default);
            IRestResponse<TokenResponseData> stringToken = null;

            try
            {
                stringToken = await client.ExecutePostTaskAsync<TokenResponseData>(request);

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

            return new TokenAuthContext<IAuthenticator>()
            {
                Context = new RestsharpAuthenticatorContext() { ApiContext = new JwtAuthenticator(stringToken.Data.AccessToken) },
                Token = stringToken.Data.AccessToken,
                BaseUri = _baseUri
            };
        }
    }
}