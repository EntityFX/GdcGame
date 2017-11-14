//using EntityFX.Gdcame.Contract.MainServer.GameDataTransfer;
using EntityFX.Gdcame.Contract.MainServer.GameDataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Application.Contract.Controller.MainServer
{
    public interface IDataTransferController
    {
       Task SendDataAsync(DataTransfer[] sendedData);
    }
}
