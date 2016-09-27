using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Presentation.Contract.Model;

namespace EntityFX.Gdcame.Presentation.Contract.Controller
{
    public interface IAccountController
    {
        Task DeleteAsync(string id);

        AccountInfoModel GetById(string id);

        AccountInfoModel GetByLogin(string login);

        IEnumerable<AccountInfoModel> Get(string filter = null);
    }

}
