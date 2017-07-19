using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.DataAccess.Contract.User;

namespace EntityFX.Gdcame.Infrastructure.Common
{
    public interface IDatabasesProvider
    {
       
        IContainerBootstrapper GetRepositoryProvider(string connectionString);
    }
}
