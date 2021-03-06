﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using shschool;

namespace wcfShschool
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的介面名稱 "IService1"。
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [WebGet(ResponseFormat=WebMessageFormat.Json) ]
        string GetData(int value);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        Scenarior[] GetAllScenariors();

        [OperationContract]
        [WebGet]
        void InvokeScenarior(string SceneName);
        

        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,RequestFormat=WebMessageFormat.Json,Method="GET" )]
        CompositeType GetDataUsingDataContract(string composite);

        // TODO: 在此新增您的服務作業
    }

    // 使用下列範例中所示的資料合約，新增複合型別至服務作業。
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
