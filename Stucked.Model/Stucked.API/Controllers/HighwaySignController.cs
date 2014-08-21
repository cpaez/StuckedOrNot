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
    public class HighwaySignController : ApiController
    {
        public StuckedContext Context { get; set; }
        public TransitStatusService TransitStatusService { get; set; }

        public HighwaySignController()
        {
            this.Context = new StuckedContext();
            this.TransitStatusService = new TransitStatusService();
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
