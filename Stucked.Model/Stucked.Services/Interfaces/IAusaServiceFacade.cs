using Stucked.Model;
using System;
using System.Collections.Generic;

namespace Stucked.Services
{
    public interface IAusaServiceFacade
    {
        IEnumerable<TransitStatus> GetAusaTransitCurrentStatus(string seed);
    }
}
