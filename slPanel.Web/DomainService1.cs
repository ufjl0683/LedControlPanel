
namespace slPanel.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // 使用 LedLocalEntities 內容實作應用程式邏輯。
    // TODO: 將應用程式邏輯加入至這些方法或其他方法。
    // TODO: 連接驗證 (Windows/ASP.NET Forms) 並取消下面的註解，以停用匿名存取
    // 視需要也考慮加入要限制存取的角色。
    // [RequiresAuthentication]
    [EnableClientAccess()]
    public partial class DomainService1 : LinqToEntitiesDomainService<LedLocalEntities>
    {

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblDevice' 查詢。
        public IQueryable<tblDevice> GetTblDevice()
        {
            return this.ObjectContext.tblDevice;
        }

        public void InsertTblDevice(tblDevice tblDevice)
        {
            if ((tblDevice.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblDevice, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblDevice.AddObject(tblDevice);
            }
        }

        public void UpdateTblDevice(tblDevice currenttblDevice)
        {
            this.ObjectContext.tblDevice.AttachAsModified(currenttblDevice, this.ChangeSet.GetOriginal(currenttblDevice));
        }

        public void DeleteTblDevice(tblDevice tblDevice)
        {
            if ((tblDevice.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblDevice, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblDevice.Attach(tblDevice);
                this.ObjectContext.tblDevice.DeleteObject(tblDevice);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblProject' 查詢。
        public IQueryable<tblProject> GetTblProject()
        {
            return this.ObjectContext.tblProject;
        }

        public void InsertTblProject(tblProject tblProject)
        {
            if ((tblProject.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblProject, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblProject.AddObject(tblProject);
            }
        }

        public void UpdateTblProject(tblProject currenttblProject)
        {
            this.ObjectContext.tblProject.AttachAsModified(currenttblProject, this.ChangeSet.GetOriginal(currenttblProject));
        }

        public void DeleteTblProject(tblProject tblProject)
        {
            if ((tblProject.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblProject, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblProject.Attach(tblProject);
                this.ObjectContext.tblProject.DeleteObject(tblProject);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblProjectGroup' 查詢。
        public IQueryable<tblProjectGroup> GetTblProjectGroup()
        {
            return this.ObjectContext.tblProjectGroup;
        }

        public void InsertTblProjectGroup(tblProjectGroup tblProjectGroup)
        {
            if ((tblProjectGroup.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblProjectGroup, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblProjectGroup.AddObject(tblProjectGroup);
            }
        }

        public void UpdateTblProjectGroup(tblProjectGroup currenttblProjectGroup)
        {
            this.ObjectContext.tblProjectGroup.AttachAsModified(currenttblProjectGroup, this.ChangeSet.GetOriginal(currenttblProjectGroup));
        }

        public void DeleteTblProjectGroup(tblProjectGroup tblProjectGroup)
        {
            if ((tblProjectGroup.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblProjectGroup, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblProjectGroup.Attach(tblProjectGroup);
                this.ObjectContext.tblProjectGroup.DeleteObject(tblProjectGroup);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblProjectGroupSection' 查詢。
        public IQueryable<tblProjectGroupSection> GetTblProjectGroupSection()
        {
            return this.ObjectContext.tblProjectGroupSection;
        }

        public void InsertTblProjectGroupSection(tblProjectGroupSection tblProjectGroupSection)
        {
            if ((tblProjectGroupSection.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblProjectGroupSection, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblProjectGroupSection.AddObject(tblProjectGroupSection);
            }
        }

        public void UpdateTblProjectGroupSection(tblProjectGroupSection currenttblProjectGroupSection)
        {
            this.ObjectContext.tblProjectGroupSection.AttachAsModified(currenttblProjectGroupSection, this.ChangeSet.GetOriginal(currenttblProjectGroupSection));
        }

        public void DeleteTblProjectGroupSection(tblProjectGroupSection tblProjectGroupSection)
        {
            if ((tblProjectGroupSection.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblProjectGroupSection, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblProjectGroupSection.Attach(tblProjectGroupSection);
                this.ObjectContext.tblProjectGroupSection.DeleteObject(tblProjectGroupSection);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblSectionLedOneTimePlan' 查詢。
        public IQueryable<tblSectionLedOneTimePlan> GetTblSectionLedOneTimePlan()
        {
            return this.ObjectContext.tblSectionLedOneTimePlan;
        }

        public void InsertTblSectionLedOneTimePlan(tblSectionLedOneTimePlan tblSectionLedOneTimePlan)
        {
            if ((tblSectionLedOneTimePlan.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSectionLedOneTimePlan, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblSectionLedOneTimePlan.AddObject(tblSectionLedOneTimePlan);
            }
        }

        public void UpdateTblSectionLedOneTimePlan(tblSectionLedOneTimePlan currenttblSectionLedOneTimePlan)
        {
            this.ObjectContext.tblSectionLedOneTimePlan.AttachAsModified(currenttblSectionLedOneTimePlan, this.ChangeSet.GetOriginal(currenttblSectionLedOneTimePlan));
        }

        public void DeleteTblSectionLedOneTimePlan(tblSectionLedOneTimePlan tblSectionLedOneTimePlan)
        {
            if ((tblSectionLedOneTimePlan.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSectionLedOneTimePlan, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblSectionLedOneTimePlan.Attach(tblSectionLedOneTimePlan);
                this.ObjectContext.tblSectionLedOneTimePlan.DeleteObject(tblSectionLedOneTimePlan);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblSectionLedPlan' 查詢。
        public IQueryable<tblSectionLedPlan> GetTblSectionLedPlan()
        {
            return this.ObjectContext.tblSectionLedPlan;
        }

        public void InsertTblSectionLedPlan(tblSectionLedPlan tblSectionLedPlan)
        {
            if ((tblSectionLedPlan.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSectionLedPlan, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblSectionLedPlan.AddObject(tblSectionLedPlan);
            }
        }

        public void UpdateTblSectionLedPlan(tblSectionLedPlan currenttblSectionLedPlan)
        {
            this.ObjectContext.tblSectionLedPlan.AttachAsModified(currenttblSectionLedPlan, this.ChangeSet.GetOriginal(currenttblSectionLedPlan));
        }

        public void DeleteTblSectionLedPlan(tblSectionLedPlan tblSectionLedPlan)
        {
            if ((tblSectionLedPlan.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSectionLedPlan, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblSectionLedPlan.Attach(tblSectionLedPlan);
                this.ObjectContext.tblSectionLedPlan.DeleteObject(tblSectionLedPlan);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblUser' 查詢。
        public IQueryable<tblUser> GetTblUser()
        {
            return this.ObjectContext.tblUser;
        }

        public void InsertTblUser(tblUser tblUser)
        {
            if ((tblUser.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblUser, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblUser.AddObject(tblUser);
            }
        }

        public void UpdateTblUser(tblUser currenttblUser)
        {
            this.ObjectContext.tblUser.AttachAsModified(currenttblUser, this.ChangeSet.GetOriginal(currenttblUser));
        }

        public void DeleteTblUser(tblUser tblUser)
        {
            if ((tblUser.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblUser, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblUser.Attach(tblUser);
                this.ObjectContext.tblUser.DeleteObject(tblUser);
            }
        }
    }
}


