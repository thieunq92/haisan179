using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Web.UI;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class Permissions : SailsAdminBasePage
    {
        private string _currentGroup = string.Empty;
        private Role _role;
        private User _user;

        private IList _permissions;
        private IList _fixedPermission;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UserIdentity.HasPermission(AccessLevel.Administrator))
            {
                ShowError("You must be administrator to use this function");
            }

            if (Request.QueryString["roleid"]!=null)
            {
                _role = Module.RoleGetById(Convert.ToInt32(Request.QueryString["roleid"]));
                litTitle.Text = string.Format("Quyền cho vai trò {0}", _role.Name);
            }
            else if (Request.QueryString["userid"]!=null)
            {
                _user = Module.UserGetById(Convert.ToInt32(Request.QueryString["userid"]));
                litTitle.Text = string.Format("Quyền cho người dùng <i>{0}</i>", _user.UserName);
            }
            else
            {
                ShowError("Bad request");
                return;
            }

            if (_role != null)
            {
                _permissions = Module.PermissionsGetByRole(_role);
            }
            else
            {
                _permissions = Module.PermissionsGetByUser(_user);
                _fixedPermission = Module.PermissionsGetByUserRole(_user);
            }

            if (!IsPostBack)
            {
                rptPermissions.DataSource = Module.Permissions;
                rptPermissions.DataBind();
            }
        }

        protected void rptPermissions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is ModulePermission)
            {
                ModulePermission permission = (ModulePermission) e.Item.DataItem;
                CheckBox chkPermission = (CheckBox) e.Item.FindControl("chkPermission");
                chkPermission.Text = permission.FriendlyName;
                if (_permissions.Contains(permission.Name))
                {
                    chkPermission.Checked = true;
                }
                else
                {
                    chkPermission.Checked = false;
                }

                if (_fixedPermission != null)
                {
                    if (_fixedPermission.Contains(permission.Name))
                    {
                        chkPermission.Checked = true;
                        chkPermission.Enabled = false;
                    }
                }

                if (!string.IsNullOrEmpty(permission.GroupName) && permission.GroupName!=_currentGroup)
                {
                    _currentGroup = permission.GroupName;
                    HtmlGenericControl divClear = e.Item.FindControl("divClear") as HtmlGenericControl;
                    if (divClear!=null)
                    {
                        divClear.Visible = true;
                        divClear.InnerHtml = string.Format("<strong>{0}</strong>", _currentGroup);
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rptPermissions.Items)
            {
                CheckBox chkPermission = (CheckBox)item.FindControl("chkPermission");
                HiddenField hiddenName = (HiddenField) item.FindControl("hiddenName");
                if (_role!=null)
                {
                    if (_permissions.Contains(hiddenName.Value) && !chkPermission.Checked) // Nếu có quyền và không có check
                    {
                        SpecialPermission permission = Module.PermissionGetByRole(hiddenName.Value, _role);
                        if (permission!=null)
                        {
                            Module.Delete(permission);
                        }
                    }
                    else if(!_permissions.Contains(hiddenName.Value) && chkPermission.Checked)
                    {
                        SpecialPermission permission = Module.PermissionGetByRole(hiddenName.Value, _role);
                        if (permission==null)
                        {
                            permission = new SpecialPermission();
                            permission.Name = hiddenName.Value;
                            permission.Role = _role;
                            permission.ModuleType = Section.ModuleType;
                            Module.SaveOrUpdate(permission);
                        }
                    }
                }
                else
                {
                    if (chkPermission.Enabled)// Phải enable, tức là quyền theo user chứ không phải theo role
                    {
                        if (_permissions.Contains(hiddenName.Value) && !chkPermission.Checked)
                            // Nếu có quyền và không có check
                        {
                            SpecialPermission permission = Module.PermissionGetByUser(hiddenName.Value, _user);
                            if (permission != null)
                            {
                                Module.Delete(permission);
                            }
                        }
                        else if (!_permissions.Contains(hiddenName.Value) && chkPermission.Checked)
                        {
                            SpecialPermission permission = Module.PermissionGetByUser(hiddenName.Value, _user);
                            if (permission == null)
                            {
                                permission = new SpecialPermission();
                                permission.Name = hiddenName.Value;
                                permission.User = _user;
                                permission.ModuleType = Section.ModuleType;
                                Module.SaveOrUpdate(permission);
                            }
                        }
                    }
                }               
            }
            ShowSuccess("Cập nhật phân quyền thành công");
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
