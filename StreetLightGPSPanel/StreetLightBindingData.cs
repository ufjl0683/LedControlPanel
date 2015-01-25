using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StreetLightPanel
{


    public class StreetLightBindingDataGroup : INotifyPropertyChanged
    {
        int _DimLevel = 0;
        public CeraDevices.CoordinatorDevice coor
        {
            set;
            get;
        }
        public int DimLevel
        {

            get { return _DimLevel; }
            set
            {
                if (value != _DimLevel)
                {
                    _DimLevel = value;
                    if (this.PropertyChanged != null)
                        this.PropertyChanged(this, new PropertyChangedEventArgs("DimLevel"));

                }
            }

        }
        public StreetLightBindingData[] BindingDatas { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [DataContract]
    public class                         StreetLightBindingData:INotifyPropertyChanged

    {
        bool _IsEnable;
        int _DimLevel = 0;
        bool _IsChecked;
        string _originalDevID="";
       // string _DevID;
        [DataMember]
        public string DevID
        {
            get;
            set;
            //get
            //{
            //    if (ReplaceDevID == "")
            //        return _DevID;
            //    else
            //        return ReplaceDevID;

            //}

            //set
            //{
            //    _DevID = value;
            //}
        }
        [DataMember]
        public string OriginalDevID
        {
            get { return _originalDevID; }
            set { this._originalDevID = value; }
        }

        public int DimLevel {

            get { return _DimLevel; }
            set
            {
                if (value != _DimLevel)
                {
                    _DimLevel = value;
                    if(this.PropertyChanged!=null)
                        this.PropertyChanged(this,new PropertyChangedEventArgs("DimLevel"));

                }
            }


        }
        [DataMember]
        public string LightNo
        {
            get;
            set;
        }
        
        public bool IsEnable {  
        
         get {
            return _IsEnable;
        }


            set
            {
                if (value != _IsEnable)
                {
                    _IsEnable = value;
                     if( this.PropertyChanged!=null)
                         this.PropertyChanged(this,new PropertyChangedEventArgs("IsEnable"));
                }
            }
        }
        public double W
        {
            get;
            set;
        }
        public override string ToString()
        {
            return DevID;
        }
        public int Status
        {
            get
            {
                if (IsFake)
                    return -1;
                else if (IsEnable)
                    return 1;
                else
                    return 0;
            }
        }
        public bool boolMark {
            get;set;
            }

        public bool IsFake
        {
            get;
            set;
        }
        public bool IsChecked {
            get
            {
                return _IsChecked;
            }


            set
            {
                if (value != _IsChecked)
                {
                    _IsChecked = value;
                    if (this.PropertyChanged != null)
                        this.PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
