
namespace slPanel.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // MetadataTypeAttribute 會將 tblDeviceMetadata 識別為
    // 帶有 tblDevice 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblDevice.tblDeviceMetadata))]
    public partial class tblDevice
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblDevice 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblDeviceMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblDeviceMetadata()
            {
            }

            public string ColorType { get; set; }

            public int DeviceID { get; set; }

            public string DevType { get; set; }

            public int GroupID { get; set; }

            public int ProjectID { get; set; }

            public double Rotation { get; set; }

            public int SectionID { get; set; }

            public string Shape { get; set; }

            public tblProjectGroupSection tblProjectGroupSection { get; set; }

            public int X { get; set; }

            public int Y { get; set; }

            public int ZeeBeeID { get; set; }
            public string MAC { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblProjectMetadata 識別為
    // 帶有 tblProject 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblProject.tblProjectMetadata))]
    public partial class tblProject
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblProject 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblProjectMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblProjectMetadata()
            {
            }

            public int ProjectID { get; set; }

            public string ProjectName { get; set; }
            [Include]
            public EntityCollection<tblProjectGroup> tblProjectGroup { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblProjectGroupMetadata 識別為
    // 帶有 tblProjectGroup 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblProjectGroup.tblProjectGroupMetadata))]
    public partial class tblProjectGroup
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblProjectGroup 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblProjectGroupMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblProjectGroupMetadata()
            {
            }

            public int GroupID { get; set; }

            public string GroupName { get; set; }

            public string GroupPicture { get; set; }

            public bool IsPictureDownload { get; set; }

            public int ProjectID { get; set; }

            public tblProject tblProject { get; set; }
             [Include]
            public EntityCollection<tblProjectGroupSection> tblProjectGroupSection { get; set; }

            public string UploadDesc { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblProjectGroupSectionMetadata 識別為
    // 帶有 tblProjectGroupSection 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblProjectGroupSection.tblProjectGroupSectionMetadata))]
    public partial class tblProjectGroupSection
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblProjectGroupSection 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblProjectGroupSectionMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblProjectGroupSectionMetadata()
            {
            }

            public int GroupID { get; set; }

            public int ProjectID { get; set; }

            public int SectionID { get; set; }

            public string SectionName { get; set; }
             [Include]
            public EntityCollection<tblDevice> tblDevice { get; set; }

            public tblProjectGroup tblProjectGroup { get; set; }
             [Include]
            public EntityCollection<tblSectionLedOneTimePlan> tblSectionLedOneTimePlan { get; set; }
             [Include]
            public EntityCollection<tblSectionLedPlan> tblSectionLedPlan { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblSectionLedOneTimePlanMetadata 識別為
    // 帶有 tblSectionLedOneTimePlan 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblSectionLedOneTimePlan.tblSectionLedOneTimePlanMetadata))]
    public partial class tblSectionLedOneTimePlan
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblSectionLedOneTimePlan 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblSectionLedOneTimePlanMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblSectionLedOneTimePlanMetadata()
            {
            }

            [Range(0,100)]
            [Display(Order = 6)]
            public int B { get; set; }
            [Display(Order=1,Prompt="啟始時段",Name="啟始時段",AutoGenerateField=false)]
            public DateTime BeginTime { get; set; }
            [Display(Order=2,Name="區間(分)")]
            public int DurationMin { get; set; }
            [Range(0, 100)]
            [Display(Order = 5)]
            public int G { get; set; }
            [Display(AutoGenerateField=false)]
            public int GroupID { get; set; }
            [Display(AutoGenerateField = false)]
            public int PlanID { get; set; }
            [Display(AutoGenerateField = false)]
            public int ProjectID { get; set; }
             [Range(0, 100)]
             [Display(Order = 4)]
            public int R { get; set; }
            [Display(AutoGenerateField = false)]
            public int SectionID { get; set; }
            [Display(AutoGenerateField = false)]
            public tblProjectGroupSection tblProjectGroupSection { get; set; }
             [Range(0, 100)]
             [Display(Order=3)]
            public int W { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblSectionLedPlanMetadata 識別為
    // 帶有 tblSectionLedPlan 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblSectionLedPlan.tblSectionLedPlanMetadata))]
    public partial class tblSectionLedPlan
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblSectionLedPlan 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblSectionLedPlanMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblSectionLedPlanMetadata()
            {
            }

            [Range(0,100)]
            [Display(Order = 5)]
            public int B { get; set; }

            [Display(Order=1,Prompt="啟始時段",Name="啟始時段",AutoGenerateField=false)]
           // [RegularExpression(@"^[0-9][0-9]:[0-9][0-9]$")]
            public DateTime BeginTime { get; set; }

             [Range(0, 100)]
             [Display(Order = 4)]
            public int G { get; set; }

            [Display(AutoGenerateField=false)]
            public int GroupID { get; set; }
            [Display(AutoGenerateField = false)]
            public int PlanID { get; set; }
              [Display(AutoGenerateField = false)]
            public int ProjectID { get; set; }

             [Range(0, 100)]
             [Display(Order = 3)]
            public int R { get; set; }
              [Display(AutoGenerateField = false)]
            public int SectionID { get; set; }
              [Display(AutoGenerateField = false)]
            public tblProjectGroupSection tblProjectGroupSection { get; set; }
             [Range(0, 100)]
             [Display(Order = 2)]
            public int W { get; set; }
        }
    }

    // MetadataTypeAttribute 會將 tblUserMetadata 識別為
    // 帶有 tblUser 類別其他中繼資料的類別。
    [MetadataTypeAttribute(typeof(tblUser.tblUserMetadata))]
    public partial class tblUser
    {

        // 這個類別可讓您將自訂屬性 (Attribute) 附加到 tblUser 類別
        // 的 properties。
        //
        // 例如，下列程式碼將 Xyz 屬性標記為
        // 必要的屬性，並指定有效值的格式:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class tblUserMetadata
        {

            // 中繼資料類別本就不應該具現化。
            private tblUserMetadata()
            {
            }

            public string Passwoord { get; set; }

            public string UserID { get; set; }
        }
    }
}
