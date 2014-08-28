using Stucked.Model;
using System.Collections.Generic;

namespace Stucked.Services
{
    public interface ITransitStatusService
    {
        IEnumerable<Highway> GetTransitStatusForAllHighways();
        IEnumerable<Segment> GetTransitStatusForAllSegments();
        IEnumerable<HighwaySignStatus> GetTransitStatusForAllHighwaySigns();
    }
}
