﻿using System.Web.Http;
using System.Web.Http.Controllers;

namespace EntityFX.Gdcame.Presentation.Web.Api.Controllers
{
    public class HeartbeatController : ApiController
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
        }

        // Get api/Heartbeat
        public string Get()
        {
            return "value";
        }
    }
}