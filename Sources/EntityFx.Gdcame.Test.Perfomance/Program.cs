using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using CommandLine;
using EntityFx.Gdcame.Test.Perfomance.Tests;

namespace EntityFx.Gdcame.Test.Perfomance
{
    class Program
    {
        private static Dictionary<int, ITest> Tests = new Dictionary<int, ITest>()
        {
            {0, new EchoTest()},
            {1, new EchoAuthTest()},
        };

        static void Main(string[] args)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 15000;
            //ThreadPool.SetMaxThreads(2000, 2000);
            var cmdOptions = new CmdOptions();
            Parser.Default.ParseArguments<TestsListSubOption, RunTestSubOptions>(args)
                .MapResult(
                    (TestsListSubOption tl) => DisplayTestList(tl),
                    (RunTestSubOptions t) => RunTests(t), errors => 1
                );
        }

        private static object RunTests(RunTestSubOptions runTestSubOptions)
        {
            if (Tests.ContainsKey(runTestSubOptions.TestNumber))
            {
                Tests[runTestSubOptions.TestNumber].Run(runTestSubOptions);
            }
            return 0;
        }

        private static object DisplayTestList(TestsListSubOption tl)
        {
            foreach (var test in Tests)
            {
                Console.WriteLine("{0}: {1}", test.Key, test.Value.Name);
            }
            return null;
        }
    }
}
