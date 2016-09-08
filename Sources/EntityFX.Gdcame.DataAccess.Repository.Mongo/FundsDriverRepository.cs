using EntityFX.Gdcame.DataAccess.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.FundsDriver;

namespace EntityFX.Gdcame.DataAccess.Repository.Mongo
{
    class FundsDriverRepository : IFundsDriverRepository
    {
        public Item[] FindAll(GetAllFundsDriversCriterion criterion)
        {
            throw new NotImplementedException();
        }
    }
}
