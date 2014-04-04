using System;
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

            foreach (var item in trafficStatus)
            {
                System.Console.WriteLine("Type : {0} | Key: {1} | Value: {2}", 
                    item.Type, item.Key, item.Value);   
            }

            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }
    }
}
