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
        public double Day { get; set; }

        public double Week { get; set; }

        public double Total { get; set; }
    }
}
