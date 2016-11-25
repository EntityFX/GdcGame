using EntityFX.Gdcame.Common.Contract.UserRating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.DataAccess.Contract.Rating
{
    public class RatingHistory
    {
        public string UserID { get; set; }
        public DateTime Data { get; set; }
        public int ManualStepsCount { get; set; }
        public decimal TotalEarned { get; set; }
        public int RootCounter { get; set; }
    }
}
