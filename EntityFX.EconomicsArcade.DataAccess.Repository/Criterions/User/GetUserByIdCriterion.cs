﻿using EntityFX.EconomicsArcade.Infrastructure.Repository.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.User
{
    public class GetUserByIdCriterion : GetByIdCriterion, ICriterion
    {
        public GetUserByIdCriterion(int id)
            :base(id)
        {
        }
    }
}
