using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Presentation.Web.Api.Models;
using EntityFX.Gdcame.Presentation.Web.Model;

namespace EntityFX.Gdcame.Manager.Contract.AdminManager
{
    public interface IAccountController
    {
        void Delete(string id);

        AccountInfoModel GetById(string id);

        IEnumerable<AccountInfoModel> Get(string filter = null);
    }
}
