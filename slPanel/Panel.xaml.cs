using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.ServiceModel.DomainServices.Client;
using slPanel.Web;
using System.IO;
//using System.Windows.Interactivity;
using Microsoft.Expression.Interactivity.Layout;
using UIDevices;
 
using System.Windows.Threading;
//using Microsoft.Windows;

namespace slPanel
{
    public partial class Panel : Page,ControlService.IControlServiceCallback
    {
        slPanel.Web.DomainService1 dbservice = new Web.DomainService1();
      
        ControlService.ControlServiceClient controlService;
          int ProjectID=-1;
        string UserID;
        System.Windows.Threading.DispatcherTimer tmr = new System.Windows.Threading.DispatcherTimer();
        public Panel()
        {
            //  HierarchicalDataTemplate
            InitializeComponent();
            //  this.vbImage.GetBindingExpression(Viewbox.DataContextProperty).
            //  dbservice = this.Resources["dbservice"] as DomainService1;
            lnkSave.DataContext = lnkUndo.DataContext = dbservice;
            dbservice.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(dbservice_PropertyChanged);
            controlService = new ControlService.ControlServiceClient
               
            (new System.ServiceModel.InstanceContext(this), "CustomBinding_IControlService", "net.tcp://"+App.Current.Host.Source.Host+":4502/ControlService");

            controlService.RegistAsync();

            tmr.Interval =  TimeSpan.FromSeconds(60);
            tmr.Tick += new EventHandler(tmr_Tick);
            tmr.Start();
               
        }

        void tmr_Tick(object sender, EventArgs e)
        {
            if (controlService.State == System.ServiceModel.CommunicationState.Opened)
            {
                controlService.ToServerSayHelloAsync();
            }
            else
            {
                controlService = new ControlService.ControlServiceClient

            (new System.ServiceModel.InstanceContext(this), "CustomBinding_IControlService", "net.tcp://" + App.Current.Host.Source.Host + ":4502/ControlService");
            }
            //throw new NotImplementedException();
        }

