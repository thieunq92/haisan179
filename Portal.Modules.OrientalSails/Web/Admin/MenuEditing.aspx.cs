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

namespace Portal.Modules.OrientalSails.Web
{
    public partial class MenuEditing : System.Web.UI.Page
    {
        private MenuEditingBLL menuEditingBLL;
        private UserBLL userBLL;
        private PermissionBLL permissionBLL;
        public MenuEditingBLL MenuEditingBLL
        {
            get
            {
                if (menuEditingBLL == null)
                {
                    menuEditingBLL = new MenuEditingBLL();
                }
                return menuEditingBLL;
            }
        }
        public Portal.Modules.OrientalSails.Domain.Menu Menu
        {
            get
            {
                var menuId = -1;
                try
                {
                    menuId = Int32.Parse(Request.QueryString["mi"]);
                }
                catch { }
                var menu = MenuEditingBLL.MenuGetById(menuId);
                if (menu.Id == -1)
                {
                    Response.Redirect("404.aspx");
                }
                return menu;
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
        public bool AllowEditMenu
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowEditMenu);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.Title = "Sửa thực đơn";
            var allowAccess = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowAccessReportAccountPaymentPage);
            if (!allowAccess)
            {
                ShowErrors("Bạn không có quyền truy cập vào trang này");
                plhAdminContent.Visible = false;
                return;
            }
            if (!Page.IsPostBack)
            {
                txtName.Text = Menu.Name;
                txtDetails.Text = Menu.Details;
                txtCostOfAdult.Text = Menu.CostOfAdult.ToString("#,##0.##");
                txtCostOfChild.Text = Menu.CostOfChild.ToString("#,##0.##");
                txtCostOfBaby.Text = Menu.CostOfBaby.ToString("#,##0.##");

            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (menuEditingBLL != null)
            {
                menuEditingBLL.Dispose();
                menuEditingBLL = null;
            }
            if (userBLL != null)
            {
                userBLL.Dispose();
                userBLL = null;
            }
            if (permissionBLL != null)
            {
                permissionBLL.Dispose();
                permissionBLL = null;
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (!AllowEditMenu)
            {
                ShowErrors("Bạn không có quyền sửa thực đơn");
                return;
            }
            Menu.Name = txtName.Text;
            var costOfAdult = 0.0;
            try
            {
                costOfAdult = Double.Parse(txtCostOfAdult.Text);
            }
            catch { }
            Menu.CostOfAdult = costOfAdult;
            var costOfChild = 0.0;
            try
            {
                costOfChild = Double.Parse(txtCostOfChild.Text);
            }
            catch { }
            Menu.CostOfChild = costOfChild;
            var costOfBaby = 0.0;
            try
            {
                costOfBaby = Double.Parse(txtCostOfBaby.Text);
            }
            catch { }
            Menu.CostOfBaby = costOfBaby;
            Menu.Details = txtDetails.Text;
            MenuEditingBLL.MenuSaveOrUpdate(Menu);
            ShowSuccess("Cập nhật thực đơn thành công");
            var activityLogging = new ActivityLogging()
            {
                CreatedTime = DateTime.Now,
                CreatedBy = CurrentUser,
                Function = "Chỉnh sửa thực đơn",
                Detail = "Sửa thực đơn",
                ObjectId = "MenuId:" + Menu.Id,
            };
            MenuEditingBLL.ActivityLoggingSaveOrUpdate(activityLogging);
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