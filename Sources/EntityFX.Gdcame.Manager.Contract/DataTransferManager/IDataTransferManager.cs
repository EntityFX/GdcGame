using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Contract.MainServer.GameDataTransfer;

namespace EntityFX.Gdcame.Manager.Contract.MainServer.DataTransferManager
{
    public interface IDataTransferManager
    {
        void SendGameData(DataTransfer [] sendedData);
    }
}
