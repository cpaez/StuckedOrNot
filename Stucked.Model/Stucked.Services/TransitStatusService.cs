using Stucked.DataAccess;
using Stucked.Model;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;

namespace Stucked.Services
{
    public class TransitStatusService : ITransitStatusService
    {
        private StuckedContext Context = new StuckedContext();

        private string PointOfMeasureSeed = "V";
        private string SignInformationSeed = "C";

        private string DefaultSegmentColor = "green";

        public IAusaServiceFacade AusaServiceFacade { get; set; }

        public TransitStatusService()
        {
            this.AusaServiceFacade = new AusaServiceFacade();
        }

        public IEnumerable<Highway> GetHighways()
        {
            return this.Context.Highways;
        }

        public IEnumerable<Segment> GetTransitStatusForAllHighways(int highwayId)
        {
            return this.Context.GetSegmentsByHighway(highwayId);
        }

        /// <summary>
        /// Method that returns all the Segments with its current status (Smooth - Slow - Delayed - Stucked)
        /// It retrieves segments found on the DB and then calls the AUSA service to get current status for each one
        /// </summary>
        /// <returns>List of Segments</returns>
        public IEnumerable<SegmentStatus> GetTransitStatusForAllSegments()
        {
            var finalSegmentlist = new List<SegmentStatus>();
            var originalSegmentList = this.Context.Segments.ToList();

            var measurePoints = this.AusaServiceFacade.GetAusaTransitCurrentStatus(PointOfMeasureSeed);

            foreach (var segment in originalSegmentList)
            {
                var statusColor = this.DefaultSegmentColor;
                var currentStatus = measurePoints.FirstOrDefault(n => n.Key == segment.Name);

                if (currentStatus != null)
                    statusColor = this.GetColorByStatus(currentStatus.Value);

                segment.GeoJson = segment.GeoJson.Replace("\"color\": \"darkblue\"", "\"color\": \"" + statusColor + "\"");

                var segmentStatus = new SegmentStatus(segment);
                var statusColorCode = (currentStatus != null) ? currentStatus.Value : "FFFFFF";
                segmentStatus.Status = this.GetStatusByColor(statusColorCode);

                finalSegmentlist.Add(segmentStatus);
            }

            return finalSegmentlist;
        }

        /// <summary>
        /// Method that returns a list of Highway Sign Status
        /// It retrieves highways signs found on the DB and then calls the AUSA service to get current status for each one
        /// </summary>
        /// <returns>List of HighwaySignStatus</returns>
        public IEnumerable<HighwaySignStatus> GetTransitStatusForAllHighwaySigns()
        {
            var finalHighwaySignlist = new List<HighwaySignStatus>();
            var originalHighwaySignList = this.Context.HigwaySigns.ToList();

            var signInformation = this.AusaServiceFacade.GetAusaTransitCurrentStatus(SignInformationSeed);

            foreach (var highwaySign in originalHighwaySignList)
            {
                var highwaySignStatus = new HighwaySignStatus(highwaySign);

                var statusColor = this.DefaultSegmentColor;
                var currentStatus = signInformation.Where(n => n.Key.StartsWith(highwaySign.Name));

                var signMessage = "<br \"/>";
                foreach (var status in currentStatus)
                {
                    if (status.Value.Length > 3)
                        signMessage += status.Value + "<br \"/>";
                }
                highwaySignStatus.Status = signMessage;

                finalHighwaySignlist.Add(highwaySignStatus);
            }

            return finalHighwaySignlist;
        }

        #region Protected Members

        protected string GetColorByStatus(string status)
        {
            var smoothTransit = ConfigurationManager.AppSettings["SmoothTransit"];
            var slowTransit = ConfigurationManager.AppSettings["SlowTransit"];
            var delayedTransit = ConfigurationManager.AppSettings["DelayedTransit"];
            var stuckedTransit = ConfigurationManager.AppSettings["StuckedTransit"];

            var color = smoothTransit;

            switch (status)
            {
                case "FFFFFF":
                    color = smoothTransit;
                    break;
                case "008000":
                    color = slowTransit;
                    break;
                case "FFFF00":
                    color = delayedTransit;
                    break;
                case "FF0000":
                    color = stuckedTransit;
                    break;
                default:
                    color = smoothTransit;
                    break;
            }

            return color;
        }

        protected string GetStatusByColor(string color)
        {
            var smoothTransit = ConfigurationManager.AppSettings["Smooth"];
            var slowTransit = ConfigurationManager.AppSettings["Slow"];
            var delayedTransit = ConfigurationManager.AppSettings["Delayed"];
            var stuckedTransit = ConfigurationManager.AppSettings["Stucked"];

            var status = smoothTransit;

            switch (color)
            {
                case "FFFFFF":
                    status = smoothTransit;
                    break;
                case "008000":
                    status = slowTransit;
                    break;
                case "FFFF00":
                    status = delayedTransit;
                    break;
                case "FF0000":
                    status = stuckedTransit;
                    break;
                default:
                    status = smoothTransit;
                    break;
            }

            return status;
        }
        
        #endregion Protected Members
    }
}
