using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EntityFX.Gdcame.Utils.WebApiClient.Exceptions;
using RestSharp.Portable.Authenticators;
using RestSharp.Portable.Authenticators.OAuth2;
using RestSharp.Portable.Authenticators.OAuth2.Configuration;

namespace EntityFX.Gdcame.Utils.WebApiClient.Auth
{
    public class PasswordAuthProvider : IAuthProvider<PasswordAuthData, PasswordOAuthContext>
    {
        private readonly Uri _baseUri;

        public PasswordAuthProvider(Uri baseUri)
        {
            _baseUri = baseUri;
        }

        public async Task<PasswordOAuthContext> Login(IAuthRequestData<PasswordAuthData> authRequest)
        {
            var client = new GdcameOAuthClient(new GameClientFactory(_baseUri)
                ,
                new RuntimeClientConfiguration()
                {
                    ClientId = authRequest.RequestData.Usename,
                    ClientSecret = authRequest.RequestData.Password
                }, _baseUri);
            Task<string> token;
            string stringToken = string.Empty;
            token =
                client.GetPasswordToken(new Dictionary<string, string>().ToLookup(pair => pair.Key, pair => pair.Value));
            try
            {
                stringToken = await token;
            }
            catch (HttpRequestException exception)
            {
                ExceptionHandlerHelper.HandleHttpRequestException(exception);
            }
            catch (UnexpectedResponseException unexpectedResponseException)
            {
                throw new WrongAuthException<PasswordAuthData>(authRequest.RequestData, unexpectedResponseException.Message, unexpectedResponseException);
            }
            catch
            {
                //TODO: log here
                throw;
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