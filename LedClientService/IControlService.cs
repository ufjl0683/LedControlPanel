using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Runtime.Remoting.Messaging;

namespace LedClientService
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的介面名稱 "IControlService"。
    [ServiceContract(CallbackContract=typeof(ICallBack)) ]
    public interface IControlService
    {
        [OperationContract]
        void ImportProject(string serverIP,int projectid);
        [OperationContract]
        void ToServerSayHello();
        [OperationContract]
        void Regist();
        [OperationContract]
        void ReloadSchedule();
        [OperationContract]
        LedClientService.Devices.LedOutputData[] GetAllLEDDeviceOutput();
        [OperationContract]
        LedClientService.Devices.LedDevice[] GetAllLEDDeviceInfo();
        [OperationContract]
        void ImportDevices(string serverIP, int projectID);

    }



    public interface ICallBack
    {
     

      [OperationContract(IsOneWay = true)]
      void ToClientSayHello(string hello);
      [OperationContract(IsOneWay = true)]
      void ToClientNotifyLedDisplayChange(int DeviceID, LedClientService.Devices.LedOutputData outdata);
      [OperationContract(IsOneWay = true)]
      void ToClientNotifyConnectionChange(int DeviceID, bool Connected);

    }


}
