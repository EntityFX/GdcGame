using EntityFx.Gdcame.Test.PerfomanceFramework;

namespace EntityFx.Gdcame.Test.Perfomance.Tests
{
    interface ITest
    {
        TestResultInfo Run(RunTestSubOptions options);

        string Name { get; }
    }
}