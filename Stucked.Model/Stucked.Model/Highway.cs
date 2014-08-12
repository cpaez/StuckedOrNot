using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stucked.Model
{
    public class Highway
    {
        public int HighwayId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GeoJson { get; set; }
    }
}
