﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stucked.Model
{
    public class HighwaySign
    {
        public int HighwaySignId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SnapToRoads { get; set; }
        public string GeoJson { get; set; }
        public int HighwayId { get; set; }
    }
}
