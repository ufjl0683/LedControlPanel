using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace LedClientServiceWraper
{
    class Program
    {
        static void Main(string[] args)
        {
            LedClientService.Service1.launchHost();
            //LedClientService.ControlService service = new LedClientService.ControlService();
            //service.ImportProject(2);
            //ServiceHost host = new ServiceHost(new LedClientService.ControlService());
            //host.Open();
            Console.ReadLine();
        }
    }
}
