using System;
using System.Linq;
using System.Threading.Tasks;
using RestSharp.Portable;
using RestSharp.Portable.Authenticators.OAuth2;
using RestSharp.Portable.Authenticators.OAuth2.Configuration;
using RestSharp.Portable.Authenticators.OAuth2.Infrastructure;
using RestSharp.Portable.Authenticators.OAuth2.Models;

namespace EntityFX.Gdcame.Utils.WebApiClient.Auth
{
    public class GdcameOAuthClient : OAuth2Client
    {
        private readonly Uri _endpointUri;

        public GdcameOAuthClient(IRequestFactory factory, IClientConfiguration configuration, Uri endpointUri) : base(factory, configuration)
        {
            _endpointUri = endpointUri;
        }

        protected override UserInfo ParseUserInfo(string content)
        {
            throw new NotImplementedException();
        }

        public override string Name
        {
            get { return "Gdcame"; }
        }

        protected override Endpoint AccessCodeServiceEndpoint { get { return null; } }

        protected override Endpoint AccessTokenServiceEndpoint
        {
            get
            {
                return new Endpoint()
                {
                    BaseUri = _endpointUri.AbsoluteUri,
                    Resource = "/token"
                };
            }
        }

        protected override Endpoint UserInfoServiceEndpoint {
            get { return null;}
        }

        /// <summary>
        /// Called before the request to get the access token
        /// 
        /// </summary>
        /// <param name="args">The request/response arguments</param>
        protected override void BeforeGetAccessToken(BeforeAfterRequestArgs args)
        {
            RestRequestExtensions.AddObject(args.Request, (object)new
            {
                username = this.Configuration.ClientId,
                password = this.Configuration.ClientSecret,
                grant_type = "password"
            });

            /*RestRequestExtensions.AddObject(args.Request, (object)new
            {
                refresh_token = LookupExtensions.GetOrThrowUnexpectedResponse(args.Parameters, "refresh_token")
            });*/

        }

        protected override string ParseAccessTokenResponse(string content)
        {
            var parseResult = OAuth2Client.ParseStringResponse(content, "error", "error_description", "access_token");
            if (parseResult.Contains("error"))
            {
                throw new UnexpectedResponseException("error", parseResult["error_description"].FirstOrDefault());
            }
            return OAuth2Client.ParseStringResponse(content, "access_token");
        }

        public async Task<string> GetPasswordToken(ILookup<string, string> parameters)
        {
            IRestClient client = RequestFactoryExtensions.CreateClient(Factory, this.AccessTokenServiceEndpoint);
            client.IgnoreResponseStatusCode = true;
            IRestRequest request = RequestFactoryExtensions.CreateRequest(Factory, this.AccessTokenServiceEndpoint, Method.POST);
            BeforeAfterRequestArgs args1 = new BeforeAfterRequestArgs();
            args1.Client = client;
            args1.Request = request;
            ILookup<string, string> lookup1 = parameters;
            args1.Parameters = lookup1;
            IClientConfiguration configuration = this.Configuration;
            args1.Configuration = configuration;
            this.BeforeGetAccessToken(args1);
            IRestResponse response;
            response = await client.Execute(request);

            string content = RestSharp.Portable.Authenticators.OAuth2.Infrastructure.RestResponseExtensions.GetContent(response);
            this.AccessToken = this.ParseAccessTokenResponse(content);
            int num = Enumerable.FirstOrDefault<int>(Enumerable.Select<string, int>(OAuth2Client.ParseStringResponse(content, new string[1]
            {
                "expires_in"
            })["expires_in"], (Func<string, int>)(x => Convert.ToInt32(x, 10))));
            this.ExpiresAt = num != 0 ? new DateTime?(DateTime.Now.AddSeconds((double)num)) : new DateTime?();
            BeforeAfterRequestArgs args2 = new BeforeAfterRequestArgs();
            args2.Response = response;
            ILookup<string, string> lookup2 = parameters;
            args2.Parameters = lookup2;
            this.AfterGetAccessToken(args2);
            return AccessToken;
        }
    }
}