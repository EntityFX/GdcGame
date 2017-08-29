using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Contract.MainServer.GameDataTransfer;
using EntityFX.Gdcame.DataAccess.Contract.Common.User;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.MainServer.DataTransferManager;
using EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData;

namespace EntityFX.Gdcame.Manager.MainServer
{
    public class DataTransferManager : IDataTransferManager
    {
        private readonly IMapper<UserDataTransfer, User> _mapper;
        private readonly IUserDataAccessService _userDataAccessService;
        private readonly IGameDataStoreDataAccessService _gameDataStoreDataAccessService;

        public DataTransferManager(IGameDataStoreDataAccessService gameDataStoreDataAccessService, IUserDataAccessService userDataAccessService, IMapper<UserDataTransfer, User> mapper)
        {
            _gameDataStoreDataAccessService = gameDataStoreDataAccessService;
            _userDataAccessService = userDataAccessService;
            _mapper = mapper;
        }

        public void SendGameData(DataTransfer[] sendedData)
        {
            foreach (var data in sendedData)
            {
                var user = this._mapper.Map(data.UserData);
                _userDataAccessService.Create(user);
            }
            _gameDataStoreDataAccessService.StoreGameDataForUsers(sendedData.Select(el=>el.GameData).ToArray());
        }
    }
}
