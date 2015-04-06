using CeraDevices;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StreetLightPanel
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        ServiceHost serviceHost;
        double currentx, currenty;
      //  CeraDevices.CoordinatorDevice coor;
        CeraDevices.DeviceManager coor_mgr;
        System.Collections.Generic.Dictionary<string, StreetLightBindingData> dictStreetLightBindingInfos = new Dictionary<string, StreetLightBindingData>();
        System.Collections.Generic.Dictionary<string, StreetLightBindingData> dictStreetLightBindingInfosOriginal = new Dictionary<string, StreetLightBindingData>();
        System.Windows.Threading.DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();
      Config LedConfig;
        IValueConverter fontconverter = new IsEnableToColorConverter();
        public MainWindow()
        {
            InitializeComponent();
           coor_mgr = ((App.Current) as App).coor_mgr;
            InitLed2dPositoin();
            serviceHost = new ServiceHost(new Service1());
            serviceHost.Open();
         }
#region map
        Envelope ConvertPointToEnvelop(MapPoint point)
        {


            ESRI.ArcGIS.Client.Projection.WebMercator wm = new ESRI.ArcGIS.Client.Projection.WebMercator();
            //return wm.FromGeographic(new ESRI.ArcGIS.Client.Geometry.Envelope(point, point) { SpatialReference = new SpatialReference(org_wkid) }).Extent;
            return wm.FromGeographic(point).Extent;
        }
        MapPoint ConvertMapPointTo102100(MapPoint mp)
        {
            ESRI.ArcGIS.Client.Projection.WebMercator wm = new ESRI.ArcGIS.Client.Projection.WebMercator();
            return wm.FromGeographic(mp) as MapPoint;
        }

        void ZoomToLevel(int level)
        {

            double resolution = (this.map.Layers["basemap"] as TiledLayer).TileInfo.Lods[level].Resolution;
            this.map.ZoomToResolution(resolution);
        }

        void ZoomToLevel(int level, MapPoint point)
        {
            bool zoomentry = false;
            double resolution;
            if (level == -1)
                resolution = map.Resolution;
            else
                resolution = (this.map.Layers["basemap"] as TiledLayer).TileInfo.Lods[level].Resolution;


            if (Math.Abs(map.Resolution - resolution) < 0.05)
            {
                this.map.PanTo(point);
                return;
            }
            zoomentry = false;
            this.map.ZoomToResolution(resolution);

            map.ExtentChanged += (s, a) =>
            {
                if (!zoomentry)
                    this.map.PanTo(point);

                zoomentry = true;

                //   SwitchLayerVisibility();
            };



        }

        int GetCurrentLevel()
        {

            Lod[] Lods = (this.map.Layers["basemap"] as TiledLayer).TileInfo.Lods;
            for (int i = 0; i < Lods.Length; i++)
            {
                if (Math.Abs(this.map.Resolution - Lods[i].Resolution) < 1 || this.map.Resolution >= Lods[i].Resolution)
                {
                    return i;
                }

            }
            return 0;
        }

        void LocateTo(int level, double x, double y)
        {

            ESRI.ArcGIS.Client.Geometry.MapPoint mp = new ESRI.ArcGIS.Client.Geometry.MapPoint(x, y, new SpatialReference(102100));
            if (currentx != x || currenty != y)
            {
                currentx = x;
                currenty = y;
              
            }

            this.ZoomToLevel(level, mp);
           

          
        }
