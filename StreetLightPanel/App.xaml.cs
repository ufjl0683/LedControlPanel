﻿using CeraDevices;
using CeraDevices.Coordinator2;
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
//{  "1",	"2766",	"610"},
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
#region 海洋大學
			
//{"1","2366","1698"},	
									 
//{"2","2286","1667"},	
									
//{"3","2207","1601"},	
									
//{"4","2099","1577"},	
									
//{"5","2015","1564"},	
									
//{"6","1839","1543"},	
									
//{"7","1605","1536"},	
									
//{"8","1811","1614"},	
									
//{"9","1752","1705"},	
									
//{"10","1879","2011"},
										
//{"11","1793","1948"},
										
//{"12","1708","1885"},
										
//{"13","1622","1822"},
										
//{"14","1533","1758"},
										
//{"15","1568","1709"},
										
//{"16","1433","1684"},
										
//{"17","1340","1616"},
										
//{"18","1270","1569"},
										
//{"19","1172","1537"},
										
//{"20","1066","1556"},
										
//{"21","916","1683"},	
									
//{"22","739","1616"},	
									
//{"23","631","1570"},	
									
//{"24","1010","1798"},
										
//{"25","1070","1831"},
										
//{"26","1160","1940"},
									
//{"27","1236","1994"},
										
//{"28","1303","2056"},
										
//{"29","1480","2108"},
										
//{"30","1675","2266"},
										
//{"31","307","2210"},	
									
//{"32","365","2204"},	
									
//{"33","506","2268"},	
									
//{"34","686","2307"},	
									
//{"35","817","2141"},	
									
//{"36","898","2024"},	
									
//{"37","972","1925"},	
									
//{"38","1175","2058"},
										
//{"39","1128","2172"},
										
//{"40","967","2293"},	
									
//{"41","866","2267"},	

//{"42","1105","2306"},
										
//{"43","1091","2320"},
										
//{"44","1198","2376"},
										
//{"45","1182","2395"},
										
//{"46","1300","2457"},
										
//{"47","1285","2474"},
										
//{"48","1386","2525"},
										
//{"49","1373","2545"},
										
//{"50","1456","2583"},
										
//{"51","1443","2603"},
										
//{"52","1559","2670"},
										
//{"53","1668","2661"},
										
//{"54","1780","2723"},
										
//{"55","1892","2806"},
										
//{"56","2003","2954"},
										
//{"57","2107","3203"},	
									
//{"58","1718","2519"},	
									
//{"59","1780","2457"},	
									
//{"60","1815","2351"},	
									
//{"61","1717","2380"},	
									
//{"62","1953","2579"},
										
//{"63","1941","2409"},	
									
//{"64","1995","2329"},
										
//{"65","2144","2209"},	
									
//{"66","2005","2109"},	
									
//{"67","2261","1855"},	
								
//{"68","2447","1673"},	
									
//{"69","2582","1561"},	
									
//{"70","2634","1470"},	
									
//{"71","2549","1438"},	
												
//{"72","2374","2371"},	
									
//{"73","2461","2421"},	
									
//{"74","2558","2473"},	
									
//{"75","2661","2514"},	
									
//{"76","2772","2543"},	
									
//{"77","2869","2569"},	
									
//{"78","2964","2591"},	
									
//{"79","3062","2611"},	
									
//{"80","3167","2630"},
										
//{"81","3279","2645"},	
									
//{"82","3375","2652"},
										
//{"83","3496","2653"},	
									
//{"84","3734","2719"},	
									
//{"85","3827","2784"},	
									
//{"86","3927","2844"},	
											
//{"87","2548","1716"},	
									
//{"88","2678","1762"},	
									
//{"89","2930","1869"},	
									
//{"90","3101","1945"},	
									
//{"91","3261","2012"},	
									
//{"92","3406","2081"},	
									
//{"93","3495","2120"},	
									
//{"94","3625","2170"},	
									
//{"95","3751","2207"},	
									
//{"96","3894","2239"},	
									
//{"97","4167","2247"},	
									
//{"98","4184","2490"},	
									
//{"99","4183","2550"},	
									
//{"100","4182","2605"},	
									
//{"101","4082","2971"},
										
//{"102","4428","2064"},	
									
//{"103","4848","2077"},	
									
////{"104","5747","2109"},	
//    {"104","5470","2095"},								
//{"105","5529","2130"},	
									
//{"106","5583","2150"},	
									
//{"107","5650","2172"},	


									
//{"108","5592","2258"},	
									
//{"109","5574","2241"},	
									
//{"110","5783","2068"},	
									
//{"111","5965","2122"},	
									
//{"112","5978","2023"},
										
//{"113","5227","1972"},	
									
//{"114","5145","1944"},
										
//{"115","5060","1911"},	
									
//{"116","4737","1741"},
										
//{"117","4648","1702"},	
									
//{"118","3973","1406"},
										
//{"119","3997","1414"},	
									
//{"120","4495","1267"},
										
//{"121","4622","1315"},
										
