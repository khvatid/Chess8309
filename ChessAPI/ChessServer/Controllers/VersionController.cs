﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChessServer.Controllers
{
    public class VersionController : ApiController
    {
        public string GetVersion()
        {
            return "0.1";
        }
    }
}
