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

namespace Portal.Modules.OrientalSails.Web
{
    public partial class BookingViewing : System.Web.UI.Page
    {
        private BookingViewingBLL bookingViewingBLL;
        private PermissionBLL permissionBLL;
        private UserBLL userBLL;
        public BookingViewingBLL BookingViewingBLL
        {
            get
            {
                if (bookingViewingBLL == null)
                    bookingViewingBLL = new BookingViewingBLL();
                return bookingViewingBLL;
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
                var restaurantBooking = BookingViewingBLL.RestaurantBookingGetById(restaurantBookingId);
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
        public bool AllowEditBooking
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowEditBooking);
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
            this.Master.Title = "Chi tiết đặt chỗ";
            var allowAccess = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowAccessBookingViewingPage);
            if (!allowAccess)
            {
                ShowErrors("Bạn không có quyền truy cập vào trang này");
                plhAdminContent.Visible = false;
                return;
            }
            if (!AllowViewHistoryBooking)
            {
                plhHistory.Visible = false;
                plhHistoryErrorMessage.Visible = true;
            }
            if (!Page.IsPostBack)
            {
                ddlMenu.DataSource = BookingViewingBLL.MenuGetAll();
                ddlMenu.DataTextField = "Name";
                ddlMenu.DataValueField = "Id";
                ddlMenu.DataBind();
                ddlMenu.SelectedValue = "-1";
                LoadFormData();
                BindingHistoryData();
                DisplayLockOrUnlockButton();
            }
        }
        public void LoadFormData()
        {
            try
            {
                ddlMenu.SelectedValue = RestaurantBooking.Menu.Id.ToString();
            }
            catch { }
            txtDate.Text = RestaurantBooking.Date.HasValue ? RestaurantBooking.Date.Value.ToString("dd/MM/yyyy") : "";
            txtTime.Text = RestaurantBooking.Time;
            agencySelector.Value = "";
            try
            {
                agencySelector.Value = RestaurantBooking.Agency.Id.ToString();
            }
            catch { }
            txtNumberOfPaxAdult.Text = RestaurantBooking.NumberOfPaxAdult.ToString();
            txtNumberOfPaxChild.Text = RestaurantBooking.NumberOfPaxChild.ToString();
            txtNumberOfPaxBaby.Text = RestaurantBooking.NumberOfPaxBaby.ToString();
            txtCostPerPersonAdult.Text = RestaurantBooking.CostPerPersonAdult.ToString();
            txtCostPerPersonChild.Text = RestaurantBooking.CostPerPersonChild.ToString();
            txtCostPerPersonBaby.Text = RestaurantBooking.CostPerPersonBaby.ToString();
            txtNumberOfDiscountedPaxAdult.Text = RestaurantBooking.NumberOfDiscountedPaxAdult.ToString();
            txtNumberOfDiscountedPaxChild.Text = RestaurantBooking.NumberOfDiscountedPaxChild.ToString();
            txtNumberOfDiscountedPaxBaby.Text = RestaurantBooking.NumberOfDiscountedPaxBaby.ToString();
            txtTotalPriceOfSet.Text = RestaurantBooking.TotalPriceOfSet.ToString();
            ddlStatus.SelectedValue = ((int)RestaurantBooking.Status).ToString();
            if (RestaurantBooking.Payment == 1)
            {
                rbPayNow.Checked = true;
            }
            if (RestaurantBooking.Payment == 2)
            {
                rbDebt.Checked = true;
            }
            if (Request["Notify"] == "0")
            {
                chkVAT.Checked = RestaurantBooking.Agency.IsVat;
                switch (RestaurantBooking.Agency.PaymentType)
                {
                    case 1:
                        rbPayNow.Checked = true;
                        break;
                    case 2:
                        rbDebt.Checked = true;
                        break;
                }
            }
            chkGala.Checked = RestaurantBooking.GALA;
            ddlTablePosition.SelectedValue = RestaurantBooking.VITRIBANAN.ToString();
            txtSpecialRequest.Text = RestaurantBooking.SpecialRequest;
            txtMenuDetail.Text = RestaurantBooking.MenuDetail;
            ddlPartOfDay.SelectedValue = RestaurantBooking.PartOfDay.ToString();
            txtReason.Text = RestaurantBooking.Reason;
        }
        public void BindingHistoryData()
        {
            var listTrackingChangeBooking = BookingViewingBLL.TrackingChangeBookingGetAllByBookingId(RestaurantBooking.Id);
            rptHistoryStatus.DataSource = listTrackingChangeBooking.Where(x => x.ColumnName == "Status").OrderBy(x => x.Id);
            rptHistoryStatus.DataBind();
            rptHistoryDate.DataSource = listTrackingChangeBooking.Where(x => x.ColumnName == "Date").OrderBy(x => x.Id);
            rptHistoryDate.DataBind();
            rptHistoryNumberOfSet.DataSource = listTrackingChangeBooking.Where(x => x.ColumnName == "NumberOfSet").OrderBy(x => x.Id);
            rptHistoryNumberOfSet.DataBind();
            rptHistoryPricePerPerson.DataSource = listTrackingChangeBooking.Where(x => x.ColumnName == "CostPerPerson").OrderBy(x => x.Id);
            rptHistoryPricePerPerson.DataBind();
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (bookingViewingBLL != null)
            {
                bookingViewingBLL.Dispose();
                bookingViewingBLL = null;
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!AllowEditBooking)
            {
                ShowErrors("Bạn không có quyền sửa đặt chỗ");
                return;
            }
            if (RestaurantBooking.LockStatus == LockStatusEnum.Locked)
            {
                ShowErrors("Đặt chỗ đã bị khóa không thể lưu thông tin được");
                return;
            }
            RestaurantBooking.InitializeTrackingChangeValue();
            RestaurantBooking.CurrentUser = CurrentUser;
            RestaurantBooking.Menu = BookingViewingBLL.MenuGetById(Int32.Parse(ddlMenu.SelectedValue));
            RestaurantBooking.Agency = BookingViewingBLL.AgencyGetById(Int32.Parse(agencySelector.Value));
            RestaurantBooking.Date = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            RestaurantBooking.Status = (StatusEnum)Int32.Parse(ddlStatus.SelectedValue);
            RestaurantBooking.Time = txtTime.Text;
            var numberOfPaxAdult = 0;
            try
            {
                numberOfPaxAdult = Int32.Parse(txtNumberOfPaxAdult.Text);
            }
            catch { }
            RestaurantBooking.NumberOfPaxAdult = numberOfPaxAdult;
            var numberOfPaxChild = 0;
            try
            {
                numberOfPaxChild = Int32.Parse(txtNumberOfPaxChild.Text);
            }
            catch { }
            RestaurantBooking.NumberOfPaxChild = numberOfPaxChild;
            var numberOfPaxBaby = 0;
            try
            {
                numberOfPaxBaby = Int32.Parse(txtNumberOfPaxBaby.Text);
            }
            catch { }
            RestaurantBooking.NumberOfPaxBaby = numberOfPaxBaby;
            var costPerPersonAdult = 0.0;
            try
            {
                costPerPersonAdult = Double.Parse(txtCostPerPersonAdult.Text);
            }
            catch { }
            RestaurantBooking.CostPerPersonAdult = costPerPersonAdult;
            var costPerPersonChild = 0.0;
            try
            {
                costPerPersonChild = Double.Parse(txtCostPerPersonChild.Text);
            }
            catch { }
            RestaurantBooking.CostPerPersonChild = costPerPersonChild;
            var costPerPersonBaby = 0.0;
            try
            {
                costPerPersonBaby = Double.Parse(txtCostPerPersonBaby.Text);
            }
            catch { }
            RestaurantBooking.CostPerPersonBaby = costPerPersonBaby;
            var numberOfDiscountedPaxAdult = 0;
            try
            {
                numberOfDiscountedPaxAdult = Int32.Parse(txtNumberOfDiscountedPaxAdult.Text);
            }
            catch { }
            RestaurantBooking.NumberOfDiscountedPaxAdult = numberOfDiscountedPaxAdult;
            var numberOfDiscountedPaxChild = 0;
            try
            {
                numberOfDiscountedPaxChild = Int32.Parse(txtNumberOfDiscountedPaxChild.Text);
            }
            catch { }
            RestaurantBooking.NumberOfDiscountedPaxChild = numberOfDiscountedPaxChild;
            var numberOfDiscountedPaxBaby = 0;
            try
            {
                numberOfDiscountedPaxBaby = Int32.Parse(txtNumberOfDiscountedPaxBaby.Text);
            }
            catch { }
            RestaurantBooking.NumberOfDiscountedPaxBaby = numberOfDiscountedPaxBaby;
            RestaurantBooking.SpecialRequest = txtSpecialRequest.Text;
            RestaurantBooking.Time = txtTime.Text;
            if (rbPayNow.Checked)
            {
                RestaurantBooking.Payment = 1;
            }
            if (rbDebt.Checked)
            {
                RestaurantBooking.Payment = 2;
            }
            RestaurantBooking.VAT = chkVAT.Checked;
            RestaurantBooking.PartOfDay = Int32.Parse(ddlPartOfDay.SelectedValue);
            RestaurantBooking.TotalPriceOfSet = Double.Parse(txtTotalPriceOfSet.Text);
            RestaurantBooking.Receivable = RestaurantBooking.TotalPrice - RestaurantBooking.TotalPaid;
            RestaurantBooking.MenuDetail = txtMenuDetail.Text;
            RestaurantBooking.TrackingChangeNumberOfSet = String.Format("Tổng số suất ăn: {0} <br/>Người lớn: {1}<br/>Trẻ em: {2}<br/>Sơ sinh: {3}", RestaurantBooking.TotalSet, RestaurantBooking.NumberOfPaxAdult, RestaurantBooking.NumberOfPaxChild, RestaurantBooking.NumberOfPaxBaby);
            RestaurantBooking.TrackingChangeCostPerPerson = String.Format("Người lớn: {0}₫<br/>Trẻ em : {1}₫<br/>Baby: {2}₫", RestaurantBooking.CostPerPersonAdult.ToString("#,##0.##"), RestaurantBooking.CostPerPersonChild.ToString("#,##0.##"), RestaurantBooking.CostPerPersonBaby.ToString("#,##0.##"));
            RestaurantBooking.LastEditedBy = CurrentUser;
            RestaurantBooking.LastEditedDate = DateTime.Now;
            if (RestaurantBooking.Status == StatusEnum.Cancel)
            {
                RestaurantBooking.Reason = txtReason.Text;
            }
            else
            {
                RestaurantBooking.Reason = "";
                txtReason.Text = "";
            }
            RestaurantBooking.GALA = chkGala.Checked;
            RestaurantBooking.VITRIBANAN = Int32.Parse(ddlTablePosition.SelectedValue);
            BookingViewingBLL.RestaurantBookingSaveOrUpdate(RestaurantBooking);
            ShowSuccess("Cập nhật booking thành công");
            var activityLogging = new ActivityLogging()
            {
                CreatedTime = DateTime.Now,
                CreatedBy = CurrentUser,
                Function = "Chỉnh sửa đặt chỗ",
                Detail = "Sửa đặt chỗ",
                ObjectId = "BookingId:" + RestaurantBooking.Id,
            };
            BookingViewingBLL.ActivityLoggingSaveOrUpdate(activityLogging);
            BindingHistoryData();
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

        protected void btnLock_Click(object sender, EventArgs e)
        {
            if (!PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowLockBooking))
            {
                ShowErrors("Bạn không có quyền khóa booking!");
                return;
            }
            RestaurantBooking.LockStatus = LockStatusEnum.Locked;
            BookingViewingBLL.RestaurantBookingSaveOrUpdate(RestaurantBooking);
            LoadFormData();
            DisplayLockOrUnlockButton();
        }

