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

        private string DefaultHighwayColor = "green";
        string AusaServiceUrl = ConfigurationManager.AppSettings["AusaServiceUrl"];

        public IEnumerable<Highway> GetTransitStatusForAllHighways()
        {
            var finalHighwaylist = new List<Highway>();
            var originalHighwayList = this.Context.Highways.ToList();

            var measurePoints = this.GetPointsOfMeasureWithCurrentStatus();

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

        protected IEnumerable<TransitStatus> GetPointsOfMeasureWithCurrentStatus()
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

                var item = this.GetStatusItem(kv);

                if (item != null)
                    statusList.Add(item);
            }

            return statusList;
        }

        protected TransitStatus GetStatusItem(string[] kv)
        {
            TransitStatus statusItem = null;

            if (kv.Length == 2)
            {
                if (kv[0].StartsWith("V"))
                {
                    statusItem = new TransitStatus(kv[0], kv[1]);   
                }
            }

            return statusItem;
        }

        #endregion Protected Members
    }
}
