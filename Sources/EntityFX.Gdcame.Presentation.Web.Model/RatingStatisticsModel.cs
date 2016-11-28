using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Application.Contract.Model
{
    public class RatingStatisticsModel
    {
        public string UserID { get; set; }

        public CountValuesModel MunualStepsCount { get; set; }

        public CountValuesModel TotalEarned { get; set; }

        public CountValuesModel RootCounter { get; set; }
    }

    public class CountValuesModel
    {
        public decimal Day { get; set; }

        public decimal Week { get; set; }

        public decimal Total { get; set; }
    }
}
