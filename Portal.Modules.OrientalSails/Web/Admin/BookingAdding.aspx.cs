using CMS.Core.Domain;
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Enums;
using Portal.Modules.OrientalSails.Enums.RestaurantBooking;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class BookingAdding : System.Web.UI.Page
    {
        private BookingAddingBLL bookingAddingBLL;
        private PermissionBLL permissionBLL;
        public BookingAddingBLL BookingAddingBLL
        {
            get
            {
                if (bookingAddingBLL == null)
                    bookingAddingBLL = new BookingAddingBLL();
                return bookingAddingBLL;
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
        public bool AllowAddBooking
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowAddBooking);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.Title = "Thêm đặt chỗ";
            var allowAccess = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowAccessBookingAddingPage);
            if (!allowAccess)
            {
                ShowErrors("Bạn không có quyền truy cập vào trang này");
                plhAdminContent.Visible = false;
                return;
            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (bookingAddingBLL != null)
            {
                bookingAddingBLL.Dispose();
                bookingAddingBLL = null;
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!AllowAddBooking)
            {
                ShowErrors("Bạn không có quyền thêm đặt chỗ");
                return;
            }
            DateTime? date = null;
            try
            {
                date = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch { }
            var agencyId = -1;
            try
            {
                agencyId = Int32.Parse(agencySelector.Value);
            }
            catch { }
            var agency = BookingAddingBLL.AgencyGetById(agencyId);
            var restaurantBooking = new RestaurantBooking()
            {
                CreatedBy = CurrentUser,
                CreatedDate = DateTime.Now,
                LastEditedBy = CurrentUser,
                LastEditedDate = DateTime.Now,
                Date = date,
                Agency = agency,
                Status = StatusEnum.Approved,
                PartOfDay = 2,
                Payment = 1,
            };
            BookingAddingBLL.RestaurantBookingSaveOrUpdate(restaurantBooking);
            var activityLogging = new ActivityLogging()
            {
                CreatedBy = CurrentUser,
                CreatedTime = DateTime.Now,
                Function = "Chỉnh sửa đặt chỗ",
                Detail = "Thêm đặt chỗ",
                ObjectId = "BookingId:" + restaurantBooking.Id,
            };
            BookingAddingBLL.ActivityLoggingSaveOrUpdate(activityLogging);
            Response.Redirect("BookingViewing.aspx?NodeId=1&SectionId=15&Notify=0&bi=" + restaurantBooking.Id);
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