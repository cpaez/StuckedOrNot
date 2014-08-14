﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stucked.Model
{
    public class MeasurePoints
    {
        public MeasurePoints()
        { }

        public MeasurePoints(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}
