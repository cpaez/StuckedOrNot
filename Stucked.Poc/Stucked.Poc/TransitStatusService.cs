using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Stucked.Poc
{
    public class TransitStatusService : ITransitStatusInterface
    {
        string serviceUrl = "http://www.ausa.com.ar/autopista/carteleria/plano/mime.txt";

        public List<TransitStatus> CheckStatus()
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
                string[] words = remoteFile.Split(delimiterChars);

                char[] delimiterChars2 = { '=' };
                var statusList = new List<TransitStatus>();

                for (int i = 0; i < words.Length -1; i++)
                {
                    string[] kv = words[i].Split(delimiterChars2);
                    statusList.Add(GetStatusItem(kv));
                }

                return statusList;
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("Error getting the remote file from AUSA. {0}", e.Message));
            }
        }

        private TransitStatus GetStatusItem(string[] kv)
        {
            TransitStatus statusItem = null;
            
            if (kv.Length == 2)
            {
                statusItem = new TransitStatus
                {
                    Key = kv[0],
                    Value = kv[1],
                    Type = this.AnalyzeTransitStatusItem(kv[0])
                };
            }
            else
            {
                statusItem = new TransitStatus
                {
                    Key = string.Empty,
                    Value = string.Empty,
                    Type = string.Empty
                };
            }

            return statusItem;
        }

        public List<TransitStatus> CheckSpecificHighway()
        {
            throw new NotImplementedException();
        }

        private string AnalyzeTransitStatusItem(string item)
        {
            string response = string.Empty;

            if (item.StartsWith("VPM"))
            {
                response = "Measure Points";
            }
            else if (item.StartsWith("CPM"))
            {
                response = "Sign Information";
            }

            return response;
        }
    }
}
