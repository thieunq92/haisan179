using CMS.Core.Domain;
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class RestaurantBookingHistory : System.Web.UI.Page
    {
        private RestaurantBookingHistoryBLL restaurantBookingHistoryBLL;
        private PermissionBLL permissionBLL;
        private UserBLL userBLL;
        public RestaurantBookingHistoryBLL RestaurantBookingHistoryBLL
        {
            get
            {
                if (restaurantBookingHistoryBLL == null)
                    restaurantBookingHistoryBLL = new RestaurantBookingHistoryBLL();
                return restaurantBookingHistoryBLL;
            }
        }
        public RestaurantBooking RestaurantBooking
        {
            get
            {
                var restaurantBookingId = -1;
                try
                {
                    restaurantBookingId = Int32.Parse(Request.QueryString["bi"]);
                }
                catch { }
                var restaurantBooking = RestaurantBookingHistoryBLL.RestaurantBookingGetById(restaurantBookingId);
                if (restaurantBooking.Id == -1)
                {
                    Response.Redirect("404.aspx");
                }
                return restaurantBooking;
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
                    permissionBLL = new PermissionBLL();
                return permissionBLL;
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
            if (!AllowViewHistoryBooking)
            {
                ShowErrors("Bạn không có quyền xem lịch sử");
                plhAdminContent.Visible = false;
                return;
            }
            BindingHistoryData();
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (restaurantBookingHistoryBLL != null)
            {
                restaurantBookingHistoryBLL.Dispose();
                restaurantBookingHistoryBLL = null;
            }
            if(permissionBLL != null){
                permissionBLL.Dispose();
                permissionBLL = null;
            }
            if (userBLL != null)
            {
                userBLL.Dispose();
                userBLL = null;
            }
        }
        public void BindingHistoryData()
        {
            var listTrackingChangeBooking = RestaurantBookingHistoryBLL.TrackingChangeBookingGetAllByBookingId(RestaurantBooking.Id);
            rptHistoryStatus.DataSource = listTrackingChangeBooking.Where(x => x.ColumnName == "Status").OrderBy(x => x.Id);
            rptHistoryStatus.DataBind();
            rptHistoryDate.DataSource = listTrackingChangeBooking.Where(x => x.ColumnName == "Date").OrderBy(x => x.Id);
            rptHistoryDate.DataBind();
            rptHistoryNumberOfSet.DataSource = listTrackingChangeBooking.Where(x => x.ColumnName == "NumberOfSet").OrderBy(x => x.Id);
            rptHistoryNumberOfSet.DataBind();
            rptHistoryPricePerPerson.DataSource = listTrackingChangeBooking.Where(x => x.ColumnName == "CostPerPerson").OrderBy(x => x.Id);
            rptHistoryPricePerPerson.DataBind();
        }
        public string GetDate(string date)
        {
            DateTime? dateInDateTimeType = null;
            try
            {
                dateInDateTimeType = DateTime.ParseExact(date, "M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            }
            catch { }
            return dateInDateTimeType.HasValue ? dateInDateTimeType.Value.ToString("dd/MM/yyyy") : "";
        }
        public string GetStatus(string status)
        {
            switch (status)
            {
                case "Approved":
                    return "Xác nhận";
                case "Cancel":
                    return "Hủy";
                case "Pending":
                    return "Chờ xác nhận";
                default:
                    return "";
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