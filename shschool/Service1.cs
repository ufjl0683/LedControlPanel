using shschool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.Text;

namespace wcfShschool
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的類別名稱 "Service1"。
    [ServiceBehavior(InstanceContextMode= InstanceContextMode.Single)]
    public class Service1 : IService1
    {
        public  FongNanMain main;

        
         
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(string composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
        //    composite = "{\"BoolValue\":true,\"StringValue\":\"hello\"}";

            DataContractJsonSerializer sr = new DataContractJsonSerializer(typeof(CompositeType));
            MemoryStream  ms=new MemoryStream();
         
            byte[] data=System.Text.UTF8Encoding.UTF8.GetBytes(composite);
            ms.Write(data,0,data.Length);
            ms.Seek(0, SeekOrigin.Begin);
            try
            {
                CompositeType ret = (CompositeType)sr.ReadObject(ms);
                return ret;
            }
            catch (Exception ex)
            {
                 throw new FaultException(
                new FaultReason(ex.Message),
                new FaultCode("Data Access Error"));

            }
            
            //if (composite.BoolValue)
            //{
            //    composite.StringValue += "Suffix";
            //}
            //return composite;
        }


        public Scenarior[] GetAllScenariors()
        {
            return main.LedConfig.Scenariors.ToArray();
        }


        public void InvokeScenarior(string SceneNAme)
        {
            main.InvokeScene(SceneNAme);
        }
    }
}
