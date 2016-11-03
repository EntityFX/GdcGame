using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using CommandLine;

namespace EntityFx.Gdcame.Test.Perfomance
{
    public class CmdOptions
    {
        [Option('l', "tests-list", HelpText = "Prints list of available tests", Required = true)]
        public bool TestList { get; set; }

        [Option('t', "test", HelpText = "Test number", Required = true)]
        public int TestNumber { get; set; }


    }
    [Verb("tests-list", HelpText = "Prints list of available tests")]
    public class TestsListSubOption
    {
        
    }

    [Verb("run-test", HelpText = "Prints list of available tests")]
    public class RunTestSubOptions
    {
        [Value(0, Required = true, HelpText = "The test number", MetaName = "Test Number", MetaValue = "1")]
        public int TestNumber { get; set; }

        [Option('p', "port", Default = 9001, HelpText = "Api port", Required = false)]
        public int Port { get; set; }

        [Option('c', "concurrent", Default = false, HelpText = "Specifies concurrent requests", Required = false)]
        public bool Concurrent { get; set; }

        [Option('r', "requests-count", Default = 10000, HelpText = "Requests count")]
        public int RequestsCount { get; set; }

        [Option('s', "servers", HelpText = "List of api servers. If not specified config file will be used.", Required = false)]
        public IEnumerable<string> ServersList { get; set; }

        [Option("admin-login", Default = "admin", HelpText = "Admin password", Required = false)]
        public string AdminLogin { get; set; }

        [Option("admin-password", Default = "P@ssw0rd", HelpText = "Admin password", Required = false)]
        public string AdminPassword { get; set; }
    }
}