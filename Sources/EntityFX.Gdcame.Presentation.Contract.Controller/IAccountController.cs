using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Model;

namespace EntityFX.Gdcame.Application.Contract.Controller
{
    public interface IAccountController
    {
        Task<bool> DeleteAsync(string id);

        Task<AccountInfoModel> GetByIdAsync(string id);

        Task<AccountInfoModel> GetByLoginAsync(string login);

        Task<IEnumerable<AccountInfoModel>> GetAsync(string filter = null);
    }

}
