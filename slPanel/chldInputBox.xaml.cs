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

namespace slPanel
{
    public partial class chldInputBox : ChildWindow
    {
        public string InputString
        {
            get
            {
                return txtInput.Text;
            }
        }
        private chldInputBox()
        {
            InitializeComponent();
        }

        public chldInputBox(string caption, string message):this()
        {
            this.Title = caption;
            this.tbMessage.Text = message;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

