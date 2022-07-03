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

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class RestaurantBookingSelectedPayment : System.Web.UI.Page
    {
        private RestaurantBookingSelectedPaymentBLL restaurantBookingSelectedPaymentBLL;
        private IList<RestaurantBooking> listRestaurantBooking;
        private PermissionBLL permissionBLL;
        private UserBLL userBLL;
        public RestaurantBookingSelectedPaymentBLL RestaurantBookingSelectedPaymentBLL
        {
            get
            {
                if (restaurantBookingSelectedPaymentBLL == null)
                {
                    restaurantBookingSelectedPaymentBLL = new RestaurantBookingSelectedPaymentBLL();
                }
                return restaurantBookingSelectedPaymentBLL;
            }
        }
        public IList<RestaurantBooking> ListRestaurantBooking
        {
            get
            {
                listRestaurantBooking = new List<RestaurantBooking>();
                try
                {
                    var arrayRestaurantBookingId = Request.QueryString["lbi"].Split(new char[] { ',' });
                    var listRestaurantBookingId = arrayRestaurantBookingId.Select(x => Int32.Parse(x)).ToList();
                    listRestaurantBooking = RestaurantBookingSelectedPaymentBLL.RestaurantBookingGetAllByListId(listRestaurantBookingId)
                        .Future()
                        .OrderBy(x => x.Date)
                        .ToList();
                }
                catch { }
                return listRestaurantBooking;
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
            ClientScript.GetPostBackEventReference(btnPayment, string.Empty);
            if (!IsPostBack)
            {
                LoadBankAccount();
                rptRestaurantBooking.DataSource = ListRestaurantBooking;
                rptRestaurantBooking.DataBind();
            }
        }
        private void LoadBankAccount()
        {
            var listBankAccount = RestaurantBookingSelectedPaymentBLL.BankAccountGetAll().Future().ToList();
            foreach (BankAccount bankAccount in listBankAccount)
            {
                ddlBankAccount.Items.Add(new ListItem(bankAccount.AccName + " - " + bankAccount.AccNo, bankAccount.Id.ToString()));
            }
            ddlBankAccount.Items.Insert(0, new ListItem("Chọn Tài Khoản", ""));

        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (restaurantBookingSelectedPaymentBLL != null)
            {
                restaurantBookingSelectedPaymentBLL.Dispose();
                restaurantBookingSelectedPaymentBLL = null;
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

        protected void btnPayment_Click(object sender, EventArgs e)
        {
            if (!AllowPaymentBooking)
            {
                ShowErrors("Bạn không có quyền thanh toán đặt chỗ");
                return;
            }
            var totalPaid = 0.0;
            try
            {
                totalPaid = Double.Parse(txtPaid.Text);
            }
            catch { }
            var restOfTotalPaid = totalPaid;
            var totalOfReceivable = ListRestaurantBooking.Select(x => x.Receivable).Sum();
            var deficit = totalOfReceivable - totalPaid;
            var restaurantBookingHaveLowestReceivable = ListRestaurantBooking.OrderBy(x => x.Receivable).FirstOrDefault();
            if (restaurantBookingHaveLowestReceivable != null)
            {
                if (deficit > restaurantBookingHaveLowestReceivable.Receivable)
                {
                    ShowErrors("Số tiền thiếu sau thanh toán lớn hơn số tiền còn lại nhỏ nhất không thể thực hiện thanh toán");
                    return;
                }
            }
            var restaurantBookingNotPayAndSoonDate = ListRestaurantBooking.Where(x => x.IsPaid == false).OrderBy(x => x.Date).First();
            var paymentGroup = new PaymentGroup();
            foreach (var restaurantBooking in ListRestaurantBooking)
            {
                if (restaurantBooking == restaurantBookingNotPayAndSoonDate) continue;
                var amountMustPay = restaurantBooking.Receivable;
                if (restOfTotalPaid >= amountMustPay)
                {
                    restaurantBooking.TotalPaid += amountMustPay;
                    restaurantBooking.Receivable -= amountMustPay;
                    restOfTotalPaid -= amountMustPay;
                    restaurantBooking.MarkIsPaid = true;
                    restaurantBooking.ReceiptVoucher = txtReceiptVoucher.Text;
                    RestaurantBookingSelectedPaymentBLL.RestaurantBookingSaveOrUpdate(restaurantBooking);
                    var paymentHistory = new PaymentHistory()
                    {
                        Time = DateTime.Now,
                        Createdby = CurrentUser,
                        Payby = restaurantBooking.Agency,
                        RestaurantBooking = restaurantBooking,
                        Amount = amountMustPay,
                        PaymentGroup = paymentGroup,
                    };
                    if (chkPayByBankAccount.Checked)
                    {
                        paymentHistory.IsPayByBankAccount = true;
                        if (!string.IsNullOrEmpty(ddlBankAccount.SelectedValue))
                        {
                            paymentHistory.BankAccount =
                                RestaurantBookingSelectedPaymentBLL.BankAccountGetById(Convert.ToInt32(ddlBankAccount.SelectedValue));
                        }
                    }
                    RestaurantBookingSelectedPaymentBLL.PaymentGroupSaveOrUpdate(paymentGroup);
                    RestaurantBookingSelectedPaymentBLL.PaymentHistorySaveOrUpdate(paymentHistory);
                    var activityLogging = new ActivityLogging()
                    {
                        CreatedTime = DateTime.Now,
                        CreatedBy = CurrentUser,
                        Function = "Chỉnh sửa đặt chỗ",
                        Detail = "Thanh toán",
                        ObjectId = "BookingId:" + restaurantBooking.Id,
                    };
                    RestaurantBookingSelectedPaymentBLL.ActivityLoggingSaveOrUpdate(activityLogging);
                }
            }
            if (restOfTotalPaid > 0)
            {
                restaurantBookingNotPayAndSoonDate.TotalPaid += restOfTotalPaid;
                restaurantBookingNotPayAndSoonDate.Receivable -= restOfTotalPaid;
                if (restaurantBookingNotPayAndSoonDate.Receivable <= 0)
                {
                    restaurantBookingNotPayAndSoonDate.MarkIsPaid = true;
                }
                else
                {
                    restaurantBookingNotPayAndSoonDate.MarkIsPaid = false;
                }
                restaurantBookingNotPayAndSoonDate.ReceiptVoucher = txtReceiptVoucher.Text;
                RestaurantBookingSelectedPaymentBLL.RestaurantBookingSaveOrUpdate(restaurantBookingNotPayAndSoonDate);
                var paymentHistory = new PaymentHistory()
                {
                    Time = DateTime.Now,
                    Createdby = CurrentUser,
                    Payby = restaurantBookingNotPayAndSoonDate.Agency,
                    RestaurantBooking = restaurantBookingNotPayAndSoonDate,
                    Amount = restOfTotalPaid,
                };
                if (chkPayByBankAccount.Checked)
                {
                    paymentHistory.IsPayByBankAccount = true;
                    if (!string.IsNullOrEmpty(ddlBankAccount.SelectedValue))
                    {
                        paymentHistory.BankAccount =
                            RestaurantBookingSelectedPaymentBLL.BankAccountGetById(Convert.ToInt32(ddlBankAccount.SelectedValue));
                    }
                }
                RestaurantBookingSelectedPaymentBLL.PaymentGroupSaveOrUpdate(paymentGroup);
                RestaurantBookingSelectedPaymentBLL.PaymentHistorySaveOrUpdate(paymentHistory);
                var activityLogging = new ActivityLogging()
                {
                    CreatedTime = DateTime.Now,
                    CreatedBy = CurrentUser,
                    Function = "Chỉnh sửa đặt chỗ",
                    Detail = "Thanh toán",
                    ObjectId = "BookingId:" + restaurantBookingNotPayAndSoonDate.Id,
                };
                RestaurantBookingSelectedPaymentBLL.ActivityLoggingSaveOrUpdate(activityLogging);
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ReloadReceivablesPage", "parent.location.href=parent.location.href", true);
        }
    }
}