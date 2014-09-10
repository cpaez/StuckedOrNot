using Stucked.Model;
using System.Collections.Generic;

namespace Stucked.Services
{
    public interface ITransitStatusService
    {
        IEnumerable<Highway> GetHighways();
        IEnumerable<Segment> GetTransitStatusForAllHighways(int highwayId);
        IEnumerable<SegmentStatus> GetTransitStatusForAllSegments();
        IEnumerable<HighwaySignStatus> GetTransitStatusForAllHighwaySigns();
    }
}
