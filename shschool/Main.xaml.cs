using CeraDevices;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace shschool
{
    /// <summary>
    /// Main.xaml 的互動邏輯
    /// </summary>
    public partial class Main : Page
    {

        CeraDevices.CoordinatorDevice coor;
        System.Collections.Generic.Dictionary<string, StreetLightBindingData> dictStreetLightBindingInfos = new Dictionary<string, StreetLightBindingData>();
        System.Collections.Generic.Dictionary<string, StreetLightBindingData> dictStreetLightBindingInfosOriginal = new Dictionary<string, StreetLightBindingData>();
        IValueConverter converter = new IsEnableToColorConverter();
        System.Windows.Threading.DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();
      public   Config LedConfig;
        public Main()
        {
            InitializeComponent();
            coor = ((App.Current) as App).dev;
            
        //    this.ledBtn.Foreground = new SolidColorBrush(Colors.White);
          //this.ledBtn.Text = this.ledBtn.Text;
          //  this.ledBtn.Foreground = this.ledBtn.Foreground;
         

        }

       

        async void Initial()
        {
            #if !DEBUG
          
               await   GetDeviceInfoAndSetBindingData();
               if ((App.Current as App).IsStart)
               {
                   SetAllDeviceTo100();
                   (App.Current as App).IsStart = false;
               }
            tmr.Interval = TimeSpan.FromSeconds(5);
            tmr.Tick += tmr_Tick;
            tmr.Start();
        #endif
        }

        bool IsInTimer = false;
        void tmr_Tick(object sender, EventArgs e)
        {
            try
            {
                if (IsInTimer||IsinSetting)
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

           void SetAllDeviceTo100()
        {
            try
            {
               
                IsinSetting = true;
               
                foreach (StreetLightBindingData data in this.dictStreetLightBindingInfos.Values)
                {
                    if (data.IsEnable)
                    {
                        
                        coor.SetDeviceDimLevel(data.DevID, 100);
                        data.DimLevel = 100;
                    }
                    else
                    {
                        data.DimLevel = 100;
                    }
                }
            }
            catch
            {
            }
            finally
            {
                IsinSetting = false;
            }

        }

           void SetAllDeviceTo0()
           {
               try
               {
                   IsinSetting = true;
                   foreach (StreetLightBindingData data in this.dictStreetLightBindingInfos.Values)
                   {
                       if (data.IsEnable)
                       {
                           coor.SetDeviceDimLevel(data.DevID, 0);
                           data.DimLevel = 0;
                       }
                       else
                           data.DimLevel = 0;
                   }
               }
               catch
               {
               }
               finally
               {
                   IsinSetting = false;
               }
           }
           void SetAllDeviceTo20()
           {
               try
               {
                   IsinSetting = true;
                   foreach (StreetLightBindingData data in this.dictStreetLightBindingInfos.Values)
                   {
                       if (data.IsEnable)
                       {
                           coor.SetDeviceDimLevel(data.DevID, 20);
                           data.DimLevel = 20;
                       }
                       else
                           data.DimLevel = 20;
                   }
               }
               catch
               {
               }
               finally
               {
                   IsinSetting = false;
               }

           }

        async  Task GetDeviceInfoAndSetBindingData()
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


            foreach (DeviceInfo info in infos )
            {
                if (info == null)
                    continue;
                if (dictStreetLightBindingInfos.ContainsKey(info.addr))
                {
#if Version1
                    dictStreetLightBindingInfos[info.addr].IsEnable = info.visibility;
#else
                    dictStreetLightBindingInfos[info.addr].IsEnable = info.visibility==1;
#endif

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
        //void btn_Click(object sender, RoutedEventArgs e)
        //{
        //    StreetLightBindingDataGroup group = new StreetLightBindingDataGroup()
        //    {
        //        DimLevel = ((sender as Button).DataContext as StreetLightBindingData).DimLevel,
        //        BindingDatas = new StreetLightBindingData[] { (sender as Button).DataContext as StreetLightBindingData }

        //    };
        //  this.NavigationService.Navigate( new GroupDevice(group));
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Collections.Generic.List<StreetLightBindingData> list = new List<StreetLightBindingData>();
            foreach (UIElement element in this.LayoutRoot.Children)
            {

                if (element is LedButton)
                {
                    LedButton btn = element as LedButton;
                    if (btn.IsChecked)
                    {
                        StreetLightBindingData data = (element as LedButton).DataContext as StreetLightBindingData;
                        list.Add(data);
                    }
                    

                }
            }

            if (list.Count == 0)
                return;
            StreetLightBindingDataGroup group = new StreetLightBindingDataGroup()
            {
                DimLevel = 20,
                BindingDatas = list.ToArray()

            };
            this.tmr.Stop();
            this.NavigationService.Navigate(new GroupDevice(group));
        }

        private void Button_Click_All(object sender, RoutedEventArgs e)
        {
            foreach (UIElement element in this.LayoutRoot.Children)
            {

                if (element is LedButton)
                {
                    StreetLightBindingData data = (element as LedButton).DataContext as StreetLightBindingData;
                    if (data.IsEnable)
                        (element as LedButton).IsChecked = true;

                }
            }
         
        }

        private void Button_Click_Inverse(object sender, RoutedEventArgs e)
        {
            foreach (UIElement element in this.LayoutRoot.Children)
            {

                if (element is LedButton)
                {
                    StreetLightBindingData data = (element as LedButton).DataContext as StreetLightBindingData;
                    if (data.IsEnable)
                        (element as LedButton).IsChecked = !(element as LedButton).IsChecked;

                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {



            if (App.Current.Properties.Contains("LightCollection"))
            {
                this.dictStreetLightBindingInfos = App.Current.Properties["LightCollection"] as Dictionary<string, StreetLightBindingData>;
                this.dictStreetLightBindingInfosOriginal = App.Current.Properties["LightCollectionOriginal"] as Dictionary<string, StreetLightBindingData>;
            }
            System.Collections.Generic.Dictionary<string, StreetLightBindingData> List = new Dictionary<string, StreetLightBindingData>(); System.Collections.Generic.Dictionary<string, StreetLightBindingData> list = new Dictionary<string, StreetLightBindingData>();
            if (!System.IO.File.Exists("config.xml"))
            {
                Config config = new Config();
                
                foreach (UIElement element in this.LayoutRoot.Children)
                {
                  
                    if (element is LedButton)
                    {
                        LedButton btn = element as LedButton;
                        list.Add(btn.Text, new StreetLightBindingData() { DevID = btn.Text,OriginalDevID=btn.Text });
                        btn.Tag = btn.Text;
                    }

                  
                }

                config.StreetLightBindingDatas = list.Values.ToArray();
                config.Scenariors = new List<Scenarior>();
                //System.Xml.Serialization.XmlSerializer sr = new System.Xml.Serialization.XmlSerializer(typeof(Config));
                //sr.Serialize(System.IO.File.OpenWrite("config.xml"), config);
                SaveConfig(config);
                LedConfig = config;
                this.listScene.ItemsSource = LedConfig.Scenariors;
            }
            else
            {
                //System.Xml.Serialization.XmlSerializer sr = new System.Xml.Serialization.XmlSerializer(typeof(Config));
                //Config config = sr.Deserialize(System.IO.File.OpenRead("config.xml")) as Config;
                LedConfig = LoadConfig();
                foreach (StreetLightBindingData data in LedConfig.StreetLightBindingDatas)
                    list.Add(data.OriginalDevID, data);

                this.listScene.ItemsSource = LedConfig.Scenariors;
            }

            foreach (UIElement element in this.LayoutRoot.Children)
            {

                if (element is LedButton)
                {
                    LedButton btn = element as LedButton;

                    StreetLightBindingData temp;
                    string devid = "";
                    btn.Tag = list[btn.Text].OriginalDevID;
                    devid = list[btn.Text].DevID;
                    if (!dictStreetLightBindingInfos.ContainsKey(devid))
                    {

                        temp = new StreetLightBindingData() { DevID = devid,OriginalDevID=btn.Tag.ToString() ,DimLevel = 20, IsEnable = false };

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
                    btn.SetBinding(LedButton.TextProperty, new Binding("DevID"));
                    btn.SetBinding(LedButton.IsEnabledProperty, new Binding("IsEnable"));
                    btn.SetBinding(LedButton.IsCheckedProperty, new Binding("IsChecked") { Mode = BindingMode.TwoWay });
                    // btn.Foreground = btn.Foreground;
                    btn.DataContext = temp;
                    btn.Text = temp.DevID;
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Tag.ToString() == "Hide")
            {
                (this.Resources["ShowScenarior"] as Storyboard).Begin();
                (sender as Button).Tag = "Show";
            }
            else
            {
                (this.Resources["stbHideScenarior"] as Storyboard).Begin();
                (sender as Button).Tag = "Hide";
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SetAllDeviceTo100();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            SetAllDeviceTo0();
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

        private void btnSaveScene_Click(object sender, RoutedEventArgs e)
        {

            
            wndSelectSceneName dialog = new wndSelectSceneName();

            if (dialog.ShowDialog() == true)
            {
                Scenarior scene = new Scenarior() { SceneName = dialog.SceneName };

                scene.List = new List<StreetLightBindingData>();
                foreach (StreetLightBindingData data in dictStreetLightBindingInfos.Values)
                {
                    scene.List.Add(new StreetLightBindingData() { DevID = data.DevID, OriginalDevID = data.OriginalDevID, DimLevel = data.DimLevel });
                }

                LedConfig.Scenariors.Add(scene);
                SaveConfig(LedConfig);
                this.listScene.ItemsSource = null;
                this.listScene.ItemsSource = LedConfig.Scenariors;
            }

        }

        //private void Button_Click_5(object sender, RoutedEventArgs e)
        //{
        //    if (listScene.SelectedItem == null)
        //    {
        //        MessageBox.Show("請先選取情境");
        //        return;
        //    }

        //    Scenarior scene = listScene.SelectedItem as Scenarior;
        //    foreach (StreetLightBindingData data in scene.List)
        //    {

        //        StreetLightBindingData outputled = dictStreetLightBindingInfosOriginal[data.OriginalDevID];
        //        if(outputled.DimLevel != data.DimLevel)
        //        {
        //            outputled.DimLevel = data.DimLevel;
        //            if (outputled.IsEnable)
        //                coor.SetDeviceDimLevel(data.DevID, data.DimLevel);
        //        }
                
        //    }
        //}

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (chkEdit.IsChecked != true)
                return;
          
            if (listScene.SelectedItem == null)
            {
                MessageBox.Show("請先選取情境");
                return;
            }

            
            Scenarior scene = listScene.SelectedItem as Scenarior;
            if (MessageBox.Show("確定要刪除情境-" + scene.SceneName, "系統", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;
            this.LedConfig.Scenariors.Remove(scene);
            listScene.ItemsSource = null;
            listScene.ItemsSource = LedConfig.Scenariors;
            SaveConfig(LedConfig);
        }

        private void Grid_TouchDown(object sender, TouchEventArgs e)
        {

        }

        bool IsinSetting = false;
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Scenarior scene = (sender as Grid).DataContext as Scenarior;
            try
            {
                listScene.IsEnabled = false;
                IsinSetting = true;
                foreach (StreetLightBindingData data in scene.List)
                {

                    StreetLightBindingData outputled = dictStreetLightBindingInfosOriginal[data.OriginalDevID];
                    if (outputled.DimLevel != data.DimLevel)
                    {
                        outputled.DimLevel = data.DimLevel;
                        if (outputled.IsEnable)
                        {
                            try
                            {
                                coor.SetDeviceDimLevel(outputled.DevID, data.DimLevel);
                            }
                            catch { ;}
                        }

                    }

                }
            }
            catch { ;}
            finally
            {
                IsinSetting = false; ;
                listScene.IsEnabled = true;
            }
        }

         void CheckLedOutput()
       {
           StreetLightInfo[] infos=null;

            try
            {
                 infos =  coor.GetStreetLightList();
            }
            catch
            {

            }
           if(infos!=null)
            foreach (StreetLightInfo data in infos)
            {
                if (!dictStreetLightBindingInfos.ContainsKey(data.DevID))
                    continue;
               
                if ( (dictStreetLightBindingInfos[data.DevID]).IsEnable && data.CurrentDimLevel != (dictStreetLightBindingInfos[data.DevID]).DimLevel    )
                {
                    try
                    {
                        coor.SetDeviceDimLevel(data.DevID, (dictStreetLightBindingInfos[data.DevID]).DimLevel);
                    }
                    catch { ;}
                }
            }
        }

       private void Page_Unloaded(object sender, RoutedEventArgs e)
       {
           this.tmr.Stop();
       }

       private void Unselect_click(object sender, RoutedEventArgs e)
       {
           foreach (UIElement element in this.LayoutRoot.Children)
           {

               if (element is LedButton)
               {
                   StreetLightBindingData data = (element as LedButton).DataContext as StreetLightBindingData;
                   if (data.IsEnable)
                       (element as LedButton).IsChecked = false;

               }
           }
       }

       WrapPanel myWrapnel;
       private void WrapPanel_Load(object sender, RoutedEventArgs e)
       {
           myWrapnel = sender as WrapPanel;
           myWrapnel.Width = listScene.ActualWidth;
       }

       
    }
}
