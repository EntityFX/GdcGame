using EntityFX.Gdcame.DataAccess.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.CustomRule;

namespace EntityFX.Gdcame.DataAccess.Repository.LocalStorage
{
    public class CustomRuleRepository : ICustomRuleRepository
    {
        public CustomRule[] FindAll(GetAllCustomRulesCriterion criterion)
        {
            return new CustomRule []{ };
        }
    }
}
