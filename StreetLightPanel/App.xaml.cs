using CeraDevices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace StreetLightPanel
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {
       // public CeraDevices.CoordinatorDevice dev;
        public CeraDevices.DeviceManager coor_mgr;
        public StreetLightInfo[] street_light_info;
        public bool IsStart = true;
        public static string[,] Light2DInfo = new string[,] {
#region 綠創新
//        {  "1",	"2766",	"610"},
//{"2",	"2334",	"611"},
//{"3",	"1929",	"607"},
//{"4",	"1524",	"607"},
//{"5",	"987",	"692"},
//{"6",	"757",	"381"},
//{"7",	"526",	"650"},
//{"8",	"659",	"913"},
//{ "9",	"813",	"1169"},
//{"10",	"1070",	"1291"},
//{"11",	"1319",	"1292"},
//{"12",	"1646",	"1291"},
//{"13",	"2036",	"1290"},
//{"14",	"2325",	"1289"},
//{"15",	"2749",	"1275"},
//{"16",	"2773",	"1055"}
    


#endregion

#region 法鼓山

// {  "1","1479","925"},
//{"2","1343","1103"},
//{"3","1181","1243"},
//{"4","1885","945"},
//{"5","1653","1203"},
//{"6","1505","1379"},
//{"7","2023","1297"},
//{"8","1775","1466"},
//{"9","2377","1328"},
//{"10","2089","1482"},
//{"11","2939","1296"},
//{"12","2335","1936"},
//{"13","2299","1730"},
//{"14","3217","335"}
#endregion
#region 台北大學
{"1","4603","684"},

{"3","4671","773"},

{"5","4742","868"},

{"7","4796","949"},

{"9","4862","1046"},

#endregion
    };


        public App()
        {

            coor_mgr =new DeviceManager(new CeraDevices.CoordinatorDevice[]{ new CeraDevices.CoordinatorDevice("http://10.10.1.1:8080")});
            #region 綠創園區
//coor_mgr = new DeviceManager(new CeraDevices.CoordinatorDevice[] { new CeraDevices.CoordinatorDevice("http://172.16.23.92:8080") });
            #endregion

            try
            {
#if !DEBUG
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                street_light_info = coor_mgr.GetStreetLightList();

#endif
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "," + ex.StackTrace);
                Environment.Exit(-1);
            }
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            System.IO.StreamWriter wr =
                new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "unhandleerr.log");

            Exception ex=(Exception) e.ExceptionObject ;
            wr.WriteLine(ex.Message + "," + ex.StackTrace);
        }
    }
}
