using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using UIDevices;

namespace slPanel.Web
{

    public partial class tblProjectGroupSection
    {
        public string PicUrl
        {
            get
            {
                return this.tblProjectGroup.PicUrl;
            }
        }
    }

   
    public partial class tblDevice
    {

        int _W,_R,_G,_B;

        private bool _IsConnected = true;
        public bool IsConnected
        {
            get
            {
                return _IsConnected;
            }
            set
            {
                if (_IsConnected != value)
                {
                    _IsConnected = value;
                    this.RaisePropertyChanged("IsConnected");
                }
            }
        }
        public int W
        {
            get {
              
                  return _W;
               }
            set
            {
                if (value != _W)
                {
                    _W = value;
                    RaisePropertyChanged("DisplayColor");
                    RaisePropertyChanged("W");
                }
            }
        }
        public int R
        {
            get { return _R; }
            set
            {
                if (value != _R)
                {
                    _R = value;
                    RaisePropertyChanged("DisplayColor");
                    RaisePropertyChanged("R");
                }
            }
        }
        public int G
        {
            get { return _G; }
            set
            {
                if (value != _G)
                {
                    _G = value;
                    RaisePropertyChanged("DisplayColor");
                    RaisePropertyChanged("G");
                }
            }
        }
        public int B
        {
            get { return _B; }
            set
            {
                if (value != _B)
                {
                    _B = value;
                    RaisePropertyChanged("DisplayColor");
                    RaisePropertyChanged("B");
                }
            }
        }
        byte ReMapColor(int color)
        {
            if (color == 0)
                return 0;
            if (color >= 100) return 255;
            byte outcolor=(byte)(color  * 255/100+100);
            if(outcolor>255) outcolor=255;
            return outcolor;
        }
        public Color DisplayColor
        {
            get
            {

                byte ww, rr, gg, bb;
                ww = ReMapColor(W);
                rr = ReMapColor(R);
                gg = ReMapColor(G);
                bb = ReMapColor(B);
                if (this.ColorType == "MONO")
                {

                    return new Color() { A = 255, R=ww, G =ww, B = 0 } ; //Yellow Display
                }
                //else if (this.DevType == "COLOR")
                //{
                    return new Color() { A = 255, R =rr, G =gg, B =bb };
                //}
                //else
                //{
                //    return new SolidColorBrush(new Color() { A = 255, R = W, G = G, B = B });
                //}

            }


        }
      public  string PicUrl
        {
            get
            {
               
                return this.tblProjectGroupSection.PicUrl;
            }
        }

      public bool IsChecked
      {
          get;
          set;
      }

      public IUIDevice UIDevice { get; set; }

      partial void OnZeeBeeIDChanged()
      {
          //throw new NotImplementedException();
     
      }


    }
    public partial class tblProjectGroup
    {
        partial void OnIsPictureDownload()
        {
           
           RaisePropertyChanged("UploadDesc");
           RaisePropertyChanged("PicUrl");
        }

        partial void OnGroupPictureChanged()
        {
           // throw new NotImplementedException();
            RaisePropertyChanged("PicUrl");
        }

        private string _PicUrl;
        public string PicUrl
        {
            get
            {
                return new Uri(Application.Current.Host.Source, "../Images/"+this.ProjectID+"/" + this.GroupPicture).ToString();
            }
            set
            {
                _PicUrl = value;
                RaisePropertyChanged("PicUrl");
                //this.PropertyChanged("PicUrl");
            }
        }
     
    }
}
