using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Enums;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class PrintMenu : System.Web.UI.Page
    {
        public RestaurantBookingByDateBLL restaurantBookingByDateBLL;
        private PermissionBLL permissionBLL;
        private UserBLL userBLL;
        public PermissionBLL PermissionBLL
        {
            get
            {
                if (permissionBLL == null)
                    permissionBLL = new PermissionBLL();
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
        public RestaurantBookingByDateBLL RestaurantBookingByDateBLL
        {
            get
            {
                if (restaurantBookingByDateBLL == null)
                    restaurantBookingByDateBLL = new RestaurantBookingByDateBLL();
                return restaurantBookingByDateBLL;
            }
        }
        public bool AllowExportMenu
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowExportMenu);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AllowExportMenu)
            {
                plhAdminContent.Visible = false;
                plhErrorMessage.Visible = true;
                return;
            }
            if (!Page.IsPostBack)
            {
                var bookingId = Request["Id"];
                if (!string.IsNullOrEmpty(bookingId))
                {
                    var booking =
                        RestaurantBookingByDateBLL.RestaurantBookingGetById(Convert.ToInt32(bookingId));
                    if (booking != null&& !string.IsNullOrEmpty(booking.MenuDetail))
                    {
                        litMenu1.Text = litMenu2.Text = litMenu3.Text = litMenu4.Text = booking.MenuDetail.Replace("\n", "<br/>");
                    }
                }
            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (restaurantBookingByDateBLL != null)
            {
                restaurantBookingByDateBLL.Dispose();
                restaurantBookingByDateBLL = null;
            }
            if (userBLL != null)
            {
                userBLL.Dispose();
                userBLL = null;
            }
            if (permissionBLL != null) {
                permissionBLL.Dispose();
                permissionBLL = null;
            }
        }
    }
}