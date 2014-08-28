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
    public class SegmentController : ApiController
    {
        public ITransitStatusService TransitStatusService;

        public SegmentController()
        { }

        public SegmentController(ITransitStatusService transitStatusService)
        {
            this.TransitStatusService = transitStatusService;
        }

        // GET api/highway
        public IEnumerable<Segment> Get()
        {
            return this.TransitStatusService.GetTransitStatusForAllSegments();
        }
    }
}
