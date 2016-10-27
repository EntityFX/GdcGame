using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Utils.WebApiClient.Exceptions;
using RestSharp.Portable.Authenticators;
using RestSharp.Portable.Authenticators.OAuth2;
using RestSharp.Portable.Authenticators.OAuth2.Configuration;

namespace EntityFX.Gdcame.Utils.WebApiClient.Auth
{
    public class PasswordAuthProvider : IAuthProvider<PasswordAuthData, PasswordOAuthContext>
    {
        private readonly ILogger _logger;
        private readonly Uri _baseUri;

        public TimeSpan? Timeout { get; private set; }

        public PasswordAuthProvider(Uri baseUri, ILogger logger = null, TimeSpan? timeout = null)
        {
            _baseUri = baseUri;
            _logger = logger;
            Timeout = timeout;
        }

        public async Task<PasswordOAuthContext> Login(IAuthRequestData<PasswordAuthData> authRequest)
        {
            var client = new GdcameOAuthClient(new GameClientFactory(_baseUri)
                ,
                new RuntimeClientConfiguration()
                {
                    ClientId = authRequest.RequestData.Usename,
                    ClientSecret = authRequest.RequestData.Password
                }, _baseUri, Timeout);
            Task<string> token;
            string stringToken = string.Empty;
            token =
                client.GetPasswordToken(new Dictionary<string, string>().ToLookup(pair => pair.Key, pair => pair.Value));
            try
            {
                stringToken = await token;
            }
            catch (HttpRequestException requestException)
            {
                ExceptionHandlerHelper.HandleHttpRequestException(requestException);
            }
            catch (UnexpectedResponseException unexpectedResponseException)
            {
                throw new WrongAuthException<PasswordAuthData>(new WrongAuthData<PasswordAuthData> { RequestData = authRequest.RequestData }
                    , unexpectedResponseException.Message, unexpectedResponseException);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
            }

            return new PasswordOAuthContext()
            {
                Context = new OAuth2AuthorizationRequestHeaderAuthenticator(client, "bearer"),
                OAuthToken = stringToken,
                BaseUri = _baseUri
            };
        }


    }
}