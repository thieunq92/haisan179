using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GemBox.Spreadsheet;
using iTextSharp.text.pdf;
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using Portal.Modules.OrientalSails.Enums;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class ReportVAT : SailsAdminBase
    {
        private double _totalPrice = 0.0;
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
        public bool AllowSaveVAT
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowSaveVAT);
            }
        }
        public bool AllowExportVAT
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowExportVAT);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.Title = "Báo cáo quản lý VAT";
            var allowAccess = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowAccessReportVATPage);
            if (!allowAccess)
            {
                ShowErrors("Bạn không có quyền truy cập vào trang này");
                plhAdminContent.Visible = false;
                return;
            }
            if (!IsPostBack)
            {
                GetReportData();
                BuildOrderLink();
            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
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
            var date = Request["date"];
            if (!string.IsNullOrEmpty(date))
            {
                txtDate.Text = date;
            }
            rptReport.DataSource = Module.GetReportVat(Request.QueryString, from, to, agencyId, codeIntType, date);
            rptReport.DataBind();
        }
        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReportVAT.aspx" + QueryStringBuildByCriterion());
        }
        private void BuildOrderLink()
        {
            var url = "ReportVAT.aspx" + QueryStringBuildByCriterion();
            var odateUrl = url;
            var oagencyUrl = url;
            var icond = "";
            var icona = "";
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
                odateUrl += "&od=asc";
            }
            if (!string.IsNullOrEmpty(Request["oa"]))
            {
                if (Request["oa"].ToLower() == "asc")
                {
                    oagencyUrl += "&oa=desc";
                    icona = "<i class=\"fas fa-sort-up\"></i>";
                }
                else
                {
                    oagencyUrl += "&oa=asc";
                    icona = "<i class=\"fas fa-sort-down\"></i>";
                }
            }
            else
            {
                oagencyUrl += "&oa=asc";
            }
            litOrderDate.Text = string.Format("<a href='{0}'>Ngày</a>{1}", odateUrl, icond);
            litOrderAgency.Text = string.Format("<a href='{0}'>Đối tác</a>{1}", oagencyUrl, icona);
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
            if (!string.IsNullOrEmpty(agencySelector.Value))
            {
                nvcQueryString.Add("ai", agencySelector.Value);
            }

            if (!string.IsNullOrEmpty(txtCode.Text))
            {
                nvcQueryString.Add("code", txtCode.Text);
            }
            if (!string.IsNullOrEmpty(txtDate.Text))
            {
                nvcQueryString.Add("date", txtDate.Text);
            }
            //if (!string.IsNullOrEmpty(Request["od"]))
            //{
            //    nvcQueryString.Add("od", Request["od"]);
            //}
            //if (!string.IsNullOrEmpty(Request["oa"]))
            //{
            //    nvcQueryString.Add("oa", Request["oa"]);
            //}
            var criterions = (from key in nvcQueryString.AllKeys
                              from value in nvcQueryString.GetValues(key)
                              select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value))).ToArray();

            return "?" + string.Join("&", criterions);
        }

        protected void rptReport_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var booking = e.Item.DataItem as RestaurantBooking;
            if (booking != null)
            {
                var hplCode = (HyperLink)e.Item.FindControl("hplCode");
                if (hplCode != null)
                {
                    hplCode.Text = booking.Code;
                    hplCode.NavigateUrl = string.Format("BookingViewing.aspx{0}&bi={1}", GetBaseQueryString(),
                        booking.Id);
                }
                Literal litDate = (Literal)e.Item.FindControl("litDate");
                if (litDate != null)
                {
                    litDate.Text = booking.Date != null ? Convert.ToDateTime(booking.Date).ToString("dd/MM/yyyy") : "";
                }
                Literal litAgency = (Literal)e.Item.FindControl("litAgency");
                if (litAgency != null)
                {
                    litAgency.Text = booking.Agency != null ? booking.Agency.Name : "";
                }
                Literal litPrice = (Literal)e.Item.FindControl("litPrice");
                if (litPrice != null)
                {
                    var totalVat = booking.TotalPriceOfSet;
                    foreach (ServiceOutside serviceOutside in booking.ListServiceOutside)
                    {
                        if (serviceOutside.VAT)
                        {
                            totalVat += serviceOutside.TotalPrice;
                        }
                    }
                    _totalPrice += totalVat;
                    litPrice.Text = totalVat.ToString("#,##0.##");
                }
                CheckBox chkIsExportVat = (CheckBox)e.Item.FindControl("chkIsExportVat");
                if (chkIsExportVat != null)
                {
                    if (booking.IsExportVat) chkIsExportVat.Visible = false;
                    else chkIsExportVat.Checked = booking.IsExportVat;

                }
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                Literal litTotalPrice = (Literal)e.Item.FindControl("litTotalPrice");
                if (litTotalPrice != null)
                {
                    litTotalPrice.Text = _totalPrice.ToString("#,##0.##");
                }
            }
        }

        protected void btnExport_OnClick(object sender, EventArgs e)
        {
            if (!AllowExportVAT) {
                ShowErrors("Bạn không có quyền xuất file báo cáo VAT");
                return;
            }
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
            var date = Request["date"];
            if (!string.IsNullOrEmpty(date))
            {
                txtDate.Text = date;
            }
            var list = Module.GetReportVat(Request.QueryString, from, to, agencyId, codeIntType, date);

            ExcelFile excelFile = ExcelFile.Load(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/bao_cao_vat.xls"));
            ExcelWorksheet sheet = excelFile.Worksheets[0];

            const int firstrow = 6;
            int crow = firstrow;

            sheet.Rows.InsertCopy(crow, list.Count(), sheet.Rows[firstrow]);

            var index = 1;
            foreach (RestaurantBooking booking in list)
            {
                sheet.Cells[crow, 0].Value = index;
                sheet.Cells[crow, 1].Value = booking.Code;
                sheet.Cells[crow, 2].Value = booking.Date != null ? Convert.ToDateTime(booking.Date).ToString("dd/MM/yyyy") : "";
                sheet.Cells[crow, 3].Value = booking.Agency != null ? booking.Agency.Name : "";
                var totalVat = booking.TotalPriceOfSet;
                foreach (ServiceOutside serviceOutside in booking.ListServiceOutside)
                {
                    if (serviceOutside.VAT)
                    {
                        totalVat += serviceOutside.TotalPrice;
                    }
                }
                _totalPrice += totalVat;
                sheet.Cells[crow, 4].Value = totalVat.ToString("#,##0.##");
                sheet.Cells[crow, 5].Value = booking.IsExportVat ? "Có" : "Chưa";
                crow++;
                index++;
            }

            sheet.Cells[crow, 3].Value = "Tổng";
            sheet.Cells[crow, 4].Value = _totalPrice.ToString("#,##0.##");

            excelFile.Save(Response, "bao_cao_vat.xlsx");
        }

        protected void btnSaveStatusExport_OnClick(object sender, EventArgs e)
        {
            if (!AllowSaveVAT) {
                ShowErrors("Bạn không có quyền lưu hóa đơn đã xuất VAT");
                return;
            }
            foreach (RepeaterItem item in rptReport.Items)
            {
                if (item.ItemType == ListItemType.Item)
                {
                    CheckBox chkIsExportVat = (CheckBox)item.FindControl("chkIsExportVat");
                    if (chkIsExportVat != null)
                    {
                        if (chkIsExportVat.Checked && chkIsExportVat.Visible)
                        {
                            HiddenField hidId = (HiddenField)item.FindControl("hidId");
                            if (hidId != null)
                            {
                                var restaurantBooking = Module.GetObjectById(typeof(RestaurantBooking), Convert.ToInt32(hidId.Value)) as RestaurantBooking;
                                if (restaurantBooking != null)
                                {
                                    restaurantBooking.IsExportVat = true;
                                    Module.SaveOrUpdate(restaurantBooking);
                                }
                            }
                        }
                    }
                }
            }
            GetReportData();
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