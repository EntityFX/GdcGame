using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Presentation.Contract.Model;

namespace EntityFX.Gdcame.Presentation.Contract.Controller
{
    public interface IAccountController
    {
        void Delete(string id);

        AccountInfoModel GetById(string id);

        IEnumerable<AccountInfoModel> Get(string filter = null);
    }

}
