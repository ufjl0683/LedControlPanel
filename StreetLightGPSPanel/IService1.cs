using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace StreetLightPanel
{
    [ServiceContract]
   public interface IService1
    {
       [OperationContract]
       [WebGet]   
        System.IO.Stream GetImage(int x, int y, int z);
    }
}
