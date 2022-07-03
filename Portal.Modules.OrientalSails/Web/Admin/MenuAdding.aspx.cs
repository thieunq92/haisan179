using CMS.Core.Domain;
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Domain = Portal.Modules.OrientalSails.Domain;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class MenuAdding : System.Web.UI.Page
    {
        private MenuAddingBLL menuAddingBLL;
        private UserBLL userBLL;
        private PermissionBLL permissionBLL;
        public MenuAddingBLL MenuAddingBLL
        {
            get
            {
                if (menuAddingBLL == null)
                    menuAddingBLL = new MenuAddingBLL();
                return menuAddingBLL;
            }
        }
        public PermissionBLL PermissionBLL
        {
            get
            {
                if (permissionBLL == null)
                {
                    permissionBLL = new PermissionBLL();
                }
                return permissionBLL;
            }
        }
        public UserBLL UserBLL
        {
            get
            {
                if (userBLL == null)
                    userBLL = new UserBLL();
                return userBLL;
            }
        }
        public User CurrentUser
        {
            get
            {
                return UserBLL.UserGetCurrent();
            }
        }
        public bool AllowAddMenu
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowAddMenu);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.Title = "Thêm thực đơn";
            var allowAccess = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowAccessMenuAddingPage);
            if (!allowAccess)
            {
                ShowErrors("Bạn không có quyền truy cập vào trang này");
                plhAdminContent.Visible = false;
                return;
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (menuAddingBLL != null)
            {
                menuAddingBLL.Dispose();
                menuAddingBLL = null;
            }
            if (permissionBLL != null)
            {
                permissionBLL.Dispose();
                permissionBLL = null;
            }
            if (userBLL != null) {
                userBLL.Dispose();
                userBLL = null;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!AllowAddMenu) {
                ShowErrors("Bạn không có quyền thêm thực đơn");
                return;
            }
            var costOfAdult = 0.0;
            try
            {
                costOfAdult = Double.Parse(txtCostOfAdult.Text);
            }
            catch { }
            var costOfChild = 0.0;
            try
            {
                costOfChild = Double.Parse(txtCostOfChild.Text);
            }
            catch { }
            var costOfBaby = 0.0;
            try
            {
                costOfBaby = Double.Parse(txtCostOfBaby.Text);
            }
            catch { }
            var menu = new Domain.Menu()
            {
                Name = txtName.Text,
                CostOfAdult = costOfAdult,
                CostOfChild = costOfChild,
                CostOfBaby = costOfBaby,
                Details = txtDetails.Text,
            };
            MenuAddingBLL.MenuSaveOrUpdate(menu);
            var activityLogging = new ActivityLogging()
            {
                CreatedTime = DateTime.Now,
                CreatedBy = CurrentUser,
                Function = "Chỉnh sửa thực đơn",
                Detail = "Thêm thực đơn",
                ObjectId = "MenuId:" + menu.Id
            };
            MenuAddingBLL.ActivityLoggingSaveOrUpdate(activityLogging);
            Response.Redirect("MenuManagement.aspx?NodeId=1&SectionId=15");
        }
        public void ShowWarning(string warning)
        {
            Session["WarningMessage"] = "<strong>Warning!</strong> " + warning + "<br/>" + Session["WarningMessage"];
        }

        public void ShowErrors(string error)
        {
            Session["ErrorMessage"] = "<strong>Error!</strong> " + error + "<br/>" + Session["ErrorMessage"];
        }

        public void ShowSuccess(string success)
        {
            Session["SuccessMessage"] = "<strong>Success!</strong> " + success + "<br/>" + Session["SuccessMessage"];
        }
    }
}