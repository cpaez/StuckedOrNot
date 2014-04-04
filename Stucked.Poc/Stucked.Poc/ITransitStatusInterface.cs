using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stucked.Poc
{
    public interface ITransitStatusInterface
    {
        List<TransitStatus> CheckStatus();
        List<TransitStatus> CheckSpecificHighway();
    }
}
