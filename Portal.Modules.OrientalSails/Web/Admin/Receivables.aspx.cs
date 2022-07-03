using Portal.Modules.OrientalSails.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using GemBox.Spreadsheet;
using Org.BouncyCastle.Ocsp;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using Portal.Modules.OrientalSails.Enums;
using CMS.Core.Domain;


namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class Receivables : SailsAdminBase
    {
        private double _totalPriceOfSet = 0, _totalServiceOutsidePrice = 0, _totalActuallyCollected = 0, _totalPaid = 0, _totalReceivable = 0;
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
        public bool AllowExportReceivable
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowExportReceivable);
            }
        }
        public bool AllowAccessPayment
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowAccessPayment);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.Title = "Báo cáo công nợ";
            var allowAccess = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowAccessReceivablesPage);
            if (!allowAccess)
            {
                ShowErrors("Bạn không có quyền truy cập vào trang này");
                plhAdminContent.Visible = false;
                return;
            }
            if (!IsPostBack)
            {
                FillQueryToForm();
                GetReceivableData();
                BuildOrderLink();
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
        private void BuildOrderLink()
        {
            var url = "Receivables.aspx" + QueryStringBuildByCriterion(null);
            var odateUrl = url;
            var icond = "";
            if (!string.IsNullOrEmpty(Request["od"]))
            {
                if (Request["od"].ToLower() == "asc")
                {
                    odateUrl += "&od=desc";
                    icond = "<i class=\"fas fa-sort-up\"></i>";
                }
                else
                {
                    odateUrl += "&od=asc";
                    icond = "<i class=\"fas fa-sort-down\"></i>";
                }
            }
            else
            {
                icond = "<i class=\"fas fa-sort-up\"></i>";
                odateUrl += "&od=desc";
            }

            litOrderDate.Text = string.Format("<a href='{0}'>Ngày</a>{1}", odateUrl, icond);
        }

        private void FillQueryToForm()
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
            if (string.IsNullOrEmpty(Request["from"]))
            {
                txtFrom.Text = from.ToString("dd/MM/yyyy");
            }
            txtTo.Text = to.ToString("dd/MM/yyyy");
            if (!string.IsNullOrEmpty(Request.QueryString["ai"]))
            {
                var agencyId = Int32.Parse(Request.QueryString["ai"]);
                var agency = Module.AgencyGetById(Convert.ToInt32(agencyId));
                agencySelector.Value = agency.Id.ToString();
                agencySelectornameid.Text = agency.Name;

            }
            if (!string.IsNullOrEmpty(Request["pay"]))
            {
                ddlPayment.SelectedValue = Request["pay"];
            }
            if (!string.IsNullOrEmpty(Request["spay"]))
            {
                ddlStatusPayment.SelectedValue = Request["spay"];
            }
            var code = Request["code"];
            if (!string.IsNullOrEmpty(code))
            {
                txtCode.Text = code;
            }
        }
        private void GetReceivableData()
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
            rptReceivablesTable.DataSource = Module.GetReportReceivables(Request.QueryString, from, to, agencyId, codeIntType);
            rptReceivablesTable.DataBind();
        }

        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            Response.Redirect("Receivables.aspx" + QueryStringBuildByCriterion(null));
        }

        public string QueryStringBuildByCriterion(Agency agency)
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
            if (agency != null)
            {
                nvcQueryString.Add("ai", agency.Id.ToString());
            }
            else
            {
                if (!string.IsNullOrEmpty(agencySelector.Value))
                {
                    nvcQueryString.Add("ai", agencySelector.Value);
                }
            }
            if (!string.IsNullOrEmpty(txtCode.Text))
            {
                nvcQueryString.Add("code", txtCode.Text);
            }
            if (!string.IsNullOrEmpty(ddlPayment.SelectedValue))
            {
                nvcQueryString.Add("pay", ddlPayment.SelectedValue);
            }
            if (!string.IsNullOrEmpty(ddlStatusPayment.SelectedValue))
            {
                nvcQueryString.Add("spay", ddlStatusPayment.SelectedValue);
            }
            var criterions = (from key in nvcQueryString.AllKeys
                              from value in nvcQueryString.GetValues(key)
                              select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value))).ToArray();

            return "?" + string.Join("&", criterions);
        }

        protected void rptReceivablesTable_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is RestaurantBooking)
            {
                var booking = e.Item.DataItem as RestaurantBooking;
                var litLastDatePayment = e.Item.FindControl("litLastDatePayment") as Literal;
                if (litLastDatePayment != null)
                {
                    var payment = booking.ListPaymentHistory.OrderByDescending(p => p.Time).FirstOrDefault();
                    if (payment != null) litLastDatePayment.Text = payment.Time != null ? Convert.ToDateTime(payment.Time).ToString("dd/MM/yyy") : "";
                }
                var litServices = e.Item.FindControl("litServices") as Literal;
                if (litServices != null)
                {
                    if (booking.ListServiceOutside.Count > 0)
                    {
                        var table = "<table>";
                        foreach (ServiceOutside serviceOutside in booking.ListServiceOutside)
                        {
                            table += "<tr>";
                            table += "<td>" + serviceOutside.Service + "</td>";
                            table += "<td>&nbsp;&nbsp;</td>";
                            table += "<td>" + serviceOutside.TotalPrice.ToString("#,##0.##") + "₫" + "</td>";
                            table += "</tr>";
                        }
                        table += "</table>";
                        litServices.Text = table;
                    }
                }
                var hplAgency = (HyperLink)e.Item.FindControl("hplAgency");
                if (hplAgency != null)
                {
                    hplAgency.Text = booking.Agency != null ? booking.Agency.TradingName : "";
                    var query = QueryStringBuildByCriterion(booking.Agency);
                    var url = "Receivables.aspx" + query;
                    hplAgency.NavigateUrl = url;
                }
                _totalPriceOfSet += booking.TotalPriceOfSet;
                _totalServiceOutsidePrice += booking.TotalServiceOutsidePrice;
                _totalActuallyCollected += booking.TotalPriceOfSet + booking.TotalServiceOutsidePrice;
                _totalPaid += booking.TotalPaid;
                _totalReceivable += booking.Receivable;

            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                var litTotalPriceOfSet = e.Item.FindControl("litTotalPriceOfSet") as Literal;
                if (litTotalPriceOfSet != null)
                {
                    litTotalPriceOfSet.Text = _totalPriceOfSet.ToString("#,##0.##") + "₫";
                }
                var litTotalServiceOutsidePrice = e.Item.FindControl("litTotalServiceOutsidePrice") as Literal;
                if (litTotalServiceOutsidePrice != null)
                {
                    litTotalServiceOutsidePrice.Text = _totalServiceOutsidePrice.ToString("#,##0.##") + "₫";
                }
                var litActuallyCollected = e.Item.FindControl("litActuallyCollected") as Literal;
                if (litActuallyCollected != null)
                {
                    litActuallyCollected.Text = _totalActuallyCollected.ToString("#,##0.##") + "₫";
                }
                var litTotalPaid = e.Item.FindControl("litTotalPaid") as Literal;
                if (litTotalPaid != null)
                {
                    litTotalPaid.Text = _totalPaid.ToString("#,##0.##") + "₫";
                }
                var litReceivable = e.Item.FindControl("litReceivable") as Literal;
                if (litReceivable != null)
                {
                    litReceivable.Text = _totalReceivable.ToString("#,##0.##") + "₫";
                }
            }
        }
        protected void btnExport_OnClick(object sender, EventArgs e)
        {
            if (!AllowExportReceivable)
            {
                ShowErrors("Bạn không có quyền trích xuất công nợ");
                return;
            }
            var excelFile = ExcelFile.Load(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/bao_cao_cong_no.xlsx"));
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
            var list = Module.GetReportReceivables(Request.QueryString, from, to, agencyId, codeIntType);
            var agencyList = new Dictionary<Agency, List<RestaurantBooking>>();

            foreach (var booking in list)
            {
                if (!agencyList.Keys.Contains(booking.Agency))
                {
                    var bookings = new List<RestaurantBooking>();
                    bookings.Add(booking);
                    agencyList.Add(booking.Agency, bookings);
                }
                else
                {
                    agencyList[booking.Agency].Add(booking);
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
            excelFile.Save(Response, "bao_cao_cong_no.xlsx");
        }

        private void FillDataToSheet(ExcelWorksheet sheet
        , IList<RestaurantBooking> list, DateTime from, DateTime to)
        {
            const int firstrow = 6;
            int crow = firstrow;
            sheet.Rows.InsertCopy(crow + 1, list.Count, sheet.Rows[firstrow]);
            sheet.Cells["C4"].Value = from.ToString("dd/MM/yyyy");
            sheet.Cells["E4"].Value = to.ToString("dd/MM/yyyy");
            var stt = 1;
            foreach (var booking in list)
            {
                sheet.Cells[crow, 0].Value = stt;
                sheet.Cells[crow, 1].Value = booking.Code;
                sheet.Cells[crow, 2].Value = booking.Agency != null ? booking.Agency.TradingName : "";
                sheet.Cells[crow, 3].Value = booking.Date != null ? Convert.ToDateTime(booking.Date).ToString("dd/MM/yyyy") : "";
                sheet.Cells[crow, 4].Value = booking.PartOfDayString;
                sheet.Cells[crow, 5].Value = booking.Menu != null ? booking.Menu.Name : "";
                sheet.Cells[crow, 6].Value = booking.NumberOfPaxAdult;
                sheet.Cells[crow, 7].Value = booking.NumberOfPaxChild;
                sheet.Cells[crow, 8].Value = booking.NumberOfPaxBaby;
                sheet.Cells[crow, 9].Value = booking.CostPerPersonAdult.ToString("#,##0.##");
                sheet.Cells[crow, 10].Value = booking.CostPerPersonBaby.ToString("#,##0.##");
                sheet.Cells[crow, 11].Value = booking.NumberOfDiscountedPaxAdult;
                sheet.Cells[crow, 12].Value = booking.NumberOfDiscountedPaxBaby;
                sheet.Cells[crow, 13].Value = booking.TotalPriceOfSet.ToString("#,##0.##");
                var dichVu = "";
                foreach (ServiceOutside serviceOutside in booking.ListServiceOutside)
                {
                    dichVu += serviceOutside.Service + " : " + serviceOutside.TotalPrice.ToString("#,##0.##") + Environment.NewLine;
                }
                dichVu += "Tổng : " + booking.TotalServiceOutsidePrice.ToString("#,##0.##");
                sheet.Cells[crow, 14].Value = dichVu;

                sheet.Cells[crow, 15].Value = (booking.TotalPriceOfSet + booking.TotalServiceOutsidePrice).ToString("#,##0.##");
                sheet.Cells[crow, 16].Value = booking.TotalPaid.ToString("#,##0.##");
                sheet.Cells[crow, 17].Value = booking.Receivable.ToString("#,##0.##");

                _totalPriceOfSet += booking.TotalPriceOfSet;
                _totalServiceOutsidePrice += booking.TotalServiceOutsidePrice;
                _totalActuallyCollected += booking.TotalPriceOfSet + booking.TotalServiceOutsidePrice;
                _totalPaid += booking.TotalPaid;
                _totalReceivable += booking.Receivable;

                var payment = booking.ListPaymentHistory.OrderByDescending(p => p.Time).FirstOrDefault();
                if (payment != null) sheet.Cells[crow, 18].Value = payment.Time != null ? Convert.ToDateTime(payment.Time).ToString("dd/MM/yyy") : "";
                stt++;
                crow++;
            }
            sheet.Cells[crow, 12].Value = "Tổng";
            sheet.Cells[crow, 13].Value = _totalPriceOfSet.ToString("#,##0.##");
            sheet.Cells[crow, 14].Value = _totalServiceOutsidePrice.ToString("#,##0.##");
            sheet.Cells[crow, 15].Value = _totalActuallyCollected.ToString("#,##0.##");
            sheet.Cells[crow, 16].Value = _totalPaid.ToString("#,##0.##");
            sheet.Cells[crow, 17].Value = _totalReceivable.ToString("#,##0.##");

            _totalPriceOfSet = 0;
            _totalServiceOutsidePrice = 0;
            _totalActuallyCollected = 0;
            _totalPaid = 0;
            _totalReceivable = 0;
        }

        protected string GetClassBooking(RestaurantBooking booking)
        {
            if (booking.Payment == 1 && booking.Receivable > 0) return "custom-warning";
            if (booking.IsPaid && booking.Receivable <= 0) return "custom-success";
            return "";
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