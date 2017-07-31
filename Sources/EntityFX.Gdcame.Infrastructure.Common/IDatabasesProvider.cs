using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Infrastructure.Common
{
    public interface IDatabasesProvider
    {
       
        IContainerBootstrapper GetRepositoryProvider(string connectionString);
    }
}
