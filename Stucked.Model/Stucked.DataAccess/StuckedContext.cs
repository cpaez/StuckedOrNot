using Stucked.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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

        /// <summary>
        ///  Returns all the Segments filtered by a given Highway
        /// </summary>
        /// <param name="highwayId">Represents the Highway Id to look into segments</param>
        /// <returns>List of Segments</returns>
        public IEnumerable<Segment> GetSegmentsByHighway(int highwayId)
        {
            return this.Segments.Where(s => s.HighwayId == highwayId).ToList();
        }
    }
}
