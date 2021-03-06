﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stucked.Poc
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new TransitStatusService();

            var trafficStatus = service.CheckStatus();

            var measurePoints = new List<TransitStatus>();
            var actualSign = string.Empty;
            var actualMessage = string.Empty;

            foreach (var item in trafficStatus)
            {
                if (item.Type == "Sign Information")
	            {
                    if (item.Key.Substring(0, item.Key.IndexOf("_")) == actualSign)
                    {
                        actualMessage += " " + item.Value;
                    }
                    else
                    {
                        //System.Console.WriteLine("Type : {0} | Key: {1} | Value: {2}",
                        //    item.Type, item.Key, item.Value);

                        System.Console.WriteLine(actualMessage);
                        actualMessage = string.Empty;
                    }

                    actualSign = item.Key.Substring(0, item.Key.IndexOf("_"));
	            }
            }

            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }
    }
}
