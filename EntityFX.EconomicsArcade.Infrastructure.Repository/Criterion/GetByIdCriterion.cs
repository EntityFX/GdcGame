using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Infrastructure.Repository.Criterion
{
    public class GetByIdCriterion : ICriterion
    {
        public int Id { get; set; }

        public GetByIdCriterion(int id)
        {
            Id = id;
        }
    }
}
