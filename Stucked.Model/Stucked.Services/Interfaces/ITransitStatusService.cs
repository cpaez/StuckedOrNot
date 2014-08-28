using System;
using System.Collections.Generic;
using Stucked.Model;

namespace Stucked.Services
{
    public interface ITransitStatusService
    {
        IEnumerable<Highway> GetTransitStatusForAllHighways();
        IEnumerable<Segment> GetTransitStatusForAllSegments();
        IEnumerable<HighwaySignStatus> GetTransitStatusForAllHighwaySigns();
    }
}
