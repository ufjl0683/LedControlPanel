using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;

namespace LedClientService
{
    public partial class Service1 : ServiceBase
    {
        public static ControlService ControlService;
        public static MainControl MainControl;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            launchHost();
          
        }

        public static void launchHost()
        {
             ControlService=new LedClientService.ControlService();
             ServiceHost host = new ServiceHost(ControlService);
             host.Open();
             MainControl = new MainControl();
            
        }
        protected override void OnStop()
        {
        }
    }
}
