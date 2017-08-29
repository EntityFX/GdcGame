namespace EntityFX.Gdcame.Engine.Worker.MainServer
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using EntityFX.Gdcame.DataAccess.Contract.Common.Server;
    using EntityFX.Gdcame.DataAccess.Contract.Common.User;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;
    using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;
    using EntityFX.Gdcame.Contract.MainServer.Store;
    using DataAccess.Contract.MainServer.GameData;
    using System.Linq;
    using Infrastructure.Common;
    using System.Collections.Generic;
    using Kernel.Contract;

    public class NodeDataTransferWorker : WorkerBase
    {
        private readonly IServerDataAccessService serverDataAccessService;

        private readonly IUserDataAccessService userDataAccessService;

        private readonly IGameDataRetrieveDataAccessService gameDataRetrieveDataAccessService;

        private readonly IGameSessions gameSessions;

        private readonly IHashHelper hashHelper;

        private readonly IMapper<IGame, StoredGameData> gameMapper;

        /// <summary>
        /// The _data transfer task.
        /// </summary>
        private Task _dataTransferTask;
        private readonly CancellationTokenSource cancellationTaskToken = new CancellationTokenSource();

        public NodeDataTransferWorker(IServerDataAccessService serverDataAccessService, IUserDataAccessService userDataAccessService, IGameSessions gameSessions, IGameDataRetrieveDataAccessService gameDataRetrieveDataAccessService, IHashHelper hashHelper, IMapper<IGame, StoredGameData> gameMapper)
        {
            this.serverDataAccessService = serverDataAccessService;
            this.userDataAccessService = userDataAccessService;
            this.gameSessions = gameSessions;
            this.gameDataRetrieveDataAccessService = gameDataRetrieveDataAccessService;
            this.hashHelper = hashHelper;
            this.gameMapper = gameMapper;
            this.Name = "NodeDataTransferWorker";
        }

        public void TransferDataOnServer(string[] servers, string[] userIds)
        {
            var userSavedData = this.gameDataRetrieveDataAccessService.GetStoredGameData(userIds);
        }

        public override void Run<TData>(TData data = default(TData))
        {
            this._dataTransferTask =
                Task.Factory.StartNew(
                    a => this.PerformBackgroundPersisting(data as string[]),
                    TaskCreationOptions.LongRunning,
                    this.cancellationTaskToken.Token).ContinueWith(
                    c =>
                        {
                            if (c.IsFaulted)
                            {
                                //Log.Error(c.Exception.InnerException);
                            }
                        });
        }

        private void PerformBackgroundPersisting(string[] servers)
        {
            this.IncrementTick();

            serverDataAccessService.UpdateServers(servers);

            //foreach (var gamesKey in this.gameSessions.Games.Keys)
            //{
            //    this.gameSessions.FreezeUserGame(gamesKey, "127.0.0.1");
            //}

            int offset = 0;
            int size = 100;

            Dictionary<string, List<User>> serversDataDictionary = new Dictionary<string, List<User>>();

            for (int i = 0; i < servers.Length; i++)
            {
                serversDataDictionary.Add(servers[i], new List<User>());
            }

            while (true)
            {
                var users = userDataAccessService.FindChunked(offset, size);
                if (users.Count() == 0) break;
                foreach (var user in users)
                {
                    int newServerId = hashHelper.GetServerNumberByUserId(servers, user.Id);
                    serversDataDictionary[servers[newServerId]].Add(user);
                }
                offset += 100;
                List<User> usersForSend = new List<User>();
                foreach (var server in serversDataDictionary)
                {
                    if (server.Key == "127.0.0.1") continue;
                    foreach (var user in server.Value)
                    {
                        this.gameSessions.FreezeUserGame(user.Login, server.Key);
                        
                    }
                    usersForSend.AddRange(server.Value);        //users that should be sent to new servers
                }

                var usersSavedData = this.gameDataRetrieveDataAccessService.GetStoredGameData(usersForSend.Select(u => u.Id).ToArray());        //saved games for users for send

                foreach (var game in gameSessions.Games)        //update saved games data with active games
                {
                    var activeUser = usersForSend.Where(user => user.Login == game.Key).SingleOrDefault();
                    if(activeUser!=null)
                    {
                        var newStoredDataForUser = gameMapper.Map(game.Value);      // IGame - > StoredGameData
                        usersSavedData.Where(data => data.UserId == activeUser.Id).SingleOrDefault().StoredGameData = newStoredDataForUser;     //replace old saved data to new data
                    }
                }
                //send data to servers
            }
            
        }

        public override bool IsRunOnStart
        {
            get
            {
                return false;
            }
        }

        public override bool IsRunning
        {
            get
            {
                return this._dataTransferTask != null && (this._dataTransferTask.Status == TaskStatus.Running);
            }
        }
    }
}