        protected void btnUnlock_Click(object sender, EventArgs e)
        {
            if (!PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowUnlockBooking))
            {
                ShowErrors("Bạn không có quyền mở khóa booking!");
                return;
            }
            if (RestaurantBooking.OnlyUnlockByRole != OnlyUnlockByRoleEnum.Null)
            {
                if (!PermissionBLL.UserCheckRole(CurrentUser.Id, (int)OnlyUnlockByRoleEnum.Administrator))
                {
                    ShowErrors("Booking này hiện chỉ có administrator có thể mở khóa!");
                    return;
                }
            }
            RestaurantBooking.LockStatus = LockStatusEnum.Unlocked;
            RestaurantBooking.LastUnlockedTime = DateTime.Now;
            BookingViewingBLL.RestaurantBookingSaveOrUpdate(RestaurantBooking);
            LoadFormData();
            DisplayLockOrUnlockButton();
        }
        public void DisplayLockOrUnlockButton()
        {
            if (RestaurantBooking.LockStatus == LockStatusEnum.NotLock || RestaurantBooking.LockStatus == LockStatusEnum.Unlocked)
            {
                btnLock.Visible = true;
                btnUnlock.Visible = false;
            }
            else
            {
                btnLock.Visible = false;
                btnUnlock.Visible = true;
            }
        }
    }
}