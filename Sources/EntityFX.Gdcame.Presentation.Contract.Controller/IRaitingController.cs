using EntityFX.Gdcame.Application.Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Application.Model;

namespace EntityFX.Gdcame.Application.Contract.Controller
{
    public interface IRatingController
    {
        Task<TopRatingStatisticsModel> GetRaiting(int top = 500);
    }
}
