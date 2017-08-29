using EntityFX.Gdcame.Contract.MainServer.GameDataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using EntityFX.Gdcame.Application.Contract.Controller.MainServer;
using EntityFX.Gdcame.Manager.Contract.MainServer.DataTransferManager;

namespace EntityFX.Gdcame.Application.Api.Controller.MainServer
{
    [Authorize]
    [RoutePrefix("api/transfer")]
    public class DataTransferController : ApiController, IDataTransferController
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
