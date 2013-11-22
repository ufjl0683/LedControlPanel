using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
//using Windows.Foundation;
//using Windows.Foundation.Collections;
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Controls;
//using Windows.UI.Xaml.Controls.Primitives;
//using Windows.UI.Xaml.Data;
//using Windows.UI.Xaml.Input;
//using Windows.UI.Xaml.Media;
//using Windows.UI.Xaml.Navigation;

// 使用者控制項項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234236

namespace shschool
{
    public sealed partial class LedButton : UserControl
    {
        public event EventHandler Tapped;

        public LedButton()
        {
            this.InitializeComponent();
           // this.IsChecked = false;
           //this.Text=(string)GetValue(LedButton.TextProperty);
           // //this.Foreground = (Brush)GetValue(ForegroundProperty);
           // SetValue(LedButton.TextProperty, this.Text);
           // SetValue(LedButton.ForegroundProperty, this.Foreground);
        }

        public static readonly DependencyProperty TextProperty =
     DependencyProperty.Register(
        "Text", typeof(string),
        typeof(LedButton), new FrameworkPropertyMetadata() { Inherits=true }
    );
        public string Text //the property wrapper
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value);
            TextBlock.Text = value;
            }
        }
       

             public new  static readonly DependencyProperty ForegroundProperty =
     DependencyProperty.Register(
        "Foreground", typeof(Brush),
        typeof(LedButton), new FrameworkPropertyMetadata() { Inherits = true, DefaultValue=new SolidColorBrush(Colors.White) }
    );

           public new  Brush Foreground //the property wrapper
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value);
            TextBlock.Foreground = value;
            }
        }

           public new static readonly DependencyProperty IsCheckedProperty =
DependencyProperty.Register(
 "IsChecked", typeof(bool),
 typeof(LedButton), new FrameworkPropertyMetadata() { Inherits = true, DefaultValue = false }
);

        ////public bool InSelectioMode
        ////{
        ////    get
        ////    {
                
        ////        return this.checkBox.Visibility == Windows.UI.Xaml.Visibility.Visible ? true : false;
        ////    }
        ////    set
        ////    {
        ////        this.checkBox.Visibility = value ? Windows.UI.Xaml.Visibility.Visible : Windows.UI.Xaml.Visibility.Collapsed;
        ////    }
        ////}
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }

            set { this.checkBox.IsChecked = value;
            SetValue(IsCheckedProperty, value);
           // if (this.checkBox.IsChecked??false)
             
            
            }

        }

        bool JustUnchecked = false;
        private void Grid_Tapped(object sender, EventArgs e)
        {
            if (!this.IsEnabled)
                return;
            if (JustUnchecked)
            {
                JustUnchecked = false;
                return;
            }

         //  
           if (!(sender as LedButton).IsChecked)
           {
               //imgoff.Visibility = Windows.UI.Xaml.Visibility.Visible;
               //this.checkBox.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
               imgoff.Visibility = System.Windows.Visibility.Collapsed;
               this.checkBox.Visibility = System.Windows.Visibility.Visible;
               //(sender as LedButton).IsChecked = true;
               this.checkBox.IsChecked = true;
           }
           //else
           //{
           //    imgoff.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
           //    this.checkBox.Visibility = Windows.UI.Xaml.Visibility.Visible;
              
           //}

        //   (sender as LedButton).IsChecked = !(sender as LedButton).IsChecked;
            if (this.Tapped != null)
                this.Tapped(this, e);
        }

       

        private void userControl_Holding(object sender, EventArgs e)
        {
            

        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            imgoff.Visibility = System.Windows.Visibility.Collapsed;
            this.checkBox.Visibility = System.Windows.Visibility.Visible;
            SetValue(IsCheckedProperty, true);
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            imgoff.Visibility = System.Windows.Visibility.Visible;
            this.checkBox.Visibility =System.Windows.Visibility.Collapsed;
          //  JustUnchecked = true; ;
            SetValue(IsCheckedProperty, false);
            //this.checkBox.IsChecked = false;
        }

        private void userControl_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == false)
            {
                //imgoff.Visibility = Windows.UI.Xaml.Visibility.Visible;
                //this.checkBox.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                this.checkBox.IsChecked = false;
                SetValue(IsCheckedProperty, false);
//JustUnchecked = true; ;
            }
        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!this.IsEnabled)
                return;
            //if (JustUnchecked)
            //{
            //    JustUnchecked = false;
            //    return;
            //}

            //  
            if (!(sender as LedButton).IsChecked)
            {
                //imgoff.Visibility = Windows.UI.Xaml.Visibility.Visible;
                //this.checkBox.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                ////imgoff.Visibility = System.Windows.Visibility.Collapsed;
                ////this.checkBox.Visibility = System.Windows.Visibility.Visible;
                // (sender as LedButton).IsChecked = true;
                this.checkBox.IsChecked = true;
                SetValue(IsCheckedProperty, true);
            }
            else
            {
                this.checkBox.IsChecked = false;
                SetValue(IsCheckedProperty, false);
            }
            //else
            //{
            //    imgoff.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            //    this.checkBox.Visibility = Windows.UI.Xaml.Visibility.Visible;

            //}

            //   (sender as LedButton).IsChecked = !(sender as LedButton).IsChecked;
            if (this.Tapped != null)
                this.Tapped(this, e);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }



     
    }
}
