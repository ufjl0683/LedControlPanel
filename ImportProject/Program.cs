using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ImportProject
{
    class Program
    {
        static void Main(string[] args)
        {
            ImportProject.ServiceReference1.ControlServiceClient client = new ServiceReference1.ControlServiceClient(new InstanceContext(new CallBack()));
            if (args.Length != 0)
            {
                client.ImportProject("192.192.85.40",int.Parse(args[0]));
                Console.WriteLine("ImportCompleted");
                Console.ReadKey();
            }
        }
    }

    class CallBack : ImportProject.ServiceReference1.IControlServiceCallback
    {
        public void ToClientSayHello(string hello)
        {
            //throw new NotImplementedException();
        }

        public void ToClientNotifyLedDisplayChange(int DeviceID, ServiceReference1.LedOutputData outdata)
        {
          //  throw new NotImplementedException();
        }
    }
}
