using CeraDevices;
using System;
using System.Collections.Generic;
using System.Linq;
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

        CeraDevices.CoordinatorDevice coor;
        System.Collections.Generic.Dictionary<string, StreetLightBindingData> dictStreetLightBindingInfos = new Dictionary<string, StreetLightBindingData>();
        System.Collections.Generic.Dictionary<string, StreetLightBindingData> dictStreetLightBindingInfosOriginal = new Dictionary<string, StreetLightBindingData>();
        System.Windows.Threading.DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();
        Config LedConfig;
        IValueConverter converter = new IsEnableToColorConverter();
        public MainWindow()
        {
            InitializeComponent();
            coor = ((App.Current) as App).dev;
            InitLed2dPositoin();

        }


        void InitLed2dPositoin()
        {
            int LedWidth = 30;
            for(int i=0;i<App.Light2DInfo.GetLength(0);i++)
            {
                
                   
                    CheckBox chk = new CheckBox();
                    chk.Width = chk.Height = LedWidth;
                    chk.Style = this.FindResource("CheckBoxStyle1") as Style;
                    chk.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    chk.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    int x, y;
                    x=int.Parse(App.Light2DInfo[i,1]);
                      y=int.Parse(App.Light2DInfo[i,2]);
                    chk.Margin = new Thickness( x- LedWidth / 2, y - LedWidth / 2, 0, 0);
                    chk.Content = App.Light2DInfo[i,0];
                    chk.MouseRightButtonUp += chk_MouseRightButtonUp;

                   // chk.Background = new SolidColorBrush(Colors.Yellow);
                   // chk.Foreground = new SolidColorBrush(Colors.Red);
                    this.grdDeviceLayer.Children.Add(chk);
                }
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
            tmr.Interval = TimeSpan.FromSeconds(5);
            tmr.Tick += tmr_Tick;
            tmr.Start();
#endif
        }

        bool IsinSetting = false;
        bool IsInTimer = false;
        void tmr_Tick(object sender, EventArgs e)
        {
            try
            {
                if (IsInTimer || IsinSetting)
                    return;
                IsInTimer = true;
                GetDeviceInfoAndSetBindingData();

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
                    infos = await coor.GetDeviceListAsync();
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
            //StreetLightInfo[] street_light_info=null;
            //try
            //{
            //     street_light_info = await coor.GetStreetLightListAsync();
            //}
            //catch { }
            //if(street_light_info!=null)
            //foreach (StreetLightInfo data in street_light_info)
            //{
            //    if (dictStreetLightBindingInfos.ContainsKey(data.DevID))
            //    {
            //        dictStreetLightBindingInfos[data.DevID].DimLevel = data.CurrentDimLevel;
            //        dictStreetLightBindingInfos[data.DevID].IsEnable = true;
            //    }
            //}
            //var q = from n in dictStreetLightBindingInfos.Values where !street_light_info.Select(k=>k.DevID).ToArray().Contains(n.DevID) select n;
            //foreach (StreetLightBindingData data in q)
            //    data.IsEnable = false;
        }
        void CheckLedOutput()
        {
            StreetLightInfo[] infos = null;
            // temp for return
        
            try
            {
                infos = coor.GetStreetLightList();
            }
            catch
            {

            }
            if (infos != null)
                foreach (StreetLightInfo data in infos)
                {
                    if (!dictStreetLightBindingInfos.ContainsKey(data.DevID))
                        continue;

                    if ((dictStreetLightBindingInfos[data.DevID]).IsEnable && data.CurrentDimLevel != (dictStreetLightBindingInfos[data.DevID]).DimLevel)
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
            if (vbImage == null) return;

            // vbImage.Width=scrollViewer1.ViewportWidth;
            // vbImage.Height = scrollViewer1.ViewportHeight ;
            // vbImage.Margin = new Thickness(0);
            vbImage.Stretch = Stretch.Uniform;
            scrollViewer1_SizeChanged(null, null);
            //   imgPic.Height = vbImage.ActualHeight;
            //vbImage.UpdateLayout();
        }

        private void rdNone_Checked(object sender, RoutedEventArgs e)
        {
            vbImage.Stretch = Stretch.None;
            scrollViewer1_SizeChanged(null, null);
            //vbImage.Width = imgPic.Width;
            //vbImage.Height = imgPic.Height;
            //vbImage.Margin = new Thickness(0);
            //vbImage.UpdateLayout();
        }

        private void scrollViewer1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (rdNone.IsChecked == true)
            {
                vbImage.Stretch = Stretch.None;
                vbImage.Width = imgPic.Width;
                vbImage.Height = imgPic.Height;
                vbImage.Margin = new Thickness(0);
                vbImage.UpdateLayout();
            }
            else
            {
                vbImage.Width = scrollViewer1.ViewportWidth;
                vbImage.Height = scrollViewer1.ViewportHeight;
                vbImage.Margin = new Thickness(0);
                vbImage.Stretch = Stretch.Uniform;
            }

        }

        private void bdrOverView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(sender as IInputElement);
            double horoffset, veroffset;
            horoffset = p.X / bdrOverView.ActualWidth * imgPic.ActualWidth;
            veroffset = p.Y / bdrOverView.ActualHeight * imgPic.ActualHeight;
            scrollViewer1.ScrollToHorizontalOffset(horoffset);
            scrollViewer1.ScrollToVerticalOffset(veroffset);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            coor.SetDeviceRTC("*", DateTime.Now);
            coor.SetDeviceScheduleEnable("*", true);
            if (App.Current.Properties.Contains("LightCollection"))
            {
                this.dictStreetLightBindingInfos = App.Current.Properties["LightCollection"] as Dictionary<string, StreetLightBindingData>;
                this.dictStreetLightBindingInfosOriginal = App.Current.Properties["LightCollectionOriginal"] as Dictionary<string, StreetLightBindingData>;
            }
            System.Collections.Generic.Dictionary<string, StreetLightBindingData> List = new Dictionary<string, StreetLightBindingData>(); System.Collections.Generic.Dictionary<string, StreetLightBindingData> list = new Dictionary<string, StreetLightBindingData>();
            if (!System.IO.File.Exists("config.xml"))
            {
                Config config = new Config();

                foreach (UIElement element in this.grdDeviceLayer.Children)
                {

                    if (element is CheckBox)
                    {
                        CheckBox btn = element as CheckBox;
                        list.Add(btn.Content.ToString(), new StreetLightBindingData() { DevID = btn.Content.ToString(), OriginalDevID = btn.Content.ToString() });
                        btn.Tag = btn.Content                            ;
                    }


                }

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

            foreach (UIElement element in this.grdDeviceLayer.Children)
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

                        temp = new StreetLightBindingData() { DevID = devid, OriginalDevID = btn.Tag.ToString(), DimLevel = 20, IsEnable = false };

                        dictStreetLightBindingInfos.Add(temp.DevID, temp);
                        dictStreetLightBindingInfosOriginal.Add(temp.OriginalDevID, temp);
                    }
                    else

                        temp = dictStreetLightBindingInfos[devid] as StreetLightBindingData;



                    //   temp.IsEnable = true;

                    Binding binding = new Binding() { Path = new PropertyPath("IsEnable"), Converter = converter };

                    btn.SetBinding(Button.ForegroundProperty, binding);
                    //   btn.SetBinding(ForegroundProperty, binding);
                    // btn.Content = null;
                    btn.SetBinding(CheckBox.ContentProperty, new Binding("DevID"));
                    btn.SetBinding(CheckBox.IsEnabledProperty, new Binding("IsEnable"));
                    btn.SetBinding(CheckBox.IsCheckedProperty, new Binding("IsChecked") { Mode = BindingMode.TwoWay });
                    // btn.Foreground = btn.Foreground;
                    btn.DataContext = temp;
                    btn.Content = temp.DevID;
                    //      btn.Text = btn.Text;
                    btn.IsChecked = temp.IsChecked;
                    //   btn.IsChecked = false;
                }
                //if (element is Button)
                //{
                //    try
                //    {
                //        Button btn = (Button)element;
                //        btn.Name = "btn" + btn.Content;
                //        //   btn.Width = btn.Height = 84;
                //        btn.Click += btn_Click;
                //        StreetLightBindingData temp = new StreetLightBindingData() { DevID = btn.Content.ToString(), DimLevel = 20, IsEnable = false };
                //        dictStreetLightBindingInfos.Add(btn.Content.ToString(), temp);


                //        //   temp.IsEnable = true;
                //        btn.DataContext = temp;

                //        Binding binding = new Binding() { Path = new PropertyPath("IsEnable"), Converter = converter };

                //        btn.SetBinding(Button.ForegroundProperty, binding);
                //        //   btn.SetBinding(ForegroundProperty, binding);
                //        btn.Content = null;
                //        btn.SetBinding(Button.ContentProperty, new Binding("DevID"));
                //        btn.SetBinding(Button.IsEnabledProperty, new Binding("IsEnable"));
                //        //  btn.GetBindingExpression(ForegroundProperty).UpdateSource();

                //    }
                //    catch (Exception ex)
                //    {
                //        System.Diagnostics.Debug.Print(ex.Message);
                //    }
                //}


            } // foeach

            App.Current.Properties["LightCollection"] = dictStreetLightBindingInfos;
            App.Current.Properties["LightCollectionOriginal"] = dictStreetLightBindingInfosOriginal;
            Initial();
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
            foreach (UIElement element in grdDeviceLayer.Children)
            {
                if (element is CheckBox)
                {
                    if( (element as CheckBox).IsEnabled)
                           (element as CheckBox).IsChecked = true;
                }
            }
        }

        

        private void btnTemplateEdit_Click(object sender, RoutedEventArgs e)
        {
            new wndTempleateEdit(LedConfig.Scenariors).ShowDialog();
            SaveConfig(this.LedConfig);
        }
    }
}
