using Stucked.Model;
using Stucked.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Runtime.Caching;

namespace Stucked.Services
{
    public class AusaServiceFacade : IAusaServiceFacade
    {
        private string AusaServiceUrl = ConfigurationManager.AppSettings["AusaServiceUrl"];
        private MemoryCache cache = MemoryCache.Default;

        public IEnumerable<TransitStatus> GetAusaTransitCurrentStatus(string seed)
        {
            object webServiceResult;
            var cachedObject = cache.Get("AusaCachedStatus");

            if (cachedObject == null)
            {
                // Call the web service
                webServiceResult = this.GetRealAusaTransitCurrentStatus(seed);
                cache.Add("AusaCachedStatus", webServiceResult, DateTime.Now.AddMinutes(5));
            }
            else
            {
                webServiceResult = cachedObject;
            }

            return webServiceResult as IEnumerable<TransitStatus>;
        }

        protected IEnumerable<TransitStatus> GetRealAusaTransitCurrentStatus(string seed)
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
    }
}
