using Stucked.Model;
using Stucked.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace Stucked.API.Controllers
{
    public class SegmentController : ApiController
    {
        public ITransitStatusService TransitStatusService;

        public SegmentController()
        { }

        public SegmentController(ITransitStatusService transitStatusService)
        {
            this.TransitStatusService = transitStatusService;
        }

        // GET api/segment
        public IEnumerable<Segment> Get()
        {
            return this.TransitStatusService.GetTransitStatusForAllSegments();
        }

        // GET api/segment
        public IEnumerable<Segment> Get(int id)
        {
            return this.TransitStatusService.GetTransitStatusForAllHighways(id);
        }
    }
}
