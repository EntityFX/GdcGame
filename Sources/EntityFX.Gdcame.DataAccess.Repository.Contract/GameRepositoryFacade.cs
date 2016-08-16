using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract
{
    public class GameRepositoryFacade
    {
        public IUserGameSnapshotRepository UserGameSnapshotRepository { get; set; }
        public IFundsDriverRepository FundsDriverRepository { get; set; }
        public ICountersRepository CountersRepository { get; set; }
        public ICustomRuleRepository CustomRuleRepository { get; set; }
    }
}
