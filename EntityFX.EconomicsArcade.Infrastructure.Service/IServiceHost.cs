﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Infrastructure.Service
{
    public interface IServiceHost
    {
        string Name
        {
            get;
        }

        Uri Endpoint
        {
            get;
        }

        void Open(Uri endpointAddress);

        void Close();
    }
}
