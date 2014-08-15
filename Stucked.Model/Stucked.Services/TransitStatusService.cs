﻿using System;
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
        string AusaServiceUrl = ConfigurationManager.AppSettings["AusaServiceUrl"];

        public IEnumerable<Highway> GetTransitStatusForAllHighways()
        {
            var finalHighwaylist = new List<Highway>();
            var originalHighwayList = this.Context.Highways.ToList();

            var measurePoints = this.GetPointsOfMeasureWithCurrentStatus();

            foreach (var highway in originalHighwayList)
            {
                var currentStatus = measurePoints.FirstOrDefault(n => n.Key == highway.Name);
                var statusColor = "white";

                if (currentStatus != null)
	            {
                    statusColor = this.GetColorByStatus(currentStatus.Value);
                }

                highway.GeoJson = highway.GeoJson.Replace("\"color\": \"darkblue\"", "\"color\": \"" + statusColor + "\"");
                finalHighwaylist.Add(highway);

            }

            return finalHighwaylist;
        }

        protected string GetColorByStatus(string status)
        {
            var color = "white";

            switch (status)
            {
                case "FFFFFF":
                    color = "green";
                    break;
                case "008000":
                    color = "yellow";
                    break;
                case "FFFF00":
                    color = "orange";
                    break;
                case "FF0000":
                    color = "red";
                    break;
                default:
                    color = "white";
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
    }
}