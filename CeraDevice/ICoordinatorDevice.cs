using System;
using CeraDevices.Coordinator2;
namespace CeraDevices.Coordinator2
{
    public  interface ICoordinatorDevice
    {
        void AddDeviceByMAC(string MAC);
        void ChangeRFChanel(int chanelno);
          CorrdinatorInfo GetDeviceInfo();
        DeviceInfo[] GetDeviceList();
        System.Threading.Tasks.Task<DeviceInfo[]> GetDeviceListAsync();
          CeraDevices.StreetLightInfo[] GetStreetLightList();
        StreetLightInfo[] GetStreetLightList(string devid);
          System.Threading.Tasks.Task<StreetLightInfo[]> GetStreetLightListAsync();
        System.Threading.Tasks.Task<StreetLightInfo[]> GetStreetLightListAsync(string devid);
        StreetLightInfo[] GetVisibleStreetLightList();
        System.Threading.Tasks.Task<StreetLightInfo[]> GetVisibleStreetLightListAsync();
        void Kick(string dev_id);
        void PermitJoinNode(int secs);
        void SetDeviceDimLevel(string devid, int level);
        void SetDeviceDimLevelAsync(string devid, int level);
        void SetDeviceRTC(string devid, DateTime dt);
        System.Threading.Tasks.Task SetDeviceRTCAsync(string devid, DateTime dt);
        void SetDeviceSchedule(string devid, string timeStr, string levelStr);
        void SetDeviceScheduleAsync(string devid, string timeStr, string levelStr);
          void SetDeviceScheduleEnable(string devid, bool enable);
        void SetDeviceScheduleEnableAsync(string devid, bool enable);
        void SetStreetLightRemark(string devid, string remark);
    }
}
