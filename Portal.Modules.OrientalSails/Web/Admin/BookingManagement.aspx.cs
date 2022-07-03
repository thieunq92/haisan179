using CMS.Core.Domain;
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using Portal.Modules.OrientalSails.Enums;
using Portal.Modules.OrientalSails.Enums.RestaurantBooking;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class BookingManagement : System.Web.UI.Page
    {
        private BookingManagementBLL bookingManagementBLL;
        private PermissionBLL permissionBLL;
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
        public BookingManagementBLL BookingManagementBLL
        {
            get
            {
                if (bookingManagementBLL == null)
                    bookingManagementBLL = new BookingManagementBLL();
                return bookingManagementBLL;
            }
        }
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
        public bool AllowViewHistoryBooking
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowViewHistoryBooking);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.Title = "Quản lý đặt chỗ";
            var allowAccess = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowAccessBookingManagementPage);
            if (!allowAccess)
            {
                ShowErrors("Bạn không có quyền truy cập vào trang này");
                plhAdminContent.Visible = false;
                return;
            }
            if (!IsPostBack)
            {            
                var code = Request.QueryString["c"];
                txtCode.Text = code;
                var codeIntType = -1;
                try
                {
                    codeIntType = Int32.Parse(code.Remove(0, 2).TrimStart('0'));
                }
                catch { }
                DateTime? date = null;
                try
                {
                    date = DateTime.ParseExact(Request.QueryString["d"], "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch { }
                if (date.HasValue)
                {
                    txtDate.Text = date.Value.ToString("dd/MM/yyyy");
                }
                var agency = Request.QueryString["a"];
                txtAgency.Text = agency;
                var partOfDay = -1;
                try
                {
                    partOfDay = Int32.Parse(Request.QueryString["pod"]);
                }
                catch { }
                var agencyId = -1;
                try
                {
                    agencyId = Int32.Parse(Request.QueryString["ai"]);
                }
                catch { }
                ddlPartOfDay.SelectedValue = partOfDay.ToString();
                var queryOverRestaurantBooking = BookingManagementBLL.RestaurantBookingGetAllByCriterion(codeIntType, date, agency, partOfDay, agencyId);
                rptBooking.DataSource = queryOverRestaurantBooking
                    .OrderBy(x => x.Date).Desc
                    .Skip(pagerBookings.CurrentPageIndex * pagerBookings.PageSize).Take(pagerBookings.PageSize)
                    .Future().ToList();
                pagerBookings.AllowCustomPaging = true;
                pagerBookings.VirtualItemCount = queryOverRestaurantBooking.RowCount();
                rptBooking.DataBind();
            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (bookingManagementBLL != null)
            {
                bookingManagementBLL.Dispose();
                bookingManagementBLL = null;
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

        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            Response.Redirect("BookingManagement.aspx" + QueryStringBuildByCriterion());
        }

        public string QueryStringBuildByCriterion()
        {
            NameValueCollection nvcQueryString = new NameValueCollection();
            nvcQueryString.Add("NodeId", "1");
            nvcQueryString.Add("SectionId", "15");

            if (!string.IsNullOrEmpty(txtCode.Text))
            {
                nvcQueryString.Add("c", txtCode.Text);
            }

            if (!string.IsNullOrEmpty(txtDate.Text))
            {
                nvcQueryString.Add("d", txtDate.Text);
            }

            if (!string.IsNullOrEmpty(txtAgency.Text))
            {
                nvcQueryString.Add("a", txtAgency.Text);
            }

            if (ddlPartOfDay.SelectedValue != "-1")
            {
                nvcQueryString.Add("pod", ddlPartOfDay.SelectedValue);
            }
            var criterions = (from key in nvcQueryString.AllKeys
                              from value in nvcQueryString.GetValues(key)
                              select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value))).ToArray();

            return "?" + string.Join("&", criterions);
        }

        public string GetStatus(StatusEnum status)
        {
            var output = "";
            switch (status)
            {
                case StatusEnum.Approved:
                    output = "Xác nhận";
                    break;
                case StatusEnum.Cancel:
                    output = "Hủy";
                    break;
                case StatusEnum.Pending:
                    output = "Chờ xác nhận";
                    break;
            }
            return output;
        }
        public string GetColor(StatusEnum status, DateTime? date)
        {
            var output = "";
            switch (status)
            {
                case StatusEnum.Approved:
                    if (date.HasValue)
                    {
                        if (date.Value < DateTime.Now)
                            output = "custom-success";
                    }
                    break;
                case StatusEnum.Cancel:
                    output = "custom-danger";
                    break;
                case StatusEnum.Pending:
                    output = "custom-warning";
                    break;
            }
            return output;
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