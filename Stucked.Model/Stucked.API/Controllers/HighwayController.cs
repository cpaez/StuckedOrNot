using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Stucked.Model;
using Stucked.DataAccess;
using Stucked.Services;

namespace Stucked.API.Controllers
{
    public class HighwayController : ApiController
    {
        public ITransitStatusService TransitStatusService;

        public HighwayController()
        { }

        public HighwayController(ITransitStatusService transitStatusService)
        {
            this.TransitStatusService = transitStatusService;
        }

        // GET api/sign
        public IEnumerable<Highway> Get()
        {
            return this.TransitStatusService.GetTransitStatusForAllHighways();
        }

        // GET api/sign/5
        public string Get(int id)
        {
            return "value";
        }
    }
}
