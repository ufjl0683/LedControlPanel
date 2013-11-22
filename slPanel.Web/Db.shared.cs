using System;
using System.Collections.Generic;
using System.Linq;
#if !SILVERLIGHT
using System.Web;
#endif

namespace slPanel.Web
{
#if !SILVERLIGHT
    public partial class DomainService1
    {

        public IQueryable<tblProject> GetTblProjectWithInclude()
        {
            //   return this.ObjectContext.tblProject.Include("tblProjectGroup").Include("tblProjectGroup.tblProjectGroupSection").Include("tblProjectGroup.tblProjectGroupSection.tblDevice");//.Include("tblProjectGroup.tblProjectGroupSection.tblSectionLedPlane").Include("tblProjectGroup.tblProjectGroupSection.tblSectionLedOneTimePlane");
            return this.ObjectContext.tblProject.Include("tblProjectGroup.tblProjectGroupSection.tblDevice").Include("tblProjectGroup.tblProjectGroupSection.tblSectionLedPlan").Include("tblProjectGroup.tblProjectGroupSection.tblSectionLedOneTimePlan");


        }
    }
#endif

   

    public partial class tblProjectGroup
    {
        private bool _IsPictureDownload;
        partial void OnIsPictureDownload();
        public bool IsPictureDownload
        {
            get
            {
                return _IsPictureDownload;
            }
            set
            {
                _IsPictureDownload = value;
                OnIsPictureDownload();
            }
        }


        public string UploadDesc
        {

            get
            {
                return (!IsPictureDownload) ? "尚未上傳圖片" : "已上傳圖片" + this.GroupPicture;
            }
        }
    }
}
