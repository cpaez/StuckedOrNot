using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Stucked.Model;
using Stucked.Services;

namespace Stucked.API.Controllers
{
    public class HighwaySignController : ApiController
    {
        public ITransitStatusService TransitStatusService;

        public HighwaySignController(ITransitStatusService transitStatusService)
        {
            this.TransitStatusService = transitStatusService;
        }

        // GET api/highwaysign
        public IEnumerable<HighwaySignStatus> Get()
        {
            return this.TransitStatusService.GetTransitStatusForAllHighwaySigns();
        }

        // GET api/highwaysign/5
        public string Get(int id)
        {
            return "value";
        }
    }
}
