using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GemBox.Spreadsheet;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Enums;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class ReportAccountPayment : SailsAdminBasePage
    {
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
        public bool AllowExportAccountPayment
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowExportAccountPayment);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.Title = "Báo cáo tài khoản thanh toán";
            var allowAccess = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowAccessMenuEditingPage);
            if (!allowAccess)
            {
                ShowErrors("Bạn không có quyền truy cập vào trang này");
                plhAdminContent.Visible = false;
                return;
            }
            if (!IsPostBack)
            {
                LoadBankAccount();
                GetReportData();
            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
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
        private void LoadBankAccount()
        {
            var list = Module.LoadAllBankAccount();
            foreach (BankAccount bankAccount in list)
            {
                ddlBankAccount.Items.Add(new ListItem(bankAccount.AccName + " - " + bankAccount.AccNo, bankAccount.Id.ToString()));
            }
            ddlBankAccount.Items.Insert(0, new ListItem("Chọn Tài Khoản", ""));

        }
        private void GetReportData()
        {
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var from = firstDayOfMonth;
            if (!string.IsNullOrEmpty(Request.QueryString["f"]))
            {
                from = DateTime.ParseExact(Request.QueryString["f"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            var to = lastDayOfMonth;
            if (!string.IsNullOrEmpty(Request.QueryString["t"]))
            {
                to = DateTime.ParseExact(Request.QueryString["t"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            txtFrom.Text = from.ToString("dd/MM/yyyy");
            txtTo.Text = to.ToString("dd/MM/yyyy");
            if (!string.IsNullOrEmpty(Request.QueryString["ba"]))
            {
                ddlBankAccount.SelectedValue = Request["ba"];
            }

            var agencyId = -1;
            if (!string.IsNullOrEmpty(Request.QueryString["ai"]))
            {
                agencyId = Int32.Parse(Request.QueryString["ai"]);
                var agency = Module.AgencyGetById(Convert.ToInt32(agencyId));
                agencySelector.Value = agency.Id.ToString();
                agencySelectornameid.Text = agency.Name;
            }
            var code = Request["code"];
            var codeIntType = -1;

            if (!string.IsNullOrEmpty(code))
            {
                txtCode.Text = code;
                try
                {
                    codeIntType = Int32.Parse(code.Remove(0, 2).TrimStart('0'));
                }
                catch { }
            }
            var listPaymentHistoryHaveGroupPaymentNull = Module.GetReportPaymentHistoryByBankAccount(Request.QueryString, from, to, agencyId, codeIntType).Where(x => x.PaymentGroup == null);
            var listGroupedPaymentHistory = Module.GetReportPaymentHistoryByBankAccount(Request.QueryString, from, to, agencyId, codeIntType).Except(listPaymentHistoryHaveGroupPaymentNull).GroupBy(x => x.PaymentGroup);
            var listPaymentHistory = new List<PaymentHistory>();
            listPaymentHistory.AddRange(listPaymentHistoryHaveGroupPaymentNull);
            foreach (var groupedPaymentHistory in listGroupedPaymentHistory)
            {
                var paymentHistory = new PaymentHistory()
                {
                    Time = groupedPaymentHistory.First().Time,
                    BankAccount = groupedPaymentHistory.First().BankAccount,
                    Amount = groupedPaymentHistory.Select(x => x.Amount).Sum(),
                };
                listPaymentHistory.Add(paymentHistory);
            }
            rptReport.DataSource = listPaymentHistory;
            rptReport.DataBind();
            litTotalPrice.Text = listPaymentHistory.Sum(paymentHistory => paymentHistory.Amount).ToString("#,##0.##");

        }
        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReportAccountPayment.aspx" + QueryStringBuildByCriterion());
        }
        public string QueryStringBuildByCriterion()
        {
            NameValueCollection nvcQueryString = new NameValueCollection();
            nvcQueryString.Add("NodeId", "1");
            nvcQueryString.Add("SectionId", "15");

            if (!string.IsNullOrEmpty(txtFrom.Text))
            {
                nvcQueryString.Add("f", txtFrom.Text);
            }

            if (!string.IsNullOrEmpty(txtTo.Text))
            {
                nvcQueryString.Add("t", txtTo.Text);
            }
            if (!string.IsNullOrEmpty(ddlBankAccount.SelectedValue))
            {
                nvcQueryString.Add("ba", ddlBankAccount.SelectedValue);
            }
            if (!string.IsNullOrEmpty(agencySelector.Value))
            {
                nvcQueryString.Add("ai", agencySelector.Value);
            }

            if (!string.IsNullOrEmpty(txtCode.Text))
            {
                nvcQueryString.Add("code", txtCode.Text);
            }
            var criterions = (from key in nvcQueryString.AllKeys
                              from value in nvcQueryString.GetValues(key)
                              select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value))).ToArray();

            return "?" + string.Join("&", criterions);
        }

        protected void btnExport_OnClick(object sender, EventArgs e)
        {
            if (!AllowExportAccountPayment) {
                ShowErrors("Bạn không có quyền xuất file báo cáo tài khoản thanh toán");
                return;
            }
            var excelFile = ExcelFile.Load(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/bao_cao_tai_khoan_thanh_toan.xlsx"));
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var from = firstDayOfMonth;
            if (!string.IsNullOrEmpty(Request.QueryString["f"]))
            {
                from = DateTime.ParseExact(Request.QueryString["f"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            var to = lastDayOfMonth;
            if (!string.IsNullOrEmpty(Request.QueryString["t"]))
            {
                to = DateTime.ParseExact(Request.QueryString["t"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            var agencyId = -1;
            if (!string.IsNullOrEmpty(Request.QueryString["ai"]))
            {
                agencyId = Int32.Parse(Request.QueryString["ai"]);
                var agency = Module.AgencyGetById(Convert.ToInt32(agencyId));
                agencySelector.Value = agency.Id.ToString();
                agencySelectornameid.Text = agency.Name;
            }
            var code = Request["code"];
            var codeIntType = -1;

            if (!string.IsNullOrEmpty(code))
            {
                txtCode.Text = code;
                try
                {
                    codeIntType = Int32.Parse(code.Remove(0, 2).TrimStart('0'));
                }
                catch { }
            }
            var list = Module.GetReportPaymentHistoryByBankAccount(Request.QueryString, from, to, agencyId, codeIntType);
            var agencyList = new Dictionary<Agency, List<PaymentHistory>>();

            foreach (var paymentHistory in list)
            {
                if (!agencyList.Keys.Contains(paymentHistory.Payby))
                {
                    var bookings = new List<PaymentHistory> { paymentHistory };
                    agencyList.Add(paymentHistory.Payby, bookings);
                }
                else
                {
                    agencyList[paymentHistory.Payby].Add(paymentHistory);
                }
            }

            foreach (var agencyKey in agencyList)
            {
                var sheetName = agencyKey.Key.TradingName.Trim();
                ExcelWorksheet sheet = null;
                if (!excelFile.Worksheets.Contains(sheetName))
                {
                    sheet = excelFile.Worksheets.InsertCopy(excelFile.Worksheets.Count, sheetName,
                        excelFile.Worksheets[0]);
                }
                else sheet = excelFile.Worksheets[sheetName];
                FillDataToSheet(sheet, agencyKey.Value, from, to);
            }
            ExcelWorksheet sheetAll = excelFile.Worksheets[0];
            FillDataToSheet(sheetAll, list, from, to);
            excelFile.Save(Response, "bao_cao_tai_khoan_thanh_toan.xlsx");
        }

        private void FillDataToSheet(ExcelWorksheet sheet, IList<PaymentHistory> list, DateTime from, DateTime to)
        {
            const int firstrow = 6;
            int crow = firstrow;
            sheet.Rows.InsertCopy(crow + 1, list.Count, sheet.Rows[firstrow]);
            sheet.Cells["B4"].Value = from.ToString("dd/MM/yyyy");
            sheet.Cells["D4"].Value = to.ToString("dd/MM/yyyy");
            var stt = 1;
            var totalPrice = 0.0;
            foreach (var booking in list)
            {
                sheet.Cells[crow, 0].Value = stt;
                sheet.Cells[crow, 1].Value = booking.Time != null ? Convert.ToDateTime(booking.Time).ToString("dd/MM/yyyy") : "";
                sheet.Cells[crow, 2].Value = booking.BankAccount.AccName + " - " + booking.BankAccount.AccNo;
                sheet.Cells[crow, 3].Value = booking.Amount.ToString("#,##0.##");
                totalPrice += booking.Amount;
                stt++;
                crow++;
            }
            sheet.Cells[crow, 2].Value = "Tổng";
            sheet.Cells[crow, 3].Value = totalPrice.ToString("#,##0.##");
            totalPrice = 0;
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