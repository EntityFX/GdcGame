using System.Collections.Generic;
using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Application.Contract.Model.MainServer;

namespace EntityFX.Gdcame.Application.Contract.Controller.MainServer
{
    public interface IAccountController
    {
        Task<bool> DeleteAsync(string id);

        Task<AccountInfoModel> GetByIdAsync(string id);

        Task<AccountInfoModel> GetByLoginAsync(string login);

        Task<IEnumerable<AccountInfoModel>> GetAsync(string filter = null);
    }

}
