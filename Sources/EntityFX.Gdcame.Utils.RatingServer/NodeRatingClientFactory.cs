using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Utils.RatingServer
{
    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.Engine.Contract.RatingServer;
    using EntityFX.Gdcame.Infrastructure.Api.Auth;
    using EntityFX.Gdcame.Utils.WebApiClient.RatingServer;

    public class NodeRatingClientFactory  : INodeRatingClientFactory
    {
        public IRatingDataRetrieve BuildClient(Uri nodeUri)
        {
            var p = new PasswordAuthProvider(nodeUri);
            var res = p.Login(new PasswordAuthRequest<PasswordAuthData>()
            {
                RequestData = new PasswordAuthData() { Usename = "system", Password = "P@ssw0rd" }
            }).Result;
            return new NodeRatingApiClient(res);
        }
    }
}
