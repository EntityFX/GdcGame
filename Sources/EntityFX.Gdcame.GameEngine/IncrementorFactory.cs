using EntityFX.Gdcame.GameEngine.Contract.Incrementors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.GameEngine
{
    public class IncrementorFactory
    {
        public static TIncrementor Build<TIncrementor>(int value)
            where TIncrementor : IncrementorBase, new()
        {
            var incrementor = new TIncrementor();
            incrementor.Init(value);
            return incrementor;

        }
    }
}
