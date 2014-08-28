using Stucked.Model;
using Stucked.Services;
using System.Collections.Generic;
using System.Web.Http;

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

        // GET api/highway
        public IEnumerable<Highway> Get()
        {
            return this.TransitStatusService.GetTransitStatusForAllHighways();
        }
    }
}
