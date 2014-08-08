using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Stucked.Model;
using System.Configuration;
using System.IO;
using System.Diagnostics;

namespace Stucked.API.Controllers
{
    public class SignController : ApiController
    {
        string serviceUrl = ConfigurationManager.AppSettings["AusaServiceUrl"];

        // GET api/sign
        public IEnumerable<Sign> Get()
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

                var signList = new List<Sign>();
                var screenList = new List<Screen>();
                var messages = new List<string>();
                
                string currentSignName = string.Empty;
                string oldSignName = string.Empty;

                int currentSignScreen = 0;
                int oldSignScreen = 0;

                int currentIcon = 0;
                int currentSignScreenLine = 0;

                for (int i = 0; i < rawStatusList.Length - 1; i++)
                {
                    string[] kv = rawStatusList[i].Split(delimiterChars2);
                    var item = this.GetStatusItem(kv);

                    if (item != null)
                    {
                        if (item.Type == TransitStatusTypes.SignInformation)
                        {
                            if (oldSignScreen != currentSignScreen)
                            {
                                var currentScreen = new Screen();
                                currentScreen.Icon = currentIcon;
                                currentScreen.Number = currentSignScreen;
                                currentScreen.Messages = messages;

                                messages = new List<string>();

                                screenList.Add(currentScreen);
                            }

                            if (oldSignName != currentSignName)
                            {
                                var currentSign = new Sign();
                                currentSign.Name = currentSignName;
                                currentSign.Screens = screenList;

                                screenList = new List<Screen>();

                                signList.Add(currentSign);
                            }

                            oldSignName = currentSignName;
                            currentSignName = item.Key.Substring(0, item.Key.IndexOf("_"));

                            oldSignScreen = currentSignScreen;
                            currentSignScreen = int.Parse(item.Key.Substring(item.Key.IndexOf("_") + 1, 1));

                            if (item.Key.IndexOf("Ico") > 0)
                            {
                                currentIcon = int.Parse(item.Value);
                                currentSignScreenLine = 0;
                            }
                            else
                            {
                                currentSignScreenLine = int.Parse(item.Key.Substring(item.Key.LastIndexOf("_") + 1, 1));

                                try
                                {
                                    messages.Add(item.Value);
                                }
                                catch (ArgumentException e)
                                {   
                                    throw new ApplicationException(string.Format("Error adding message: Key: {0} / Value: {1} to the collection. {2}", item.Key, item.Value, e.Message));
                                }
                            }
                        }
                    }
                }

                return signList;
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("Error getting the remote file from AUSA. {0}", e.Message));
            }
        }

        // GET api/sign/5
        public Sign Get(int id)
        {
            return new Sign { 
                Name = "CPMV001",
                Screens = new List<Screen>()
            };
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
