using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Contract.Manager.UserManager;
using EntityFx.EconomicsArcade.Test.Shared;
using Microsoft.Practices.Unity;

namespace EntityFX.EconomicsArcade.Test.MultiClientManagerTest
{
    class Program
    {
        private static IUnityContainer _container;

        private static ConcurrentDictionary<string, Guid> _testUsersDisctionary = new ConcurrentDictionary<string, Guid>();

        private static void Main(string[] args)
        {
            var isCollapsed = false;
            if (args.Length > 0)
            {
                isCollapsed = args[0].Contains("IsCollapsed");
            }
            _container = new UnityContainer();
            var containerBootstrapper = new ContainerBootstrapper(isCollapsed).Configure(_container);



            foreach (var users in _testUsersDisctionary)
            {
                Console.WriteLine(users);
            }
            var sw = new Stopwatch();

        //    new[]{
        //    1,
        //    4,
        //    16,
        //    64,
        //    256
        //}.ToList().ForEach(t =>
        //{

        //    Parallel.For(0, t, _ =>
        //    {
        //        var userName = "multi-test-user-" + _;
        //        _testUsersDisctionary.TryAdd(userName, UserLogin(userName).Item1);
        //    });
        //    sw.Start();
        //    Parallel.ForEach(_testUsersDisctionary, (pair, state) => Parallel.For(0, 50, _ => GetGameClient(pair.Value).PerformManualStep(null)));
        //    sw.Stop();
        //    Console.WriteLine("{0}: {1}", t, sw.Elapsed);
        //    Parallel.ForEach(_testUsersDisctionary, (pair, state) =>
        //    {
        //        UserLogout(pair.Value);
        //        Guid val;
        //        _testUsersDisctionary.TryRemove(pair.Key, out val);
        //    });
        //});

        //    List<long> watches = new List<long>();
        //    new[]{
        //    1,
        //    4,
        //    16,
        //    64,
        //    256, 1024, 2048, 4096
        //}.ToList().ForEach(t =>
        //{

        //    Parallel.For(0, t, _ =>
        //    {
        //        var userName = "multi-test-user-" + _;
        //        _testUsersDisctionary.TryAdd(userName, UserLogin(userName).Item1);
        //    });
        //    Parallel.ForEach(_testUsersDisctionary, (pair, state) => Parallel.For(0, 50, _ =>
        //    {
        //                    var sw1 = new Stopwatch();
        //    sw1.Start();
        //        GetGameClient(pair.Value).PerformManualStep(null);
        //        watches.Add(sw1.ElapsedMilliseconds);
        //                    sw1.Stop();

        //    }));

        //    Console.WriteLine("{0}: Avg={1}, Min={2}, Max={3}", t, watches.Average(), watches.Min(), watches.Max());
        //    Parallel.ForEach(_testUsersDisctionary, (pair, state) =>
        //    {
        //        UserLogout(pair.Value);
        //        Guid val;
        //        _testUsersDisctionary.TryRemove(pair.Key, out val);
        //    });
        //    watches.Clear();
        //});

            new[]{
            1,
            4,
            16,
            64,
            256
        }.ToList().ForEach(t =>
        {

            Parallel.For(0, t, _ =>
            {
                var userName = "multi-test-user-" + _;
                _testUsersDisctionary.TryAdd(userName, UserLogin(userName).Item1);
            });
            sw.Start();
            Parallel.ForEach(_testUsersDisctionary, (pair, state) => Parallel.For(0, 50, _ => GetGameClient(pair.Value).Ping()));
            sw.Stop();
            Console.WriteLine("{0}: {1}", t, sw.Elapsed);
            Parallel.ForEach(_testUsersDisctionary, (pair, state) =>
            {
                UserLogout(pair.Value);
                Guid val;
                _testUsersDisctionary.TryRemove(pair.Key, out val);
            });
        });

            List<long> watches = new List<long>();
            new[]{
            1,
            4,
            16,
            64,
            256, 1024, 2048, 4096
        }.ToList().ForEach(t =>
        {

            Parallel.For(0, t, _ =>
            {
                var userName = "multi-test-user-" + _;
                _testUsersDisctionary.TryAdd(userName, UserLogin(userName).Item1);
            });
            Parallel.ForEach(_testUsersDisctionary, (pair, state) => Parallel.For(0, 50, _ =>
            {
                var sw1 = new Stopwatch();
                sw1.Start();
                GetGameClient(pair.Value).Ping();
                watches.Add(sw1.ElapsedMilliseconds);
                sw1.Stop();

            }));

            Console.WriteLine("{0}: Avg={1}, Min={2}, Max={3}", t, watches.Average(), watches.Min(), watches.Max());
            Parallel.ForEach(_testUsersDisctionary, (pair, state) =>
            {
                UserLogout(pair.Value);
                Guid val;
                _testUsersDisctionary.TryRemove(pair.Key, out val);
            });
            watches.Clear();
        });


            Console.ReadKey();
        }


        private static IGameManager GetGameClient(Guid sessionGuid)
        {
            return _container.Resolve<IGameManager>(new ParameterOverride("sesionGuid", sessionGuid));
        }

        private static Tuple<Guid, string> UserLogin(string userName)
        {
            var simpleUserManagerClient = _container.Resolve<ISimpleUserManager>();
            if (!simpleUserManagerClient.Exists(userName))
            {
                simpleUserManagerClient.Create(new UserData(){Login = userName});
            }

            var sessionManagerClient = _container.Resolve<ISessionManager>(new ParameterOverride("sessionGuid", Guid.Empty));
            return new Tuple<Guid, string>(sessionManagerClient.OpenSession(userName), userName);
        }

        private static void UserLogout(Guid session)
        {
            var sessionManagerClient = _container.Resolve<ISessionManager>(new ParameterOverride("sessionGuid", Guid.Empty));
            sessionManagerClient.CloseSession();
        }
    }
}
