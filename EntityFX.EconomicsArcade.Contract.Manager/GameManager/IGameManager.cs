using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Contract.Manager.GameManager
{
    public interface IGameManager
    {
        void BuyFundDriver(int fundDriverId);

        void PerformManualStep();

        void FightAgainstInflation();

        void PlayLottery();

        void GetCounters();

        void GetGameData();
    }
}
