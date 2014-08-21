using System;
using System.Collections.Generic;
using System.Linq;
using Stucked.DataAccess;
using Stucked.Model;
using System.Configuration;
using System.Net;
using System.IO;

namespace Stucked.Services
{
    public class TransitStatusService
    {
        private StuckedContext Context = new StuckedContext();

        private string PointOfMeasureSeed = "V";
        private string SignInformationSeed = "C";

        private string DefaultHighwayColor = "green";
        private string AusaServiceUrl = ConfigurationManager.AppSettings["AusaServiceUrl"];

        /// <summary>
        /// Method that returns all the Highways with its current status (Smooth - Slow - Delayed - Stucked)
        /// It retrieves highways found on the DB and then calls the AUSA service to get current status for each one
        /// </summary>
        /// <returns>List of Highways</returns>
        public IEnumerable<Highway> GetTransitStatusForAllHighways()
        {
            var finalHighwaylist = new List<Highway>();
            var originalHighwayList = this.Context.Highways.ToList();

            var measurePoints = this.GetTransitCurrentStatus(PointOfMeasureSeed);

            foreach (var highway in originalHighwayList)
            {
                var statusColor = this.DefaultHighwayColor;
                var currentStatus = measurePoints.FirstOrDefault(n => n.Key == highway.Name);

                if (currentStatus != null)
                    statusColor = this.GetColorByStatus(currentStatus.Value);

                highway.GeoJson = highway.GeoJson.Replace("\"color\": \"darkblue\"", "\"color\": \"" + statusColor + "\"");

                finalHighwaylist.Add(highway);
            }

            return finalHighwaylist;
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

            var signInformation = this.GetTransitCurrentStatus(SignInformationSeed);

            foreach (var highwaySign in originalHighwaySignList)
            {
                var highwaySignStatus = new HighwaySignStatus(highwaySign);

                var statusColor = this.DefaultHighwayColor;
                var currentStatus = signInformation.Where(n => n.Key.StartsWith(highwaySign.Name));

                var signMessage = string.Empty;
                foreach (var status in currentStatus)
                {
                    signMessage += status.Value + ' ';
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

        protected IEnumerable<TransitStatus> GetTransitCurrentStatus(string seed)
        {
            var remoteFile = string.Empty;
            var webRequest = WebRequest.Create(@AusaServiceUrl);

            using (var response = webRequest.GetResponse())
            using (var content = response.GetResponseStream())
            using (var reader = new StreamReader(content))
            {
                remoteFile = reader.ReadToEnd();
            }

            char[] delimiterChars = { '&' };
            string[] rawStatusList = remoteFile.Split(delimiterChars);

            char[] delimiterChars2 = { '=' };
            var statusList = new List<TransitStatus>();

            for (int i = 0; i < rawStatusList.Length - 1; i++)
            {
                string[] kv = rawStatusList[i].Split(delimiterChars2);

                var item = this.GetStatusItem(kv, seed);

                if (item != null)
                    statusList.Add(item);
            }

            return statusList;
        }

        protected TransitStatus GetStatusItem(string[] kv, string seed)
        {
            TransitStatus statusItem = null;

            if (kv.Length == 2)
            {
                if (kv[0].StartsWith(seed))
                {
                    statusItem = new TransitStatus(kv[0], kv[1]);   
                }
            }

            return statusItem;
        }

        #endregion Protected Members
    }
}
