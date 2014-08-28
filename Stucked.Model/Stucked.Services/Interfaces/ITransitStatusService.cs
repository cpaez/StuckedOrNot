using System;
using System.Collections.Generic;
using Stucked.Model;

namespace Stucked.Services
{
    public interface ITransitStatusService
    {
        IEnumerable<Segment> GetTransitStatusForAllHighways();
        IEnumerable<HighwaySignStatus> GetTransitStatusForAllHighwaySigns();
    }
}
