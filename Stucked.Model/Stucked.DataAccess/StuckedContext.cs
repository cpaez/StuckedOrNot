using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stucked.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Stucked.DataAccess
{
    public class StuckedContext : DbContext
    {
        public StuckedContext() : base("StuckedContext")
        {
        }

        public DbSet<Segment> Segments { get; set; }
        public DbSet<HighwaySign> HigwaySigns { get; set; }
    }
}
