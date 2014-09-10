namespace Stucked.Model
{
    public class Segment
    {
        public int SegmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GeoJson { get; set; }
        public int SegmentCode { get; set; }
        public string NameStart { get; set; }
        public string NameEnd { get; set; }
        public string Detail { get; set; }
        public int HighwayId { get; set; }
    }

    public class SegmentStatus
    {
        public SegmentStatus(Segment segment)
        {
            this.Segment = segment;
        }

        public Segment Segment { get; set; }
        public string Status { get; set; }
    }
}
