using System;
using System.Collections.Generic;
using System.Linq;

namespace Stucked.Model
{
    public class HighwaySign
    {
        public int HighwaySignID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SnapToRoads { get; set; }
        public string GeoJson { get; set; }
        public string Direction { get; set; }
        public string Location { get; set; }
    }

    public class HighwaySignStatus
    {
        public HighwaySignStatus(HighwaySign highwaySign)
        {
            this.HighwaySign = highwaySign;
        }

        public HighwaySign HighwaySign { get; set; }
        public string Status { get; set; }
    }
}
