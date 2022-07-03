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
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Enums;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class Revenue : SailsAdminBase
    {
        private int totalPaxSang = 0, totalPaxTrua = 0, totalPaxToi = 0;
        private double totalTongGia = 0.0, totalTrichNgoai = 0.0, totalDichVuNgoai = 0.0, totalThucThu = 0.0, totalDaThanhToan = 0.0, totalConLai = 0.0;
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
        public bool AllowExportRevenue
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowExportRevenue);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.Title = "Báo cáo doanh thu";
            var allowAccess = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowAccessRevenuePage);
            if (!allowAccess)
            {
                ShowErrors("Bạn không có quyền truy cập vào trang này");
                plhAdminContent.Visible = false;
                return;
            }
            if (!IsPostBack)
            {
                GetReceivableData();
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
            txtFrom.Text = from.ToString("dd/MM/yyyy");
            txtTo.Text = to.ToString("dd/MM/yyyy");
            IList list = new ArrayList();
            while (from <= to)
            {
                list.Add(from);
                from = from.AddDays(1);
            }
            rptRevenue.DataSource = list;
            rptRevenue.DataBind();
        }

        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            Response.Redirect("Revenue.aspx" + QueryStringBuildByCriterion());
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

            var criterions = (from key in nvcQueryString.AllKeys
                              from value in nvcQueryString.GetValues(key)
                              select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value))).ToArray();

            return "?" + string.Join("&", criterions);
        }

        protected void rptRevenue_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is DateTime)
            {
                DateTime date = (DateTime)e.Item.DataItem;
                HyperLink hplDate = (HyperLink)e.Item.FindControl("hplDate");
                if (hplDate != null)
                {
                    hplDate.Text = date.ToString("dd/MM/yyyy");
                    hplDate.NavigateUrl = string.Format("RestaurantBookingByDate.aspx?NodeId={0}&SectionId={1}&d={2}",
                        Node.Id,
                        Section.Id, HttpUtility.UrlEncode(date.ToString("dd/MM/yyyy")));
                }
                var bookings =
                    Module.BookingGetRevenueByDate(date);
                int paxSang = 0, paxTrua = 0, paxToi = 0;
                var tongGia = 0.0;
                var trichNgoai = 0.0;
                var dichVuNgoai = 0.0;
                var thucThu = 0.0;
                var daThanhToan = 0.0;
                var conLai = 0.0;
                #region --- count data

                foreach (RestaurantBooking booking in bookings)
                {
                    #region -- count pax ---

                    var pax = booking.NumberOfPaxAdult + booking.NumberOfPaxChild + booking.NumberOfPaxBaby;
                    if (booking.PartOfDay == 1)
                    {
                        paxSang += pax;
                        totalPaxSang += pax;
                    }
                    if (booking.PartOfDay == 2)
                    {
                        paxTrua += pax;
                        totalPaxTrua += pax;
                    }
                    if (booking.PartOfDay == 3)
                    {
                        paxToi += pax;
                        totalPaxToi += pax;
                    }

                    #endregion

                    //var tg = ((booking.NumberOfPaxAdult - booking.NumberOfDiscountedPaxAdult) * booking.CostPerPersonAdult) + ((booking.NumberOfPaxChild - booking.NumberOfDiscountedPaxChild) * booking.CostPerPersonChild);
                    tongGia += booking.TotalPrice;
                    totalTongGia += booking.TotalPrice;

                    trichNgoai += booking.TotalCommissionAmount;
                    totalTrichNgoai += booking.TotalCommissionAmount;
                    dichVuNgoai += booking.TotalServiceOutsidePrice;
                    totalDichVuNgoai += booking.TotalServiceOutsidePrice;
                    var tt = (booking.TotalPrice - booking.TotalCommissionAmount) + booking.TotalServiceOutsidePrice;
                    thucThu += booking.ActuallyCollected;
                    totalThucThu += booking.ActuallyCollected;
                    daThanhToan += booking.TotalPaid;
                    totalDaThanhToan += booking.TotalPaid;
                    conLai += booking.Receivable; //tt - booking.TotalPaid;
                    totalConLai += booking.Receivable; //tt - booking.TotalPaid; 
                }

                #endregion

                #region -- display pax --

                Literal litSang = (Literal)e.Item.FindControl("litSang");
                if (litSang != null)
                {
                    litSang.Text = paxSang.ToString();
                }
                Literal litTrua = (Literal)e.Item.FindControl("litTrua");
                if (litTrua != null)
                {
                    litTrua.Text = paxTrua.ToString();
                }
                Literal litToi = (Literal)e.Item.FindControl("litToi");
                if (litToi != null)
                {
                    litToi.Text = paxToi.ToString();
                }

                #endregion
                Literal litTongGia = (Literal)e.Item.FindControl("litTongGia");
                if (litTongGia != null)
                {
                    litTongGia.Text = tongGia.ToString("#,##0.##");
                }
                Literal litTrichNgoai = (Literal)e.Item.FindControl("litTrichNgoai");
                if (litTrichNgoai != null)
                {
                    litTrichNgoai.Text = trichNgoai.ToString("#,##0.##");
                }
                Literal litDichVuNgoai = (Literal)e.Item.FindControl("litDichVuNgoai");
                if (litDichVuNgoai != null)
                {
                    litDichVuNgoai.Text = dichVuNgoai.ToString("#,##0.##");
                }
                Literal litThucThu = (Literal)e.Item.FindControl("litThucThu");
                if (litThucThu != null)
                {
                    litThucThu.Text = thucThu.ToString("#,##0.##");
                }
                Literal litDaThanhToan = (Literal)e.Item.FindControl("litDaThanhToan");
                if (litDaThanhToan != null)
                {
                    litDaThanhToan.Text = daThanhToan.ToString("#,##0.##");
                }
                Literal litConLai = (Literal)e.Item.FindControl("litConLai");
                if (litConLai != null)
                {
                    litConLai.Text = conLai.ToString("#,##0.##");
                }
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                Literal litTotalSang = (Literal)e.Item.FindControl("litTotalSang");
                if (litTotalSang != null)
                {
                    litTotalSang.Text = totalPaxSang.ToString();
                }
                Literal litTotalTrua = (Literal)e.Item.FindControl("litTotalTrua");
                if (litTotalTrua != null)
                {
                    litTotalTrua.Text = totalPaxTrua.ToString();
                }
                Literal litTotalToi = (Literal)e.Item.FindControl("litTotalToi");
                if (litTotalToi != null)
                {
                    litTotalToi.Text = totalPaxToi.ToString();
                }
                Literal litTotalTongGia = (Literal)e.Item.FindControl("litTotalTongGia");
                if (litTotalTongGia != null)
                {
                    litTotalTongGia.Text = totalTongGia.ToString("#,##0.##");
                }
                Literal litTotalTrichNgoai = (Literal)e.Item.FindControl("litTotalTrichNgoai");
                if (litTotalTrichNgoai != null)
                {
                    litTotalTrichNgoai.Text = totalTrichNgoai.ToString("#,##0.##");
                }
                Literal litTotalDichVuNgoai = (Literal)e.Item.FindControl("litTotalDichVuNgoai");
                if (litTotalDichVuNgoai != null)
                {
                    litTotalDichVuNgoai.Text = totalDichVuNgoai.ToString("#,##0.##");
                }
                Literal litTotalThucThu = (Literal)e.Item.FindControl("litTotalThucThu");
                if (litTotalThucThu != null)
                {
                    litTotalThucThu.Text = totalThucThu.ToString("#,##0.##");
                }
                Literal litTotalDaThanhToan = (Literal)e.Item.FindControl("litTotalDaThanhToan");
                if (litTotalDaThanhToan != null)
                {
                    litTotalDaThanhToan.Text = totalDaThanhToan.ToString("#,##0.##");
                }
                Literal litTotalConLai = (Literal)e.Item.FindControl("litTotalConLai");
                if (litTotalConLai != null)
                {
                    litTotalConLai.Text = totalConLai.ToString("#,##0.##");
                }
            }
        }

        protected void btnExport_OnClick(object sender, EventArgs e)
        {
            if (!AllowExportRevenue)
            {
                ShowErrors("Bạn không có quyền trích xuất doanh thu");
                return;
            }
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var from = firstDayOfMonth;
            try
            {
                from = DateTime.ParseExact(Request.QueryString["f"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch
            {
            }
            var to = lastDayOfMonth;
            try
            {
                to = DateTime.ParseExact(Request.QueryString["t"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch
            {
            }
            txtFrom.Text = from.ToString("dd/MM/yyyy");
            txtTo.Text = to.ToString("dd/MM/yyyy");
            IList list = new ArrayList();
            while (from <= to)
            {
                list.Add(from);
                from = from.AddDays(1);
            }
            ExcelFile excelFile = ExcelFile.Load(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/doanh_thu_nha_hang.xls"));
            GemBox.Spreadsheet.ExcelWorksheet sheet = excelFile.Worksheets[0];

            const int firstrow = 6;
            int crow = firstrow;
            sheet.Cells["C4"].Value = from.ToString("dd/MM/yyyy");
            sheet.Cells["F4"].Value = to.ToString("dd/MM/yyyy");

            sheet.Rows.InsertCopy(crow, list.Count, sheet.Rows[firstrow]);

            foreach (var rdate in list)
            {
                DateTime date = (DateTime)rdate;
                sheet.Cells[crow, 0].Value = date.ToString("dd/MM/yyyy");

                var bookings =
                    Module.BookingGetRevenueByDate(date);
                int paxSang = 0, paxTrua = 0, paxToi = 0;
                var tongGia = 0.0;
                var trichNgoai = 0.0;
                var dichVuNgoai = 0.0;
                var thucThu = 0.0;
                var daThanhToan = 0.0;
                var conLai = 0.0;


                foreach (RestaurantBooking booking in bookings)
                {
                    #region -- count pax ---

                    var pax = booking.NumberOfPaxAdult + booking.NumberOfPaxChild + booking.NumberOfPaxBaby;
                    if (booking.PartOfDay == 1)
                    {
                        paxSang += pax;
                        totalPaxSang += pax;
                    }
                    if (booking.PartOfDay == 2)
                    {
                        paxTrua += pax;
                        totalPaxTrua += pax;
                    }
                    if (booking.PartOfDay == 3)
                    {
                        paxToi += pax;
                        totalPaxToi += pax;
                    }

                    #endregion

                    //var tg = ((booking.NumberOfPaxAdult - booking.NumberOfDiscountedPaxAdult) * booking.CostPerPersonAdult) + ((booking.NumberOfPaxChild - booking.NumberOfDiscountedPaxChild) * booking.CostPerPersonChild);
                    tongGia += booking.TotalPrice;
                    totalTongGia += booking.TotalPrice;

                    trichNgoai += booking.TotalCommissionAmount;
                    totalTrichNgoai += booking.TotalCommissionAmount;
                    dichVuNgoai += booking.TotalServiceOutsidePrice;
                    totalDichVuNgoai += booking.TotalServiceOutsidePrice;
                    var tt = (booking.TotalPrice - booking.TotalCommissionAmount) + booking.TotalServiceOutsidePrice;
                    thucThu += booking.ActuallyCollected;
                    totalThucThu += booking.ActuallyCollected;
                    daThanhToan += booking.TotalPaid;
                    totalDaThanhToan += booking.TotalPaid;
                    conLai += booking.Receivable; //tt - booking.TotalPaid;
                    totalConLai += booking.Receivable; //tt - booking.TotalPaid; 
                }

                sheet.Cells[crow, 1].Value = paxSang;
                sheet.Cells[crow, 2].Value = paxTrua;
                sheet.Cells[crow, 3].Value = paxToi;
                sheet.Cells[crow, 4].Value = tongGia.ToString("#,##0.##");
                sheet.Cells[crow, 5].Value = trichNgoai.ToString("#,##0.##");
                sheet.Cells[crow, 6].Value = dichVuNgoai.ToString("#,##0.##");
                sheet.Cells[crow, 7].Value = thucThu.ToString("#,##0.##");
                sheet.Cells[crow, 8].Value = daThanhToan.ToString("#,##0.##");
                sheet.Cells[crow, 9].Value = conLai.ToString("#,##0.##");
                crow++;
            }

            sheet.Cells[crow, 1].Value = totalPaxSang;
            sheet.Cells[crow, 2].Value = totalPaxTrua;
            sheet.Cells[crow, 3].Value = totalPaxToi;
            sheet.Cells[crow, 4].Value = totalTongGia.ToString("#,##0.##");
            sheet.Cells[crow, 5].Value = totalTrichNgoai.ToString("#,##0.##");
            sheet.Cells[crow, 6].Value = totalDichVuNgoai.ToString("#,##0.##");
            sheet.Cells[crow, 7].Value = totalThucThu.ToString("#,##0.##");
            sheet.Cells[crow, 8].Value = totalDaThanhToan.ToString("#,##0.##");
            sheet.Cells[crow, 9].Value = totalConLai.ToString("#,##0.##");
            excelFile.Save(Response, string.Format("doanh_thu_nha_hang_{0:dd_MM_yyy}_{1:dd_MM_yyy}.xlsx", from, to));
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