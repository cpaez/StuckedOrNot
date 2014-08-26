using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Stucked.Infrastructure;

namespace Stucked.API.Controllers
{
    public class BaseController : ApiController
    {
        protected static readonly ILogger _logger = LogManager.GetLogger(typeof(CustomLogger));
    }
}