#endregion

        int cnt = 0;
        void InitLed2dPositoin()
        {
            int LedWidth = 15;
          
            for(int i=0;i<App.Light2DInfo.GetLength(0);i++)
            {
                
                   
                    CheckBox chk = new CheckBox();
                    chk.Width = chk.Height = LedWidth;
                    chk.Style = this.FindResource("CheckBoxStyle1") as Style;
                    //chk.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    //chk.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    double x, y;
                    x=App.Light2DInfo[i,1];
                      y=(App.Light2DInfo[i,2]);
                  //  chk.Margin = new Thickness( x- LedWidth / 2, y - LedWidth / 2, 0, 0);
                    chk.Content = App.Light2DInfo[i,0];
                    chk.MouseRightButtonUp += chk_MouseRightButtonUp;
                    
                    ESRI.ArcGIS.Client.ElementLayer.SetEnvelope(chk,
                      ConvertPointToEnvelop( new MapPoint(x,y))
                        );

                    chk.Background = new SolidColorBrush(Colors.Yellow);
                    chk.Foreground = new SolidColorBrush(Colors.Red);
                    (this.map.Layers["grdDeviceLayer"] as ESRI.ArcGIS.Client.ElementLayer).Children.Add(chk);

                    cnt++;
                }

     //       MessageBox.Show(cnt.ToString());
            }

        void chk_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            wndSchedule wnd = new wndSchedule(chk.Content.ToString());
          //  wnd.Left = chk.Margin.Left;
          //  wnd.Top = chk.Margin.Top;
            wnd.Owner = this;
            wnd.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            wnd.ShowDialog();
            //throw new NotImplementedException();
        }
        async void Initial()
        {
#if !DEBUG

            await GetDeviceInfoAndSetBindingData();
            if ((App.Current as App).IsStart)
            {
              
             //   SetAllDeviceTo100();
                (App.Current as App).IsStart = false;
            }
            tmr.Interval = TimeSpan.FromSeconds(20);
            tmr.Tick += tmr_Tick;
            tmr.Start();

            System.Windows.Threading.DispatcherTimer tmr5min = new System.Windows.Threading.DispatcherTimer() { Interval = TimeSpan.FromMinutes(5) };
            tmr5min.Tick += tmr5min_Tick;
            tmr5min.Start();
#endif
        }

        async void tmr5min_Tick(object sender, EventArgs e)
        {

              DeviceInfo[] infos = null;
            //try
            //{
            //    try
            //    {
            //        infos = await coor_mgr.GetDeviceListAsync();
            //    }
            //    catch { ;}
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}


            //foreach (DeviceInfo info in infos)
            //{
            //    if (info == null)
            //        continue;
            //    if (dictStreetLightBindingInfos.ContainsKey(info.addr))
            //    {

            //        dictStreetLightBindingInfos[info.addr].IsEnable = info.visibility;
                  
            //    }
            //}


            StreetLightInfo[] street_light_info = null;
            try
            {
                street_light_info = await this.coor_mgr.GetStreetLightListAsync();
            }
            catch { }
            if (street_light_info != null)
            {
                using (StreamWriter wr = System.IO.File.CreateText(AppDomain.CurrentDomain.BaseDirectory + "energy.txt"))
                {
                foreach (StreetLightInfo data in street_light_info)
                {
                    if (dictStreetLightBindingInfos.ContainsKey(data.DevID))
                    {
                       //  dictStreetLightBindingInfos[data.DevID].DimLevel = data.CurrentDimLevel;
                        //dictStreetLightBindingInfos[data.DevID].W = data.W;
                        //dictStreetLightBindingInfos[data.DevID].IsEnable = true;
                       
                            wr.WriteLine(string.Format("{0} {1} {2} {3} {4} {5} {6}",DateTime.Now,data.DevID, dictStreetLightBindingInfos[data.DevID].LightNo,data.V,data.A,data.W,data.KWHP*2));
                      
                    }
                }
                }
        }
            var q = from n in dictStreetLightBindingInfos.Values where !street_light_info.Select(k => k.DevID).ToArray().Contains(n.DevID) select n;
            foreach (StreetLightBindingData data in q)
                data.IsEnable = false;
           
        }

        bool IsinSetting = false;
        bool IsInTimer = false;

        int tickcnt=0;
      async  void tmr_Tick(object sender, EventArgs e)
        {
            try
            {
                if (IsInTimer || IsinSetting)
                    return;
                IsInTimer = true;
                  await GetDeviceInfoAndSetBindingData();
                   if (tickcnt++ % 180 == 0)
                   {
                       coor_mgr.SetDeviceRTC("*", DateTime.Now);
                   }
               
                CheckLedOutput();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                IsInTimer = false;
            }
            //throw new NotImplementedException();
        }


        async Task GetDeviceInfoAndSetBindingData()
        {
            DeviceInfo[] infos = null;
            try
            {
                try
                {
                    infos = await coor_mgr.GetDeviceListAsync();
                }
                catch { ;}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            foreach (DeviceInfo info in infos)
            {
                if (info == null)
                    continue;
                if (dictStreetLightBindingInfos.ContainsKey(info.addr))
                {

                    dictStreetLightBindingInfos[info.addr].IsEnable = info.visibility;
                   
                }
            }
            StreetLightInfo[] street_light_info = null;
            try
            {
                street_light_info = await this.coor_mgr.GetStreetLightListAsync();
            }
            catch { }
            if (street_light_info != null)
                foreach (StreetLightInfo data in street_light_info)
                {
                    if (dictStreetLightBindingInfos.ContainsKey(data.DevID))
                    {
                        dictStreetLightBindingInfos[data.DevID].DimLevel = data.CurrentDimLevel;
                        dictStreetLightBindingInfos[data.DevID].W = data.W;
                      //  dictStreetLightBindingInfos[data.DevID].IsEnable = true;
                    }
                }
            var q = from n in dictStreetLightBindingInfos.Values where !street_light_info.Select(k => k.DevID).ToArray().Contains(n.DevID) select n;
            foreach (StreetLightBindingData data in q)
                data.IsEnable = false;
        }
        void CheckLedOutput()
        {
            StreetLightInfo[] infos = null;
            // temp for return
        
            try
            {
                infos = coor_mgr.GetStreetLightList();
            }
            catch
            {

            }
            if (infos != null)
                foreach (StreetLightInfo data in infos)
                {
                    if (!dictStreetLightBindingInfos.ContainsKey(data.DevID))
                        continue;

                    if ((dictStreetLightBindingInfos[data.DevID]).IsEnable /*&& data.CurrentDimLevel != (dictStreetLightBindingInfos[data.DevID]).DimLevel*/)
                    {
                        try
                        {
                            (dictStreetLightBindingInfos[data.DevID]).DimLevel=data.CurrentDimLevel;
                        //    coor.SetDeviceDimLevel(data.DevID, (dictStreetLightBindingInfos[data.DevID]).DimLevel);
                        }
                        catch { ;}
                    }
                }
        }
        private void rdUniform_Checked(object sender, RoutedEventArgs e)
        {
            //if (vbImage == null) return;
              
         //   vbImage.Stretch = Stretch.Uniform;
       //     scrollViewer1_SizeChanged(null, null);
            //   imgPic.Height = vbImage.ActualHeight;
            //vbImage.UpdateLayout();
        }

        private void rdNone_Checked(object sender, RoutedEventArgs e)
        {
            //vbImage.Stretch = Stretch.None;
            //scrollViewer1_SizeChanged(null, null);
            
        }

        //private void scrollViewer1_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    if (rdNone.IsChecked == true)
        //    {
        //        vbImage.Stretch = Stretch.None;
        //        vbImage.Width = imgPic.Width;
        //        vbImage.Height = imgPic.Height;
        //        vbImage.Margin = new Thickness(0);
        //        vbImage.UpdateLayout();
        //    }
        //    else //全覽
        //    {
        //        vbImage.Width = scrollViewer1.ViewportWidth;
        //        vbImage.Height = scrollViewer1.ViewportHeight;
        //        vbImage.Margin = new Thickness(0);
        //        vbImage.Stretch = Stretch.Uniform;
        //    }

        //}

        //private void bdrOverView_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    Point p = e.GetPosition(sender as IInputElement);
        //    double horoffset, veroffset;
        //    horoffset = p.X / bdrOverView.ActualWidth * imgPic.ActualWidth;
        //    veroffset = p.Y / bdrOverView.ActualHeight * imgPic.ActualHeight;
        //    scrollViewer1.ScrollToHorizontalOffset(horoffset);
        //    scrollViewer1.ScrollToVerticalOffset(veroffset);
        //}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
#if !DEBUG
           
            
           

            coor_mgr.SetDeviceRTC("*", DateTime.Now);
            coor_mgr.SetDeviceScheduleEnable("*", true);
#endif
            if (App.Current.Properties.Contains("LightCollection"))
            {
                this.dictStreetLightBindingInfos = App.Current.Properties["LightCollection"] as Dictionary<string, StreetLightBindingData>;
                this.dictStreetLightBindingInfosOriginal = App.Current.Properties["LightCollectionOriginal"] as Dictionary<string, StreetLightBindingData>;
            }
            System.Collections.Generic.Dictionary<string, StreetLightBindingData> List = new Dictionary<string, StreetLightBindingData>(); System.Collections.Generic.Dictionary<string, StreetLightBindingData> list = new Dictionary<string, StreetLightBindingData>();
            if (!System.IO.File.Exists("config.xml"))
            {
                Config config = new Config();


                #region  open here later
                foreach (UIElement element in (this.map.Layers["grdDeviceLayer"] as ElementLayer).Children)
                {

                    if (element is CheckBox)
                    {
                        CheckBox btn = element as CheckBox;
                        list.Add(btn.Content.ToString(), new StreetLightBindingData() { DevID = btn.Content.ToString(), OriginalDevID = btn.Content.ToString() });
                        btn.Tag = btn.Content                            ;
                    }


                }
                #endregion

                config.StreetLightBindingDatas = list.Values.ToArray();
                config.Scenariors = new List<Scenarior>();
                //System.Xml.Serialization.XmlSerializer sr = new System.Xml.Serialization.XmlSerializer(typeof(Config));
                //sr.Serialize(System.IO.File.OpenWrite("config.xml"), config);
                SaveConfig(config);
                LedConfig = config;
              
             //   this.listScene.ItemsSource = LedConfig.Scenariors;
            }
            else
            {
                //System.Xml.Serialization.XmlSerializer sr = new System.Xml.Serialization.XmlSerializer(typeof(Config));
                //Config config = sr.Deserialize(System.IO.File.OpenRead("config.xml")) as Config;
                LedConfig = LoadConfig();
                foreach (StreetLightBindingData data in LedConfig.StreetLightBindingDatas)
                    list.Add(data.OriginalDevID, data);

               // this.listScene.ItemsSource = LedConfig.Scenariors;
            }

            #region open here later
            foreach (UIElement element in (this.map.Layers["grdDeviceLayer"] as ElementLayer).Children)
            {

                if (element is CheckBox)
                {
                    CheckBox btn = element as CheckBox;

                    StreetLightBindingData temp;
                    string devid = "";
                    btn.Tag = list[btn.Content.ToString()].OriginalDevID;
                    devid = list[btn.Content.ToString()].DevID;
                    if (!dictStreetLightBindingInfos.ContainsKey(devid))
                    {

                        temp = new StreetLightBindingData() { DevID = devid, OriginalDevID = btn.Tag.ToString(), DimLevel = 0, IsEnable = false, LightNo = list[btn.Content.ToString()].LightNo, IsFake = list[btn.Content.ToString()].IsFake};

                        dictStreetLightBindingInfos.Add(temp.DevID, temp);
                        dictStreetLightBindingInfosOriginal.Add(temp.OriginalDevID, temp);
                    }
                    else

                        temp = dictStreetLightBindingInfos[devid] as StreetLightBindingData;





                   Binding binding = new Binding() { Path = new PropertyPath("IsEnable"), Converter = fontconverter };

                   btn.SetBinding(Button.ForegroundProperty, binding);

                    btn.SetBinding(CheckBox.ContentProperty, new Binding("DevID"));
                    btn.SetBinding(CheckBox.IsEnabledProperty, new Binding("IsEnable"));
                    btn.SetBinding(CheckBox.IsCheckedProperty, new Binding("IsChecked") { Mode = BindingMode.TwoWay });

                    btn.DataContext = temp;

                    btn.IsChecked = temp.IsChecked;

                }




            } // foeach

            #endregion

            App.Current.Properties["LightCollection"] = dictStreetLightBindingInfos;
            App.Current.Properties["LightCollectionOriginal"] = dictStreetLightBindingInfosOriginal;
            Initial();
            Button_Click_1(null, null); //歸位
        }

        Config LoadConfig()
        {

            System.Xml.Serialization.XmlSerializer sr = new System.Xml.Serialization.XmlSerializer(typeof(Config));
            Config config = sr.Deserialize(System.IO.File.OpenRead("config.xml")) as Config;
            return config;
        }

        void SaveConfig(Config config)
        {
            System.Xml.Serialization.XmlSerializer sr = new System.Xml.Serialization.XmlSerializer(typeof(Config));
            try
            {
                System.IO.File.Delete("config.xml");
            }
            catch { ;}
            sr.Serialize(System.IO.File.OpenWrite("config.xml"), config);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            #region open here later
            foreach (UIElement element in  (this.map.Layers["grdDeviceLayer"] as ElementLayer).Children)
            {
                if (element is CheckBox)
                {
                    if ((element as CheckBox).IsEnabled)
                        (element as CheckBox).IsChecked = true;
                }
            }
            #endregion
        }

        

        private void btnTemplateEdit_Click(object sender, RoutedEventArgs e)
        {
          wndTempleateEdit dlg=  new wndTempleateEdit(LedConfig.Scenariors);
            if (dlg.ShowDialog()==true)
            {
                SendTemplate(dlg.SelectedScenarior);

            }
            SaveConfig(this.LedConfig);
        }

        async void SendTemplate(Scenarior scene)
        {

            #region open later
            foreach (UIElement element in  (this.map.Layers["grdDeviceLayer"] as ElementLayer).Children)
            {

                try
                {
                    if (element is CheckBox)
                    {
                        if ((element as CheckBox).IsEnabled && (element as CheckBox).IsChecked == true)
                        {
                            bool isFinish = false;
                            for (int i = 0; i < 3; i++)
                            {
                                try
                                {
                                    StreetLightBindingData data = (element as CheckBox).DataContext as StreetLightBindingData;

                                    coor_mgr.SetDeviceScheduleAsync(data.DevID, scene.Schedule.GetScheduleSegTimeString(), scene.Schedule.GetScheduleSegLevelString());

                                    await Task.Delay(100);
                                    StreetLightInfo[] backInfo = await coor_mgr.GetStreetLightListAsync(data.DevID);
                                    await Task.Delay(100);
                                    if (backInfo[0].sch.IsEqual(scene.Schedule))
                                    {
                                        isFinish = true;
                                        break;
                                    }

                                }
                                catch
                                { ;}
                                Task.Delay(5000);
                            }
                            if (!isFinish)
                            {
                                MessageBox.Show(((element as CheckBox).DataContext as StreetLightBindingData).DevID + "," + "排程傳送失敗!");
                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(((element as CheckBox).DataContext as StreetLightBindingData).DevID + "," + ex.Message);
                }
            }
            MessageBox.Show("ok!");
            #endregion
        }
        private void btnUnselectAll_Click(object sender, RoutedEventArgs e)
        {

            //open  latter
            foreach (UIElement element in  (this.map.Layers["grdDeviceLayer"] as ElementLayer).Children)
            {
                if (element is CheckBox)
                {
                    if ((element as CheckBox).IsEnabled)
                        (element as CheckBox).IsChecked = false;
                }
            }
        }

        private void btnReverseSelect_Click(object sender, RoutedEventArgs e)
        {

            //open later
            foreach (UIElement element in  (this.map.Layers["grdDeviceLayer"] as ElementLayer).Children)
            {
                if (element is CheckBox)
                {
                    if ((element as CheckBox).IsEnabled)
                    {
                        (element as CheckBox).IsChecked = !((element as CheckBox).IsChecked ?? false);
                    }
                }
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
         // 銘大林口校區
            //121.341855, 24.984922
        //ZoomToLevel(19, ConvertMapPointTo102100(new MapPoint(121.3430004, 24.986459)));
         //銘大金門校區           );
          // 118.402180,24.501124
            ZoomToLevel(19, ConvertMapPointTo102100(new MapPoint(118.402180, 24.501124)));
            //銘大士林校區           );
            // {28, 121.528570, 25.086284},
       //     ZoomToLevel(19, ConvertMapPointTo102100(new MapPoint(121.528570, 25.086284)));
        }

        private void map_Loaded(object sender, RoutedEventArgs e)
        {
            //ZoomToLevel(17, ConvertMapPointTo102100(new MapPoint(118.402180,24.501124))
            //      );
        }
    }
}