        void dbservice_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "HasChanges")
            {
                if (dbservice.HasChanges)
                {
                    VisualStateManager.GoToState(lnkSave, "HasChanges", false);
                    VisualStateManager.GoToState(this.lnkUndo, "HasChanges", false);
                }
                else
                {
                    VisualStateManager.GoToState(lnkSave, "NoChanges", false);
                    VisualStateManager.GoToState(lnkUndo, "NoChanges", false);
                }


            }
            //throw new NotImplementedException();
        }

        // 使用者巡覽至這個頁面時執行。

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {



            if (this.dbservice.HasChanges)
            {
                MessageBox.Show("資料變更尚未儲存!");
                e.Cancel = true;
            }

            base.OnNavigatingFrom(e);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (this.NavigationContext.QueryString.ContainsKey("UserID"))
            {
                //  MessageBox.Show("無專案編號!");
                // this.NavigationService.GoBack();
                this.UserID = this.NavigationContext.QueryString["UserID"].ToString();
            }
            //  ProjectID = int.Parse(this.NavigationContext.QueryString["ProjectID"]);
            //  ProjectName = this.NavigationContext.QueryString["ProjectName"];
            //   this.textBlock1.Text = ProjectName + " 群組";

            EntityQuery<tblProject> qry = dbservice.GetTblProjectWithIncludeQuery(); //dbservice.GetTblProjectQuery();
            LoadOperation<tblProject> lo = dbservice.Load(qry);
            lo.Completed += (s, a) =>
                {
                    if (lo.Entities.Count() == 0)
                    {
                        MessageBox.Show("資料庫無專案資料");
                        return;
                    }
                    this.DataContext = lo.Entities.ToArray()[0]; ;
                    this.ProjectID = lo.Entities.ToArray()[0].ProjectID;
                    controlService.GetAllLEDDeviceInfoCompleted+=(ss,aa)=>
                        {

                            foreach (slPanel.ControlService.LedDevice dev in aa.Result)
                            {
                                tblProjectGroupSection section = this.dbservice.tblProjectGroupSections.Where(n => n.SectionID == dev.SectionID).FirstOrDefault();
                                if (section == null)
                                    return;
                                foreach (tblDevice tbldev in section.tblDevice)
                                {
                                    if (tbldev.ZeeBeeID == dev.ZeeBeeID)
                                    {
                                        tbldev.W = dev.W;
                                        tbldev.R = dev.R;
                                        tbldev.G = dev.G;
                                        tbldev.B = dev.B;
                                        tbldev.IsConnected = dev.IsConnected;
                                    }
                                }

                            }
                        };
                    controlService.GetAllLEDDeviceInfoAsync();

                    //controlService.GetAllLEDDeviceOutputCompleted += (ss, aa) =>
                    //{

                    //    foreach (slPanel.ControlService.LedOutputData outdata in aa.Result)
                    //    {
                    //        tblProjectGroupSection section = this.dbservice.tblProjectGroupSections.Where(n => n.SectionID == outdata.SectionID).FirstOrDefault();
                    //        if (section == null)
                    //            return;
                    //        foreach (tblDevice dev in section.tblDevice)
                    //        {
                    //            dev.W = outdata.W;
                    //            dev.R = outdata.R;
                    //            dev.G = outdata.G;
                    //            dev.B = outdata.B;

                    //        }

                    //    }
                    //};
                    //controlService.GetAllLEDDeviceOutputAsync();



                };




        }




        //private void MenuGroupDel_Click(object sender, RoutedEventArgs e)
        //{
        //    tblProjectGroup group = this.treeView1.SelectedItem as tblProjectGroup;

        //         dbservice.tblProjectGroups.Remove(group);

        //}



        StackPanel currentGroupStack = null;
        //private void MenuGroupReName_Click(object sender, RoutedEventArgs e)
        //{

        //    StackPanel stk = currentGroupStack;
        //    TextBox tb = stk.FindName("txtGroupName") as TextBox;
        //    TextBlock tblock = stk.FindName("tblkGroupName") as TextBlock;
        //    tblock.Visibility = System.Windows.Visibility.Collapsed;
        //    tb.Visibility = Visibility.Visible;
        //    tb.Focus();

        //}



        //private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        //{

        //currentGroup =   (sender as  StackPanel).DataContext as tblProjectGroup;
        //currentGroupStack =  sender as StackPanel;
        // treeView1.SelectItem(currentGroup);
        //}

        //private void txtGroupName_LostFocus(object sender, RoutedEventArgs e)
        //{

        //    StackPanel stk = (sender as TextBox).Parent as StackPanel;
        //    TextBox tb = stk.FindName("txtGroupName") as TextBox;
        //    TextBlock tblock = stk.FindName("tblkGroupName") as TextBlock;
        //    tblock.Visibility = System.Windows.Visibility.Visible;
        //    tb.Visibility = Visibility.Collapsed;

        //    
        //}




        public static void SendFile(slPanel.Web.tblProjectGroup group, int projectid)
        {
            System.Windows.Controls.OpenFileDialog d = new OpenFileDialog();
            d.Filter = "Image (*.png, *.jpg) |*.png;*.jpg";
            //  d.ShowDialog();
            //try
            //{
            //    string filename = d.File.FullName;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    return;
            //}
            if (d.ShowDialog() == true)
            {


                Stream stream = d.File.OpenRead();

                Uri u = new Uri(Application.Current.Host.Source, "../Upload.ashx/?filename=" + d.File.Name + "&projectid=" + projectid);

                // MessageBox.Show(u.ToString());
                byte[] buffer = new byte[d.File.Length];
                stream.Read(buffer, 0, (int)d.File.Length);
                stream.Close();
                WebClient wc = new WebClient();
                wc.OpenWriteCompleted += (s, ee) =>
                {
                    if (ee.Error != null)
                    {
                        MessageBox.Show(ee.Error.Message);
                        return;
                    }
                    Stream outputStream = ee.Result;
                    outputStream.Write(buffer, 0, buffer.Length);
                    outputStream.Close();
                    MessageBox.Show("檔案上傳完成!");

                    group.GroupPicture = d.File.Name;
                    group.IsPictureDownload = true;

                    //System.Windows.Media.Imaging.BitmapImage img = new System.Windows.Media.Imaging.BitmapImage(new Uri(group.PicUrl));


                };
                wc.OpenWriteAsync(u);



            }
        }

        private void hyperlinkButton1_Click(object sender, RoutedEventArgs e)
        {

            if (this.treeView1.Visibility == System.Windows.Visibility.Visible)
            {
                this.treeView1.Visibility = System.Windows.Visibility.Collapsed;
                this.hyperlinkButton1.Content = ">>";
            }
            else
            {
                this.treeView1.Visibility = System.Windows.Visibility.Visible;
                this.hyperlinkButton1.Content = "<<";
            }
            //chldNewGroup child = new chldNewGroup();
            //child.DataContext = new tblProjectGroup()
            //{
            //    // GroupName="",
            //     ProjectID=ProjectID
            //};

            //child.Closed += (s, a) =>
            //    {
            //        if (child.DialogResult != true)
            //            return;
            //        dbservice.tblProjectGroups.Add(child.DataContext as tblProjectGroup);


            //    };
            //child.Show();
        }

        tblProjectGroup currentGroup = null;
        private void treeView1_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView tv = (sender as TreeView);
            if (tv.SelectedItem == null)
                return;
            if (tv.SelectedItem is tblProjectGroup)
            {

                if (currentGroup != tv.SelectedItem as tblProjectGroup)
                {
                    currentGroup = tv.SelectedItem as tblProjectGroup;
                    DrawDevices();
                }
            }
            else if (tv.SelectedItem is tblDevice)
            {
                if (currentGroup != (tv.SelectedItem as tblDevice).tblProjectGroupSection.tblProjectGroup)
                {
                    currentGroup = (tv.SelectedItem as tblDevice).tblProjectGroupSection.tblProjectGroup;
                    DrawDevices();
                }

                UnSelectAllDevice();
                (tv.SelectedItem as tblDevice).UIDevice.IsSelected = true;

                scrollViewer1.ScrollIntoView((tv.SelectedItem as tblDevice).UIDevice as FrameworkElement);
            }
            else  //section seclect
            {
                if (currentGroup != (tv.SelectedItem as tblProjectGroupSection).tblProjectGroup)
                {
                    currentGroup = (tv.SelectedItem as tblProjectGroupSection).tblProjectGroup;
                    DrawDevices();
                }
                UnSelectAllDevice();
                foreach (tblDevice dev in (tv.SelectedItem as tblProjectGroupSection).tblDevice)
                {
                    if (dev.UIDevice != null)
                        dev.UIDevice.IsSelected = true;
                }


            }


        }

        public void DrawDevices()
        {
            // grdDeviceLayer.Children.Clear();
            UIElement[] elements = grdDeviceLayer.Children.ToArray();
            foreach (UIElement element in elements)
            {
                if (element is Image)
                    continue;
                grdDeviceLayer.Children.Remove(element);
            }
            if (currentGroup == null) return;
            //modify here 2/18

            foreach (tblProjectGroupSection sec in currentGroup.tblProjectGroupSection)
                foreach (tblDevice dev in sec.tblDevice)
                {
                    if (dev.DevType == "LED")
                    {
                        UIDevices.UILed UILedDev = new UIDevices.UILed();
                        UILedDev.Name = "LED" + dev.DeviceID;
                        UILedDev.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        UILedDev.VerticalAlignment = System.Windows.VerticalAlignment.Top;

                        UILedDev.Margin = new Thickness(dev.X, dev.Y, 0, 0);
                        UILedDev.Width = 20;
                        UILedDev.Height = 30;
                        UILedDev.Visibility = System.Windows.Visibility.Visible;
                        UILedDev.SetValue(Canvas.ZIndexProperty, 10);
                        UILedDev.DataContext = dev;
                        dev.UIDevice = UILedDev;
                        grdDeviceLayer.Children.Add(UILedDev);
                        UILedDev.OnSelected += new EventHandler(UILedDev_OnSelected);
                        UILedDev.MouseLeftButtonDown += new MouseButtonEventHandler(UILed_MouseLeftButtonDown);
                        UILedDev.MouseLeftButtonUp += new MouseButtonEventHandler(UILed_MouseLeftButtonUp);
                    }
                    else if (dev.DevType == "ROUTER")
                    {
                        UIDevices.UIRouter UIRRouter = new UIDevices.UIRouter();
                        UIRRouter.Name = "ROUTER" + dev.DeviceID;
                        UIRRouter.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        UIRRouter.VerticalAlignment = System.Windows.VerticalAlignment.Top;

                        UIRRouter.Margin = new Thickness(dev.X, dev.Y, 0, 0);
                        UIRRouter.Width = 20;
                        UIRRouter.Height = 30;
                        UIRRouter.Visibility = System.Windows.Visibility.Visible;
                        UIRRouter.SetValue(Canvas.ZIndexProperty, 10);
                        UIRRouter.DataContext = dev;
                        dev.UIDevice = UIRRouter;
                        grdDeviceLayer.Children.Add(UIRRouter);
                        UIRRouter.OnSelected += new EventHandler(UILedDev_OnSelected);
                        UIRRouter.MouseLeftButtonDown += new MouseButtonEventHandler(UILed_MouseLeftButtonDown);
                        UIRRouter.MouseLeftButtonUp += new MouseButtonEventHandler(UILed_MouseLeftButtonUp);
                    
                    }

                }
        }

        void UILedDev_OnSelected(object sender, EventArgs e)
        {

            //   slider1.DataContext = (sender as UILed).DataContext;

        }



        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
               if (dbservice.tblSectionLedOneTimePlans.HasChanges || dbservice.tblSectionLedPlans.HasChanges)
                    MessageBox.Show("排程已改變，將自動儲存新的排程設定並啟用新的排程狀態");
                this.dbservice.SubmitChanges();
                  MessageBox.Show("儲存成功");
                DispatcherTimer tmr = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(3) };
                tmr.Tick += (s,a) =>
                {

                    controlService.ReloadScheduleAsync();
                    tmr.Stop();
                };
                tmr.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "," + ex.StackTrace);
            }

        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("確定要還原變更", "LedProjectMaker", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    return;
                this.dbservice.RejectChanges();
                DrawDevices();
                MessageBox.Show("資料已還原!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "," + ex.StackTrace);
            }
        }

        //private void MenuLedDeviceAdd_Click(object sender, RoutedEventArgs e)
        //{
        //    tblProjectGroupSection section = (sender as MenuItem).DataContext as tblProjectGroupSection;
        //   //modify here 2/18
        //    tblDevice dev = new tblDevice() {ColorType="MONO", Shape="CIRCLE",Rotation=0, DeviceID = GetMaxDevciceID(), DevType = "LED", ProjectID=section.ProjectID,GroupID=section.GroupID,SectionID=section.SectionID };
        //    section.tblDevice.Add(dev);
        //    DrawDevices();

        //}


        int GetMaxDevciceID()
        {

            if (dbservice.tblDevices.Count == 0)
                return 1;
            else

                return dbservice.tblDevices.Max(c => c.DeviceID) + 1;


        }

        private void imgPic_MouseMove(object sender, MouseEventArgs e)
        {
            Point pimage = e.GetPosition(this.imgPic);
            textBlock2.Text = string.Format("x:{0:0.0},y:{1:0.00}", pimage.X, pimage.Y);


            // disable adjust device position
            //if (selectedDevice == null)
            //    return;
            // Point  p= e.GetPosition(grdDeviceLayer);
            // selectedDevice.SetValue(Grid.MarginProperty,
            //     new Thickness(p.X - deltaX, p.Y - deltaY, 0, 0));
            // (selectedDevice.DataContext as tblDevice).X =(int)( p.X - deltaX);
            // (selectedDevice.DataContext as tblDevice).Y = (int)(p.Y - deltaY);
            // selectedDevice.SetValue(Grid.WidthProperty, 20.0);
            // selectedDevice.SetValue(Grid.HeightProperty, 30.0);



        }

        Control selectedDevice;
        double deltaX, deltaY;  //計算滑鼠落點與元件座標差值

        private void UILed_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectedDevice = sender as Control;
            //  selectedDevice.CaptureMouse();
            deltaX = e.GetPosition(grdDeviceLayer).X - ((Thickness)selectedDevice.GetValue(Grid.MarginProperty)).Left;
            deltaY = e.GetPosition(grdDeviceLayer).Y - ((Thickness)selectedDevice.GetValue(Grid.MarginProperty)).Top;

            UnSelectAllDevice();
            (sender as IUIDevice).IsSelected = true;
        }

        void UnSelectAllDevice()
        {
            foreach (UIElement element in grdDeviceLayer.Children)
            {
                if (element is UIDevices.IUIDevice)
                {

                    (element as UIDevices.IUIDevice).IsSelected = false;
                }

            }


        }

        private void UILed_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // selectedDevice.ReleaseMouseCapture();
            // selectedDevice = null;
            deltaX = 0;
            deltaY = 0;

        }

        //private void DelDevice_Click(object sender, RoutedEventArgs e)
        //{
        //    if (treeView1.SelectedItem is tblDevice)
        //    {
        //        this.dbservice.tblDevices.Remove(treeView1.SelectedItem as tblDevice);
        //        DrawDevices();
        //    }
        //}



        StackPanel currentDevciePanel;
        private void device_StackPanel_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentDevciePanel = sender as StackPanel;
        }

        //private void ChangeZeeBeeID_Click(object sender, RoutedEventArgs e)
        //{
        //    if (currentDevciePanel == null) return;
        //    (currentDevciePanel.FindName("tbZeeBeeID") as TextBlock).Visibility = System.Windows.Visibility.Collapsed;
        //    (currentDevciePanel.FindName("txtZeeBeeID") as TextBox).Visibility = System.Windows.Visibility.Visible;
        //}

        //private void txtZeeBeeID_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    TextBox txtB = sender as TextBox;
        //    tblDevice dev = txtB.DataContext as tblDevice;
        //    txtB.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        //    int cnt = (from n in dbservice.tblDevices where n.DeviceID == int.Parse(txtB.Text) select n).Count();
        //    if (cnt > 1)
        //    {
        //        dev.ValidationErrors.Add(new System.ComponentModel.DataAnnotations.ValidationResult("DevcieID " + txtB.Text + "  已經存在!"));


        //        MessageBox.Show("DevcieID " + txtB.Text + "  已經存在!");
        //        return;
        //    }
        //    ((txtB.Parent as StackPanel).FindName("tbZeeBeeID") as TextBlock).Visibility = System.Windows.Visibility.Visible;
        //    ((txtB.Parent as StackPanel).FindName("txtZeeBeeID") as TextBox).Visibility = System.Windows.Visibility.Collapsed;


        //}

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

        private void StackPanel_MouseRightButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (currentGroup == null) return;
            treeView1.SelectItem((sender as StackPanel).DataContext);

        }



        //private void AddSection_Click(object sender, RoutedEventArgs e)
        //{
        //    //chldInputBox inputbox = new chldInputBox("LedProjectMaker", "請輸入控群名稱");
        //    //tblProjectGroup group = treeView1.SelectedItem as tblProjectGroup;
        //    //inputbox.Closed+=(s,a)=>
        //    //    {
        //    //        if(inputbox.DialogResult==true) 
        //    //            currentGroup.tblProjectGroupSection.Add(
        //    //            new tblProjectGroupSection() { ProjectID = group.ProjectID,
        //    //                                           GroupID = group.GroupID,
        //    //            SectionName = inputbox.InputString});
        //    //    };


        //    //inputbox.Show();


        //}



        //private void SectionDel_Click(object sender, RoutedEventArgs e)
        //{
        //    tblProjectGroupSection section = treeView1.SelectedItem as tblProjectGroupSection;
        //    if (section == null)
        //        return;
        //    dbservice.tblProjectGroupSections.Remove(section);

        //}



        private void Section_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {


      
            treeView1.SelectItem((sender as StackPanel).DataContext);

            currentGroup = ((sender as StackPanel).DataContext as tblProjectGroupSection).tblProjectGroup;
        }

        private void SectionRename_Click(object sender, RoutedEventArgs e)
        {
            //chldInputBox input = new chldInputBox("LedProjectMaker", "輸入新的群控名稱");
            //input.Closed += (s, a) =>
            //    {
            //        if (input.DialogResult == true)
            //        {
            //            (treeView1.SelectedItem as tblProjectGroupSection).SectionName = input.InputString;
            //        }
            //    };
            //input.Show();

        }

        private void SectionDailyPlan_Click(object sender, RoutedEventArgs e)
        {
            slPanel.Web.tblProjectGroupSection sec = (sender as MenuItem).DataContext as slPanel.Web.tblProjectGroupSection;
            if (sec == null) return;
            if (dbservice.HasChanges)
            {
                MessageBox.Show("請先完成儲存或復原");
                return;
            }
            chldLedPlanEdit child = new chldLedPlanEdit(dbservice){DataContext=sec};
            child.Closed += (s, a) =>
                {
                    if (child.DialogResult == false || child.DialogResult == null)
                        dbservice.RejectChanges();
                   
                };
            child.Show();
            
        }

        //private void RectangleLedDeviceAdd_Click(object sender, RoutedEventArgs e)
        //{
        //    tblProjectGroupSection section = (sender as MenuItem).DataContext as tblProjectGroupSection;
        //    //modify here 2/18
        //    tblDevice dev = new tblDevice() {ColorType="MONO", Shape = "RECTANGLE", Rotation = 0, DeviceID = GetMaxDevciceID(), DevType = "LED", ProjectID = section.ProjectID, GroupID = section.GroupID, SectionID = section.SectionID };
        //    section.tblDevice.Add(dev);
        //    DrawDevices();
        //}


        //private void DeviceMove_Click(object sender, RoutedEventArgs e)
        //{
        //  tblProjectGroupSection section=  ((sender as MenuItem).DataContext as tblProjectGroupSection);
        // tblDevice[] devices=  dbservice.tblDevices.Where(n => n.IsChecked && n.GroupID == section.GroupID && n.SectionID != section.SectionID).ToArray();
        // foreach (tblDevice dev in devices)
        // {
        //     dbservice.tblDevices.Remove(dev);
        //     tblDevice newdev = new tblDevice()
        //     {
        //          GroupID=section.GroupID,
        //          ProjectID=section.ProjectID,
        //          SectionID=section.SectionID,
        //           Rotation=dev.Rotation,
        //            IsChecked=false,
        //             DeviceID=dev.DeviceID,
        //              DevType=dev.DevType,
        //               UIDevice=dev.UIDevice,
        //                Shape=dev.Shape,
        //                 ZeeBeeID=dev.ZeeBeeID,
        //                 X=dev.X,
        //                 Y=dev.Y


        //     };
        //     section.tblDevice.Add(newdev);


        // }

        //}

        public void ToClientSayHello(string hello)
        {
            //throw new NotImplementedException();
        }


        public void ToClientNotifyLedDisplayChange(int DeviceID, ControlService.LedOutputData outdata)
        {
           tblDevice device = dbservice.tblDevices.Where(n => n.ZeeBeeID== DeviceID).FirstOrDefault();
           if (device == null)
               return;
           device.R = outdata.R;
           device.G = outdata.G;
           device.B = outdata.B;
           device.W = outdata.W;
           
          //  throw new NotImplementedException();
        }

        private void hyperlinkButton2_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            chldInputBox input = new chldInputBox("slPanel", "輸入伺服器 IP");
            input.Closed += (s, a) =>
                {
                    if (input.DialogResult == true)
                    {
                        slPanel.ControlService.ControlServiceClient client = new ControlService.ControlServiceClient();
                        client.ImportDevicesCompleted += (ss, aa) =>
                            {
                                if (aa.Error != null)
                                {

                                    MessageBox.Show("載入失敗"+aa.Error.Message);
                                    return;
                                }
                                MessageBox.Show("載入成功，請重新啟動本程式");
                            };

                        client.ImportDevicesAsync(input.InputString.Trim(),this.ProjectID);


                    }
                };
            input.Show();

        }


        public void ToClientNotifyConnectionChange(int DeviceID, bool Connected)
        {

            tblDevice device = dbservice.tblDevices.Where(n => n.ZeeBeeID == DeviceID).FirstOrDefault();
            if (device == null)
                return;

            device.IsConnected = Connected;

          //  throw new NotImplementedException();
        }
    }

       

      
      

       

        }



