using Stucked.Model;
using System.Data.Entity;

namespace Stucked.DataAccess
{
    public class StuckedContext : DbContext
    {
        public StuckedContext() : base("StuckedContext")
        {
        }

        public DbSet<Highway> Highways { get; set; }
        public DbSet<Segment> Segments { get; set; }
        public DbSet<HighwaySign> HigwaySigns { get; set; }
    }
}
