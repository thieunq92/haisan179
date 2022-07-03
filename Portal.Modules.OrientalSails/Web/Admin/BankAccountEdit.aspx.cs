using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Enums;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class BankAccountEdit : SailsAdminBasePage
    {
        private BankAccount _bankAccount;
        private PermissionBLL permissionBLL;
        private UserBLL userBLL;
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
                    permissionBLL = new PermissionBLL();
                return permissionBLL;
            }
        }
        public bool AllowAddBankAccount
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowAddBankAccount);
            }
        }
        public bool AllowEditBankAccount
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowEditBankAccount);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = Request["id"];
            if (!string.IsNullOrEmpty(id))
            {
                _bankAccount = Module.GetById<BankAccount>(Convert.ToInt32(id));
            }
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    LoadInfo();
                }
            }
        }

        protected void Page_Unload(object sender, EventArgs e) {
            if (permissionBLL != null)
            {
                permissionBLL.Dispose();
                permissionBLL = null;
            }
            if (userBLL != null)
            {
                userBLL.Dispose();
                userBLL = null;
            }
        }
        private void LoadInfo()
        {
            txtAccName.Text = _bankAccount.AccName;
            txtAccNo.Text = _bankAccount.AccNo;
            txtAccDetail.Text = _bankAccount.AccDetail;
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            if (_bankAccount == null)
            {
                if (!AllowAddBankAccount)
                {
                    ShowErrors("Bạn không có quyền thêm tài khoản ngân hàng");
                    return;
                }
                _bankAccount = new BankAccount();
            }
            else
            {
                if (!AllowEditBankAccount)
                {
                    ShowErrors("Bạn không có quyền sửa tài khoản ngân hàng");
                    return;
                }
            }

            _bankAccount.AccName = txtAccName.Text.Trim();
            _bankAccount.AccNo = txtAccNo.Text.Trim();
            _bankAccount.AccDetail = txtAccDetail.Text.Trim();
            Module.SaveOrUpdate(_bankAccount);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ReloadReceiablesPage", "parent.location.href=parent.location.href", true);
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