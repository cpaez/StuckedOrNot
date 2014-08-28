using Stucked.Model;
using Stucked.Services;
using System.Collections.Generic;
using System.Web.Http;

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
    }
}
