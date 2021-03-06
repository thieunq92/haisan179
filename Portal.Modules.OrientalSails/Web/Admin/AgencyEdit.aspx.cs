using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.ServerControls.FileUpload;
using CMS.Web.Admin.Controls;
using CMS.Web.Util;
using NHibernate.Criterion;
using log4net;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using Portal.Modules.OrientalSails.Enums;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class AgencyEdit : SailsAdminBase
    {
        private AgencyEditBLL agencyEditBLL;
        private UserBLL userBLL;
        private PermissionBLL permissionBLL;
        private DateTime _current;
        private Agency agency;
        public AgencyEditBLL AgencyEditBLL
        {
            get
            {
                if (agencyEditBLL == null)
                {
                    agencyEditBLL = new AgencyEditBLL();
                }
                return agencyEditBLL;
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
        public Agency Agency
        {
            get
            {
                if (agency == null)
                {
                    agency = new Agency();
                    StatusPage = "Add";
                }
                var agencyId = Request.QueryString["AgencyId"];
                try
                {
                    agency = AgencyEditBLL.AgencyGetById(Int32.Parse(agencyId));
                    StatusPage = "Edit";
                }
                catch { }
                return agency;
            }
        }
        public string StatusPage { get; set; }
        public bool AllowAddAgency
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowAddAgency);
            }
        }
        public bool AllowEditAgency
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowEditAgency);
            }
        }
        public bool AllowChangeSalesInCharge
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowChangeSalesIncharge);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            bool allowAccess = false;
            if (Agency.Id == 0)
            {
                this.Master.Title = "Thêm đối tác";
                allowAccess = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowAccessAddAgencyPage);
            }
            else
            {
                this.Master.Title = "Sửa đối tác";
                allowAccess = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowAccessEditAgencyPage);
            }
            if (!allowAccess)
            {
                ShowErrors("Bạn không có quyền truy cập vào trang này");
                plhAdminContent.Visible = false;
                return;
            }
            if (!IsPostBack)
            {
                Role role = AgencyEditBLL.RoleGetById(21);
                ddlSales.DataSource = role.Users;
                ddlSales.DataTextField = "FullName";
                ddlSales.DataValueField = "Id";
                ddlSales.DataBind();
                LoadInfo();
                if (Request.QueryString["agencyid"] != null)
                {
                    if (Agency.Sale != UserIdentity && !Module.PermissionCheck(Permission.EDIT_SALE_IN_CHARGE, UserIdentity))
                    {
                        ddlSales.Enabled = false;
                        txtSaleStart.Enabled = false;
                    }
                }
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (agencyEditBLL != null)
            {
                agencyEditBLL.Dispose();
                agencyEditBLL = null;
            }
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

        protected void buttonSave_Click(object sender, EventArgs e)
        {
            if (Agency.Id == 0)
            {
                if (!AllowAddAgency)
                {
                    ShowErrors("Bạn không có quyền thêm đối tác");
                    return;
                }
            }
            else
            {
                if (!AllowEditAgency)
                {
                    ShowErrors("Bạn không có quyền sửa đối tác");
                    return;
                }
            }
            if (Agency.Id == 0)
            {
                Agency.CreatedBy = UserIdentity;
                Agency.CreatedDate = DateTime.Now;
            }
            Agency.Name = textBoxName.Text;
            Agency.Phone = txtPhone.Text;
            Agency.Address = txtAddress.Text;
            Agency.TradingName = txtTradingName.Text;
            Agency.Email = txtEmail.Text;
            Agency.ModifiedBy = UserIdentity;
            Agency.ModifiedDate = DateTime.Now;
            Agency.IsVat = chkVat.Checked;
            Agency.Invoice = txtInvoice.Text;
            User oldsale = Agency.Sale;
            DateTime? oldStart = Agency.SaleStart;
            if (!string.IsNullOrEmpty(txtSaleStart.Text))
            {
                Agency.SaleStart = DateTime.ParseExact(txtSaleStart.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                Agency.SaleStart = null;
            }
            Agency.Sale = Module.UserGetById(Convert.ToInt32(ddlSales.SelectedValue));
            if (!string.IsNullOrEmpty(ddlPaymentType.SelectedValue)) Agency.PaymentType = Convert.ToInt32(ddlPaymentType.SelectedValue);
            AgencyEditBLL.AgencySaveOrUpdate(Agency);
            var activityLogging = new ActivityLogging()
            {
                CreatedTime = DateTime.Now,
                CreatedBy = UserIdentity,
                Function = "Chỉnh sửa đối tác",
                ObjectId = "AgencyId:" + Agency.Id,
            };
            if (Agency.Id == 0)
            {
                activityLogging.Detail = "Sửa đối tác";
                AgencyEditBLL.ActivityLoggingSaveOrUpdate(activityLogging);
            }
            else
            {
                activityLogging.Detail = "Thêm đối tác";
                AgencyEditBLL.ActivityLoggingSaveOrUpdate(activityLogging);
            }
            // Nếu chưa có lịch sử thì lưu lại
            if (Agency.History.Count == 0 && oldsale != null)
            {
                AgencyHistory history = new AgencyHistory();
                history.Agency = Agency;
                history.Sale = oldsale;
                if (oldStart.HasValue)
                {
                    history.ApplyFrom = oldStart.Value;
                }
                else
                {
                    history.ApplyFrom = new DateTime(2000, 1, 1);
                }
                AgencyEditBLL.AgencyHistorySaveOrUpdate(history);
            }

            // Khi có sự thay đổi về sale và ngày áp dụng thì lưu lại lịch sử
            // Nhưng nếu đã có ngày áp dụng này rồi thì lưu lại theo sale mới
            if ((oldsale != Agency.Sale || oldStart != Agency.SaleStart) && Agency.SaleStart != null)
            {
                AgencyHistory history = null;
                foreach (AgencyHistory oldhis in Agency.History)
                {
                    if (oldhis.ApplyFrom == Agency.SaleStart.Value)
                    {
                        history = oldhis;
                        break;
                    }
                }
                if (history == null)
                    history = new AgencyHistory();
                history.Agency = Agency;
                history.Sale = Agency.Sale;
                history.ApplyFrom = Agency.SaleStart.Value;
                if (!AllowChangeSalesInCharge)
                {
                    ShowErrors("Bạn không có quyền thay đổi sales phụ trách. Sales in charge không được lưu");
                }
                else
                {
                    AgencyEditBLL.AgencyHistorySaveOrUpdate(history);
                }
            }
            if (StatusPage == "Add")
            {
                ShowSuccess("Thêm mới đối tác thành công");
                Session["Redirect"] = true;
                Response.Redirect("AgencyList.aspx?NodeId=1&SectionId=15");
            }
            ShowSuccess("Cập nhật thông tin đối tác thành công");
        }

        public void LoadInfo()
        {
            textBoxName.Text = Agency.Name;
            txtPhone.Text = Agency.Phone;
            txtAddress.Text = Agency.Address;
            txtEmail.Text = Agency.Email;
            txtTradingName.Text = Agency.TradingName;
            chkVat.Checked = Agency.IsVat;
            txtInvoice.Text = Agency.Invoice;
            ddlPaymentType.SelectedValue = Agency.PaymentType.ToString();
            if (Agency.CreatedBy != null && Agency.CreatedDate.HasValue)
            {
                litCreated.Text = string.Format("Tạo bởi {0} lúc {1:dd/MM/yyyy HH:MM}", Agency.CreatedBy.FullName, Agency.CreatedDate.Value);
            }
            if (Agency.ModifiedBy != null && Agency.ModifiedDate.HasValue)
            {
                litModified.Text = string.Format(" và sửa bởi {0} lúc {1:dd/MM/yyyy HH:MM}", Agency.ModifiedBy.FullName, Agency.ModifiedDate.Value);
            }
            if (Agency.SaleStart.HasValue)
            {
                txtSaleStart.Text = Agency.SaleStart.Value.ToString("dd/MM/yyyy");
            }
            if (ddlSales.Items.Count > 0)
            {
                if (Agency.Sale != null)
                {
                    ddlSales.SelectedValue = Agency.Sale.Id.ToString();
                }
                else
                {
                    ddlSales.SelectedIndex = 0;
                }
            }
            foreach (AgencyHistory history in Agency.History)
            {
                if (history.ApplyFrom > _current && history.ApplyFrom < DateTime.Today)
                {
                    _current = history.ApplyFrom;
                }
            }
            rptHistory.DataSource = Agency.History;
            rptHistory.DataBind();
        }

        protected void rptHistory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is AgencyHistory)
            {
                AgencyHistory history = (AgencyHistory)e.Item.DataItem;

                Literal litSale = e.Item.FindControl("litSale") as Literal;
                if (litSale != null)
                {
                    if (history.Sale != null)
                    {
                        litSale.Text = history.Sale.FullName;
                    }
                    else
                    {
                        litSale.Text = "Unbound Sales";
                    }
                }

                Literal litSaleStart = e.Item.FindControl("litSaleStart") as Literal;
                if (litSaleStart != null)
                {
                    litSaleStart.Text = history.ApplyFrom.ToString("dd/MM/yyyy");
                }

                if (history.ApplyFrom == _current)
                {
                    HtmlTableRow trLine = e.Item.FindControl("trLine") as HtmlTableRow;
                    if (trLine != null)
                    {
                        trLine.Attributes.Add("style", "font-weight: bold; color: red;");
                    }
                }
            }
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
