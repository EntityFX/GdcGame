using EntityFX.Gdcame.Contract.MainServer.GameDataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Controller.MainServer;
using EntityFX.Gdcame.Manager.Contract.MainServer.DataTransferManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntityFX.Gdcame.Application.Api.Controller.MainServer
{
    [Authorize]
    [Route("api/transfer")]
    public class DataTransferController : Microsoft.AspNetCore.Mvc.Controller, IDataTransferController
    {
        private readonly IDataTransferManager _dataTransferManager;

        public DataTransferController(IDataTransferManager dataTransferManager)
        {
            _dataTransferManager = dataTransferManager;
        }

        [HttpGet]
        [Route("send-data")]
        public async Task SendDataAsync([FromBody] DataTransfer[] sendedData)
        {
            await Task.Factory.StartNew(() =>
                 _dataTransferManager.SendGameData(sendedData));
        }
    }
}
