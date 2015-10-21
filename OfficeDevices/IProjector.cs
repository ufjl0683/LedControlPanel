using System;
namespace OfficeDevices
{
  public  interface IProjector
    {
        string GetPowerStatus();
        string[] GetSignalSourceName();
        void PowerOFF();
        void PowerON();
        void SetSignalSource(string SourceName);
    }
}