//{"122","4779","1376"},	
									
//{"123","4889","1408"},	
									
//{"124","4996","1453"},	
							
//{"125","5094","1491"},	
									
//{"126","5251","1549"},	
									
//{"127","5340","1574"},	
									
//{"128","5528","1650"},	
									
//{"129","5608","1681"},	
									
//{"130","5741","1733"},	
							
//{"131","5864","1783"},	
							
//{"132","5994","1839"},
									
//{"133","6137","1894"}




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
//{"1","1473","218"},  {"2","1492","220"},
//{"3","1495","247"},  {"4","1513","251"},
//{"5","1517","277"},  {"6","1533","280"},
//{"7","1534","303"},  {"8","1553","309"},
//{"9","1556","335"},  {"10","1575","338"},
//{"11","1576","363"},  {"12","1595","369"},
//{"13","1597","393"},  {"14","1616","396"},
//{"15","1615","418"},  {"16","1634","421"},
//{"17","1632","446"},  {"18","1655","450"},
//{"19","1674","502"},  {"20","1679","485"},
//{"21","1700","537"},  {"22","1697","512"},
//{"23","1720","566"},  {"24","1712","531"},
//{"25","1737","600"},  {"26","1741","574"},
//{"27","1718","612"},  {"28","1754","603"},
//{"29","1674","637"},  {"30","1734","616"},
//{"31","1641","659"},  {"32","1702","636"},
//{"33","1615","677"},  {"34","1665","659"},
//{"35","1589","694"},  {"36","1635","679"},
//{"37","1554","716"},  {"38","1609","695"},
//{"39","1522","734"},  {"40","1576","717"},
//{"41","1495","752"},  {"42","1545","738"},
//{"43","1465","767"},  {"44","1516","755"},
//{"45","1438","785"},  {"46","1488","772"},
//{"47","1412","802"},  {"48","1460","790"},
//{"49","1382","817"},  {"50","1430","808"},
//{"51","1357","834"},  {"52","1407","823"},
//{"53","1330","859"},  {"54","1377","842"},
//{"55","1304","877"},  {"56","1350","860"},
//{"57","1275","897"},  {"58","1325","878"},
//{"59","1239","920"},  {"60","1298","898"},
//{"61","1207","938"},  {"62","1265","919"},
//{"63","1176","954"},  {"64","1231","940"},
//{"65","1152","970"},  {"66","1196","958"},
//{"67","1123","993"},  {"68","1168","973"},
//{"69","1104","1026"},  {"70","1143","990"},
//{"71","1096","1054"},  {"72","1119","1020"},
//{"73","1087","1089"},  {"74","1111","1041"},
//{"75","1074","1125"},  {"76","1107","1058"},
//{"77","1059","1157"},  {"78","1097","1094"},
//{"79","1039","1180"},  {"80","1076","1155"},
//{"81","1014","1202"},  {"82","1059","1180"},
//{"83","985","1214"},  {"84","1037","1201"},
//{"85","958","1218"},  {"86","1004","1218"},
//{"87","922","1211"},  {"88","967","1229"},
//{"89","893","1205"},  {"90","938","1231"},
//{"91","866","1202"},  {"92","904","1222"},
//{"93","840","1204"},  {"94","879","1216"},
//{"95","802","1209"},  {"96","854","1217"},
//{"97","768","1217"},  {"98","818","1221"},
//{"99","730","1225"},  {"100","782","1228"},
//{"101","692","1229"},  {"102","748","1235"},
//{"103","659","1232"},  {"104","714","1239"},
//{"105","558","957"},  {"106","666","1246"},
//{"107","575","957"},  {"108","574","936"},
//{"109","592","935"},  {"110","595","909"},
//{"111","627","894"},  {"112","624","875"},
//{"113","645","870"},  {"114","644","850"},
//{"115","668","846"},  {"116","667","824"},
//{"117","690","821"},  {"118","695","799"},
//{"119","718","800"},  {"120","720","778"},
//{"121","745","777"},  {"122","747","758"},
//{"123","771","754"},  {"124","771","735"},
//{"125","790","730"},  {"126","787","708"},
//{"127","808","703"},  {"128","835","657"},
//{"129","833","678"},  {"130","858","653"},
//{"131","882","629"},  {"132","881","610"},
//{"133","904","609"},  {"134","923","589"},
//{"135","947","569"},  {"136","926","566"},
//{"137","969","547"},  {"138","946","547"},
//{"139","990","525"},  {"140","968","526"},
//{"141","1014","502"},  {"142","992","502"},
//{"143","1036","481"},  {"144","1014","481"},
//{"145","1060","462"},  {"146","1040","459"},
//{"147","1081","445"},  {"148","1062","440"},
//{"149","1109","424"},  {"150","1085","420"},
//{"151","1135","406"},  {"152","1110","403"},
//{"153","1165","385"},  {"154","1140","381"},
//{"155","1199","361"},  {"156","1202","337"},
//{"157","1228","339"},  {"158","1260","295"},
//{"159","1255","322"},  {"160","1286","300"},
//{"161","1317","279"},  {"162","1330","251"},
//{"163","1353","257"},  {"164","1366","239"},
//{"165","1392","246"},  {"166","1405","225"},
//{"167","1441","221"},  {"168","1469","187"},


