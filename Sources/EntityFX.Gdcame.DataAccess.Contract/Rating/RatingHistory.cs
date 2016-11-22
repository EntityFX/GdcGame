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
        string UserID { get; set;}
        DateTime Data { get; set;}
        int ManualStepsCount { get; set;}
        decimal TotalEarned { get; set; }
        int RootCounter { get; set; }
    }
}
