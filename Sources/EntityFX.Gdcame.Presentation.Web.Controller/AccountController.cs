using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Application.Contract.Controller.MainServer;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Application.Contract.Model.MainServer;
using EntityFX.Gdcame.Manager.Contract.MainServer.UserManager;

namespace EntityFX.Gdcame.Application.Api.Controller.MainServer
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/admin/accounts")]
    public class AccountController : ApiController, IAccountController
    {
        private readonly ISimpleUserManager _simpleUserManager;

        public AccountController(ISimpleUserManager simpleUserManager)
        {
            _simpleUserManager = simpleUserManager;
        }

        [HttpDelete]
        [Route("")]
        public async Task<bool> DeleteAsync([FromBody] string id)
        {
            return await Task.Factory.StartNew(() => { _simpleUserManager.Delete(id); return true; });
        }

        [HttpGet]
        [Route("{id:length(32)}")]
        public async Task<AccountInfoModel> GetByIdAsync(string id)
        {
            return await Task.Factory.StartNew(() =>
            {
                var userData = _simpleUserManager.FindById(id);
                if (userData != null)
                {
                    return new AccountInfoModel()
                    {
                        UserId = userData.Id,
                        Login = userData.Login
                    };
                }
                throw new HttpResponseException(HttpStatusCode.NotFound);
            });
        }

        [HttpGet]
        [Route("filter/{filter?}")]
        [Route("")]
        public async Task<IEnumerable<AccountInfoModel>> GetAsync([FromUri]string filter = null)
        {
            return await Task.Factory.StartNew(() =>

                _simpleUserManager.FindByFilter(filter)
                .Select(u => new AccountInfoModel
                {
                    UserId = u.Id,
                    Login = u.Login
                }));

        }

        [HttpGet]
        [Route("login/{login}")]
        public async Task<AccountInfoModel> GetByLoginAsync(string login)
        {
            return await Task.Factory.StartNew(() =>
            {
                var userData = _simpleUserManager.Find(login);
                if (userData != null)
                {
                    return new AccountInfoModel()
                    {
                        UserId = userData.Id,
                        Login = userData.Login
                    };
                }
                throw new HttpResponseException(HttpStatusCode.NotFound);
            });
        }
    }
}