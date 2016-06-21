using EntityFX.EconomicsArcade.Contract.Common.Incrementors;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Mappers
{
    public class IncrementorContractMapper : IMapper<IncrementorEntity, Incrementor>
    {
        public Incrementor Map(IncrementorEntity source, Incrementor destionation = null)
        {
            destionation = destionation ?? new Incrementor();
            destionation.IncrementorType = (IncrementorTypeEnum)source.Type;
            destionation.Value = Convert.ToInt32(source.Value);
            return destionation;
        }
    }
}
