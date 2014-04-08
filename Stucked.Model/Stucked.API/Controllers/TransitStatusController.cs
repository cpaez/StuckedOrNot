using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Stucked.Model;
using System.IO;
using System.Configuration;

namespace Stucked.API.Controllers
{
    public class TransitStatusController : ApiController
    {
        string serviceUrl = ConfigurationManager.AppSettings["AusaServiceUrl"];

        // GET api/transitstatus
        public IEnumerable<TransitStatus> Get()
        {
            try
            {
                var remoteFile = string.Empty;
                var webRequest = WebRequest.Create(@serviceUrl);

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

                    if(item != null)
                        statusList.Add(item);
                }

                return statusList;
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("Error getting the remote file from AUSA. {0}", e.Message));
            }
        }

        // GET api/transitstatus/5
        public IEnumerable<TransitStatus> Get(int id)
        {
            return null;
        }

        private TransitStatus GetStatusItem(string[] kv)
        {
            TransitStatus statusItem = null;

            if (kv.Length == 2)
            {
                statusItem = new TransitStatus(kv[0], kv[1]);
            }

            return statusItem;
        }
    }
}