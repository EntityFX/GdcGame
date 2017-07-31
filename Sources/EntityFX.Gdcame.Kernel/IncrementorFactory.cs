namespace EntityFX.Gdcame.Kernel
{
    using EntityFX.Gdcame.Kernel.Contract.Incrementors;

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
