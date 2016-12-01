using EntityFX.Gdcame.Application.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Application.Contract.Controller
{
    public interface IRatingController
    {
        Task<RatingStatisticsModel[]> GetRaiting(int top = 500);
    }
}
