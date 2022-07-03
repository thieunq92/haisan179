using CMS.Core.Domain;
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Enums;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class RestaurantBookingPayment : SailsAdminBasePage
    {
        private RestaurantBookingPaymentBLL restaurantBookingPaymentBLL;
        private PermissionBLL permissionBLL;
        public RestaurantBookingPaymentBLL RestaurantBookingPaymentBLL
        {
            get
            {
                if (restaurantBookingPaymentBLL == null)
                {
                    restaurantBookingPaymentBLL = new RestaurantBookingPaymentBLL();
                }
                return restaurantBookingPaymentBLL;
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
                var restaurantBooking = RestaurantBookingPaymentBLL.RestaurantBookingGetById(restaurantBookingId);
                if (restaurantBooking.Id == -1)
                {
                    Response.Redirect("404.aspx");
                }
                return restaurantBooking;
            }
        }
        public UserBLL userBLL;
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
        public bool AllowAccessPayment
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowAccessPayment);
            }
        }
        public bool AllowPaymentBooking
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowPaymentBooking);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AllowAccessPayment)
            {
                ShowErrors("Bạn không có quyền truy cập thanh toán đặt chỗ");
                plhAdminContent.Visible = false;
                return;
            }
            if (!Page.IsPostBack)
            {
                chkPaid.Checked = RestaurantBooking.MarkIsPaid;
                LoadBankAccount();
                txtReceiptVoucher.Text = RestaurantBooking.ReceiptVoucher;
                rptPaymentHistory.DataSource = RestaurantBookingPaymentBLL.PaymentHistoryGetByBookingId(RestaurantBooking.Id);
                rptPaymentHistory.DataBind();
            }
        }

        private void LoadBankAccount()
        {
            var list = Module.LoadAllBankAccount();
            foreach (BankAccount bankAccount in list)
            {
                ddlBankAccount.Items.Add(new ListItem(bankAccount.AccName + " - " + bankAccount.AccNo, bankAccount.Id.ToString()));
            }
            ddlBankAccount.Items.Insert(0, new ListItem("Chọn Tài Khoản", ""));

        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (restaurantBookingPaymentBLL != null)
            {
                restaurantBookingPaymentBLL.Dispose();
                restaurantBookingPaymentBLL = null;
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

        protected void btnPayment_Click(object sender, EventArgs e)
        {
            if (!AllowPaymentBooking) {
                ShowErrors("Bạn không có quyền thanh toán đặt chỗ");
                return;
            }
            var paid = 0.0;
            try
            {
                paid = Double.Parse(txtPaid.Text);
            }
            catch { }
            if (paid <= 0 && !chkPaid.Checked)
            {
                return;
            }
            var paymentHistoryAmount = RestaurantBooking.Receivable;
            RestaurantBooking.TotalPaid += paid;
            if (chkPaid.Checked)
            {
                RestaurantBooking.TotalPaid += RestaurantBooking.TotalPrice - RestaurantBooking.TotalPaid;
                RestaurantBooking.Receivable = 0.0;
            }
            else
            {
                RestaurantBooking.Receivable = RestaurantBooking.TotalPrice - RestaurantBooking.TotalPaid;
            }
            RestaurantBooking.MarkIsPaid = chkPaid.Checked;
            RestaurantBooking.ReceiptVoucher = txtReceiptVoucher.Text;
            RestaurantBookingPaymentBLL.RestaurantBookingSaveOrUpdate(RestaurantBooking);
            var paymentGroup = new PaymentGroup();
            var paymentHistory = new PaymentHistory()
            {
                Time = DateTime.Now,
                Createdby = CurrentUser,
                Payby = RestaurantBooking.Agency,
                RestaurantBooking = RestaurantBooking,
                PaymentGroup = paymentGroup,
            };
            if (chkPaid.Checked)
            {
                paymentHistory.Amount = paymentHistoryAmount;
            }
            else
            {
                paymentHistory.Amount = paid;
            }
            if (chkPayByBankAccount.Checked)
            {
                paymentHistory.IsPayByBankAccount = true;
                if (!string.IsNullOrEmpty(ddlBankAccount.SelectedValue))
                {
                    paymentHistory.BankAccount =
                        Module.GetById<BankAccount>(Convert.ToInt32(ddlBankAccount.SelectedValue));
                }
            }
            RestaurantBookingPaymentBLL.PaymentGroupSaveOrUpdate(paymentGroup);
            RestaurantBookingPaymentBLL.PaymentHistorySaveOrUpdate(paymentHistory);
            var activityLogging = new ActivityLogging()
            {
                CreatedTime = DateTime.Now,
                CreatedBy = CurrentUser,
                Function = "Chỉnh sửa đặt chỗ",
                Detail = "Thanh toán",
                ObjectId = "BookingId:" + RestaurantBooking.Id,
            };
            RestaurantBookingPaymentBLL.ActivityLoggingSaveOrUpdate(activityLogging);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ReloadReceivablesPage", "parent.location.href=parent.location.href", true);
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