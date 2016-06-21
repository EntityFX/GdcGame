using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Infrastructure.Repository.Criterion
{
    public class GetAllCriterion : ICriterion
    {
        public static readonly GetAllCriterion Value = new GetAllCriterion();

        protected GetAllCriterion()
        { }
    }
}
