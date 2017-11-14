using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EntityFx.Gdcame.Test.PerfomanceFramework;
using EntityFX.Gdcame.Infrastructure;
using EntityFX.Gdcame.Infrastructure.Common;
using Newtonsoft.Json;

namespace EntityFx.Gdcame.Test.Perfomance.Tests
{
    public abstract class TestBase : ITest
    {
        private static Random random = new Random();

        private static readonly Logger _logger = new Logger(new NLoggerAdapter(NLog.LogManager.GetLogger("logger")));

        public static Logger Logger
        {
            get { return _logger; }
        }

        public TestResultInfo Run(RunTestSubOptions options)
        {
            if (!Directory.Exists(GetSaveFilePath()))
            {
                Directory.CreateDirectory(GetSaveFilePath());
            }

            var res = new TestResultInfo()
            {
                TestName = Name,
                TestActionResults = RunImplementation(options)
            };
            SaveResultsToJson(res);
            return res;
        }

        protected abstract TestActionResultItem[] RunImplementation(RunTestSubOptions options);

        public string Name { get; protected set; }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        protected Uri[] GetServers(RunTestSubOptions options)
        {
            return options.ServersList != null && options.ServersList.Any()
                ? options.ServersList.Select(_ => new Uri("http://" + _ + ":" + options.Port)).ToArray()
                : GetServers().Select(_ => new Uri("http://" + _ + ":" + options.Port)).ToArray();
        }

        protected string[] GetServers()
        {
            if (!File.Exists("servers.json"))
            {
                return null;
            }
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText("servers.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.TypeNameHandling = TypeNameHandling.Auto;
                return (string[])serializer.Deserialize(file, typeof(string[]));
            }
        }

        protected TestActionResultItem GetTestActionResultItem(string actionName,
    PerformanceAggregate performanceAggregate, int requestsCount)
        {
            return new TestActionResultItem
            {

                ElapsedMilliseconds = performanceAggregate.TotalElapsed.Milliseconds,

                ActionName = actionName,
                ActionDescription = string.Format("Tested with {0} requests", requestsCount),
                PerformanceCounters = performanceAggregate.PerformanceCounters,
                PerformanceStatisticsByServer = performanceAggregate.PerformanceStatisticsByServer.Select(
                    _ => new KeyValuePair<string, TestStatistics>(_.Key, new TestStatistics()
                    {
                        Min = _.Value.Min,
                        Max = _.Value.Max,
                        AvgMilliseconds = _.Value.AvgMilliSeconds,
                        Elapsed = _.Value.TotalElapsed,
                        CountErrors = _.Value.CountErrors
                    })).ToDictionary(_ => _.Key, _ => _.Value),
                TestStatistics = new TestStatistics()
                {
                    Min = performanceAggregate.Min,
                    Max = performanceAggregate.Max,
                    AvgMilliseconds = performanceAggregate.AvgMilliSeconds,
                    Elapsed = performanceAggregate.TotalElapsed,
                    CountErrors = performanceAggregate.CountErrors
                }
            };
        }

        private void SaveResultsToJson(TestResultInfo testResultInfo)
        {
            using (StreamWriter file = File.CreateText(GetSaveFilePath(string.Format("{0}-{1:yyyyMMdd_HHmmss}.json", testResultInfo.TestName, DateTime.Now))))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, testResultInfo);
            }
        }

        private string GetSaveFilePath(string fileName = null)
        {
            return fileName != null ? Path.Combine("results", fileName) : Path.Combine("results");
        }
    }
}