#endregion

#region 海大育樂館
			
{"1","230","106"},	
									 
{"2","230","175"},	
									
{"3","230","248"},	
									
{"4","230","322"},	
									
{"5","230","391"},	
									
{"6","288","106"},	
									
{"7","288","175"},	
									
{"8","288","248"},	
									
{"9","288","322"},	
									
{"10","288","391"},
										
{"11","348","106"},
										
{"12","348","175"},
										
{"13","348","248"},
										
{"14","348","322"},
										
{"15","348","391"},
										
{"16","403","106"},
										
{"17","403","175"},
										
{"18","403","248"},
										
{"19","403","322"},
										
{"20","403","391"},
										
{"21","461","106"},	
									
{"22","461","175"},	
									
{"23","461","248"},	
									
{"24","461","322"},
										
{"25","461","391"},
										
{"26","520","106"},
									
{"27","520","175"},
										
{"28","520","248"},
										
{"29","520","322"},
										
{"30","520","391"},
										
{"31","580","106"},	
									
{"32","580","175"},	
									
{"33","580","248"},	
									
{"34","580","322"},	
									
{"35","580","391"},	
								
#endregion

#region 海大河工館
			
//{"1","114","533"},	
									 
//{"2","114","585"},	
									
//{"3","159","533"},	
									
//{"4","159","585"},	
									
//{"5","215","533"},	
									
//{"6","215","585"},	
									
//{"7","274","533"},	
									
//{"8","274","585"},	
									
//{"9","326","533"},	
									
//{"10","326","585"},
										
//{"11","379","428"},
										
//{"12","379","477"},
										
//{"13","379","533"},
										
//{"14","379","585"},
										
//{"15","426","428"},
										
//{"16","426","477"},
										
//{"17","426","533"},
										
//{"18","426","585"},
										
//{"19","475","428"},
										
//{"20","475","477"},
										
//{"21","475","533"},	
									
//{"22","475","585"},	
									
//{"23","525","428"},	
									
//{"24","525","477"},
										
//{"25","525","533"},
										
//{"26","525","585"},
									
//{"27","581","428"},
										
//{"28","581","477"},
										
//{"29","581","533"},
										
//{"30","581","585"},
										
//{"31","638","95"},	
									
//{"32","638","144"},	
									
//{"33","638","215"},	
									
//{"34","638","264"},	
									
//{"35","638","318"},	
									
//{"36","638","368"},	
									
//{"37","638","428"},	
									
//{"38","638","477"},
										
//{"39","638","533"},
										
//{"40","638","585"},	
									
//{"41","680","95"},
	

//{"42","680","144"},
										
//{"43","680","215"},
										
//{"44","680","264"},
										
//{"45","680","318"},
										
//{"46","680","368"},
										
//{"47","680","428"},
										
//{"48","680","477"},
										
//{"49","680","533"},
										
//{"50","680","585"},
										
//{"51","729","95"},
										
//{"52","729","144"},
										
//{"53","729","215"},
										
//{"54","729","264"},
										
//{"55","729","318"},
										
//{"56","729","368"},
										
//{"57","729","428"},	
									
//{"58","729","477"},	
									
//{"59","777","215"},
	

//{"60","777","264"},


//{"61","777","318"},


//{"62","777","368"},


//{"63","777","428"},


//{"64","777","477"},


//{"65","831","215"},


//{"66","831","264"},


//{"67","831","318"},


//{"68","831","368"},


//{"69","831","428"},


//{"70","831","477"},


#endregion

    };


        public App()
        {
#if Version1
           // coor_mgr = new DeviceManager(new ICoordinatorDevice[] { new CoordinatorDevice("http://10.10.1.1:8080") });
            //海洋大學 IP: 140.121.184.101  IP: 140.121.184.102  IP: 140.121.184.103 
            coor_mgr = new DeviceManager(new ICoordinatorDevice[] {
            new CoordinatorDevice("http://140.121.184.101:8080"),
            new CoordinatorDevice("http://140.121.184.102:8080"),
            new CoordinatorDevice("http://140.121.184.103:8080")
            });

#else
           // coor_mgr = new DeviceManager(new ICoordinatorDevice[] { new CoordinatorDevice2("http://192.168.0.99:8080") });
          //  coor_mgr = new DeviceManager(new ICoordinatorDevice[] { new CoordinatorDevice2("http://122.116.43.100:8080") });
            coor_mgr = new DeviceManager(new ICoordinatorDevice[] { new CoordinatorDevice2("http://192.168.1.1:8080") });
#endif
            #region 綠創園區
          //  coor_mgr = new DeviceManager(new CeraDevices.CoordinatorDevice[] { new CeraDevices.CoordinatorDevice("http://172.16.23.92:8080") });
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
