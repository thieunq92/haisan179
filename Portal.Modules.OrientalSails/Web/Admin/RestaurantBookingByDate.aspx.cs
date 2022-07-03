using CMS.Core.Domain;
using GemBox.Spreadsheet;
using OfficeOpenXml;
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Enums;
using Portal.Modules.OrientalSails.Enums.RestaurantBooking;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class RestaurantBookingByDate : System.Web.UI.Page
    {
        private RestaurantBookingByDateBLL restaurantBookingByDateBLL;
        private PermissionBLL permissionBLL;
        private UserBLL userBLL;
        public RestaurantBookingByDateBLL RestaurantBookingByDateBLL
        {
            get
            {
                if (restaurantBookingByDateBLL == null)
                    restaurantBookingByDateBLL = new RestaurantBookingByDateBLL();
                return restaurantBookingByDateBLL;
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
        public DateTime Date
        {
            get
            {
                var date = DateTime.Now.Date;
                try
                {
                    date = DateTime.ParseExact(Request.QueryString["d"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch { }
                return date;
            }
        }
        public int PartOfDay
        {
            get
            {
                var partOfDay = -1;
                try
                {
                    partOfDay = Int32.Parse(Request.QueryString["pod"]);
                }
                catch { }
                return partOfDay;
            }
        }
        public IList<RestaurantBooking> ListRestaurantBooking
        {
            get
            {
                return RestaurantBookingByDateBLL.RestaurantBookingGetAllByCriterion(Date, PartOfDay, StatusEnum.Approved, StatusEnum.Pending);
            }
        }
        public IList<RestaurantBooking> ListCancelledAndChangeDateBooking
        {
            get
            {
                var listRestaurantBookingWithoutStatus = RestaurantBookingByDateBLL.RestaurantBookingGetAllByCriterion(Date, PartOfDay);
                var listCancelledAndChangeDateBooking = new List<RestaurantBooking>();
                foreach (var restaurantBooking in listRestaurantBookingWithoutStatus)
                {
                    var lastTrackingChangeStatus = restaurantBooking.ListTrackingChangeBooking.Where(x => x.ColumnName == "Status").LastOrDefault();
                    if (lastTrackingChangeStatus != null)
                    {
                        if (Date <= lastTrackingChangeStatus.CreatedDate
                            && lastTrackingChangeStatus.NewValue == "Cancel")
                        {
                            listCancelledAndChangeDateBooking.Add(restaurantBooking);
                            continue;
                        }
                    }
                }
                var createdDate = Date.Date;
                var originValueDate = Date.Date.ToString("M/d/yyyy hh:mm:ss tt");
                var listGroupTrackingChangeBooking = RestaurantBookingByDateBLL.TrackingChangeBookingGetAllByCriterion(createdDate, originValueDate, "Date")
                    .OrderBy(x => x.Id)
                    .GroupBy(x => x.RestaurantBooking);
                if (PartOfDay != -1)
                {
                    listGroupTrackingChangeBooking = listGroupTrackingChangeBooking.Where(x => x.Key.PartOfDay == PartOfDay);
                }

                foreach (var groupTrackingChangeBooking in listGroupTrackingChangeBooking)
                {
                    var lastTrackingChangeBooking = groupTrackingChangeBooking.Key.ListTrackingChangeBooking.LastOrDefault();
                    if (lastTrackingChangeBooking != null)
                    {
                        if (lastTrackingChangeBooking.NewValue == originValueDate)
                        {
                            continue;
                        }
                    }
                    var restaurantBooking = RestaurantBookingByDateBLL.RestaurantBookingGetById(groupTrackingChangeBooking.Key.Id);
                    listCancelledAndChangeDateBooking.Add(restaurantBooking);
                }
                return listCancelledAndChangeDateBooking;
            }
        }
        public bool AllowExportSalesReport
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowExportSalesReport);
            }
        }
        public bool AllowExportForKitchen
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowExportForKitchen);
            }
        }
        public bool AllowPaymentBooking
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowPaymentBooking);
            }
        }
        public bool AllowAccessPayment
        {
            get
            {
                return PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowAccessPayment);
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
            this.Master.Title = "Quản lý đặt chỗ theo ngày";
            var allowAccess = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowAccessBookingManagementByDatePage);
            if (!allowAccess)
            {
                ShowErrors("Bạn không có quyền truy cập vào trang này");
                plhAdminContent.Visible = false;
                return;
            }
            if (!PermissionBLL.UserCheckRole(CurrentUser.Id, (int)Roles.Administrator))
            {
                btnLockDate.Visible = false;
            }
            if (!Page.IsPostBack)
            {
               
                txtDate.Text = Date.ToString("dd/MM/yyyy");
                DisplayData();
            }
        }

        private void DisplayData()
        {
            rptBooking.DataSource = ListRestaurantBooking.OrderBy(x => x.PartOfDay).ThenBy(x => x.Time);
            rptBooking.DataBind();
            rptCancelledAndChangeDateBooking.DataSource = ListCancelledAndChangeDateBooking.OrderBy(x => x.PartOfDay).ThenBy(x => x.Time);
            rptCancelledAndChangeDateBooking.DataBind();
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
            if (permissionBLL != null)
            {
                permissionBLL.Dispose();
                permissionBLL = null;
            }
        }
        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            Response.Redirect("RestaurantBookingByDate.aspx" + QueryStringBuildByCriterion(-1));
        }
        public string QueryStringBuildByCriterion()
        {
            return QueryStringBuildByCriterion(-1);
        }
        public string QueryStringBuildByCriterion(int partOfDay)
        {
            NameValueCollection nvcQueryString = new NameValueCollection();
            nvcQueryString.Add("NodeId", "1");
            nvcQueryString.Add("SectionId", "15");

            if (!string.IsNullOrEmpty(txtDate.Text))
            {
                nvcQueryString.Add("d", txtDate.Text);
            }
            if (partOfDay != -1)
            {
                nvcQueryString.Add("pod", partOfDay.ToString());
            }

            var criterions = (from key in nvcQueryString.AllKeys
                              from value in nvcQueryString.GetValues(key)
                              select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value))).ToArray();

            return "?" + string.Join("&", criterions);
        }

        int totalAdult = 0;
        int totalChild = 0;
        int totalBaby = 0;
        double totalOfTotalPrice = 0.0;
        double totalActuallyCollected = 0.0;
        protected void rptBooking_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var numberOfPaxAdult = 0;
                try
                {
                    numberOfPaxAdult = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "NumberOfPaxAdult"));
                }
                catch { }
                totalAdult += numberOfPaxAdult;
                var numberOfPaxChild = 0;
                try
                {
                    numberOfPaxChild = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "NumberOfPaxChild"));
                }
                catch { }
                totalChild += numberOfPaxChild;
                var numberOfPaxBaby = 0;
                try
                {
                    numberOfPaxBaby = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "NumberOfPaxBaby"));
                }
                catch { }
                totalBaby += numberOfPaxBaby;
                var totalPrice = 0.0;
                try
                {
                    totalPrice = Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "TotalPrice"));
                }
                catch { }
                totalOfTotalPrice += totalPrice;
                var actuallyCollected = 0.0;
                try
                {
                    actuallyCollected = Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "ActuallyCollected"));
                }
                catch { }
                totalActuallyCollected += actuallyCollected;
                var rptServiceOutside = e.Item.FindControl("rptServiceOutside") as Repeater;
                rptServiceOutside.DataSource = ((IList<ServiceOutside>)DataBinder.Eval(e.Item.DataItem, "ListServiceOutside"));
                rptServiceOutside.DataBind();
                var btnLock = e.Item.FindControl("btnLock") as LinkButton;
                var btnUnlock = e.Item.FindControl("btnUnlock") as LinkButton;
                if (!PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowLockBooking))
                {
                    btnLock.Visible = false;
                }
                if (!PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowUnlockBooking))
                {
                    btnUnlock.Visible = false;
                }
                var restaurantBooking = (RestaurantBooking)e.Item.DataItem;
                if (restaurantBooking.OnlyUnlockByRole != OnlyUnlockByRoleEnum.Null)
                {
                    if (!PermissionBLL.UserCheckRole(CurrentUser.Id, (int)OnlyUnlockByRoleEnum.Administrator))
                    { btnUnlock.Visible = false; }
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                var lblTotalAdult = e.Item.FindControl("lblTotalAdult") as Label;
                var lblTotalChild = e.Item.FindControl("lblTotalChild") as Label;
                var lblTotalBaby = e.Item.FindControl("lblTotalBaby") as Label;
                var lblTotalOfTotalPrice = e.Item.FindControl("lblTotalOfTotalPrice") as Label;
                var lblTotalActuallyCollected = e.Item.FindControl("lblTotalActuallyCollected") as Label;
                lblTotalAdult.Text = totalAdult.ToString();
                lblTotalChild.Text = totalChild.ToString();
                lblTotalBaby.Text = totalBaby.ToString();
                lblTotalOfTotalPrice.Text = totalOfTotalPrice.ToString("#,##0.##") + "₫";
                lblTotalActuallyCollected.Text = totalActuallyCollected.ToString("#,##0.##") + "₫";
            }
        }

        protected void btnSalesReportExport_Click(object sender, EventArgs e)
        {
            if (!AllowExportSalesReport)
            {
                ShowErrors("Bạn không có quyền trích xuất báo cáo doanh thu");
                return;
            }
            var fileInfo = new FileInfo(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/DoanhThuNgay.xlsx"));
            MemoryStream mem = new MemoryStream();

            using (var excelPackage = new ExcelPackage(fileInfo))
            {
                var worksheet = excelPackage.Workbook.Worksheets["Doanh thu"];
                worksheet.Cells["F4"].Value = Date.ToString("dd/MM/yyyy");
                var listRestaurantBooking = ListRestaurantBooking.OrderBy(x => x.PartOfDay).ThenBy(x => x.Time);
                var index = 1;
                var rowFrom = 7;
                var currentRow = rowFrom + index;
                var totalSet = 0;
                var totalFOC = 0;
                var totalOfTotalPriceOfSet = 0.0;
                var totalActuallyCollected = 0.0;
                var totalOfTotalCommissionAmount = 0.0;
                var totalOfTotalServiceOutsidePrice = 0.0;
                foreach (var restaurantBooking in listRestaurantBooking)
                {
                    worksheet.InsertRow(currentRow, 1, rowFrom);
                    worksheet.Cells[currentRow, 1].Value = index;
                    worksheet.Cells[currentRow, 2].Value = Date.ToString("dd/MM/yy");
                    worksheet.Cells[currentRow, 3].Value = restaurantBooking.Agency != null ? restaurantBooking.Agency.TradingName : "";
                    worksheet.Cells[currentRow, 4].Value = restaurantBooking.PartOfDayString;
                    worksheet.Cells[currentRow, 5].Value = GetNumberOfPaxString(restaurantBooking);
                    worksheet.Cells[currentRow, 6].Value = GetFOC(restaurantBooking);
                    worksheet.Cells[currentRow, 7].Value = GetCostPerPerson(restaurantBooking);
                    worksheet.Cells[currentRow, 8].Value = restaurantBooking.TotalPriceOfSet;
                    worksheet.Cells[currentRow, 9].Value = GetCommission(restaurantBooking);
                    worksheet.Cells[currentRow, 10].Value = GetServiceOutside(restaurantBooking);
                    worksheet.Cells[currentRow, 11].Value = restaurantBooking.ActuallyCollected;
                    worksheet.Cells[currentRow, 12].Value = GetServiceOutsideDetail(restaurantBooking);
                    worksheet.Cells[currentRow, 13].Value = restaurantBooking.StatusOfPayment;
                    totalSet += restaurantBooking.TotalSet;
                    totalFOC += restaurantBooking.TotalFOC;
                    totalOfTotalCommissionAmount += restaurantBooking.TotalCommissionAmount;
                    totalOfTotalServiceOutsidePrice += restaurantBooking.TotalServiceOutsidePrice;
                    totalActuallyCollected += restaurantBooking.ActuallyCollected;
                    totalOfTotalPriceOfSet += restaurantBooking.TotalPriceOfSet;
                    index++;
                    currentRow++;
                }
                worksheet.Cells[currentRow, 5].Value = totalSet.ToString();
                worksheet.Cells[currentRow, 6].Value = totalFOC.ToString();
                worksheet.Cells[currentRow, 8].Value = totalOfTotalPriceOfSet;
                worksheet.Cells[currentRow, 9].Value = totalOfTotalCommissionAmount;
                worksheet.Cells[currentRow, 10].Value = totalOfTotalServiceOutsidePrice;
                worksheet.Cells[currentRow, 11].Value = totalActuallyCollected;
                currentRow += 2;
                var listCancelledAndChangeDateBooking = ListCancelledAndChangeDateBooking.OrderBy(x => x.PartOfDay).ThenBy(x => x.Time);
                index = 1;
                foreach (var restaurantBooking in listCancelledAndChangeDateBooking)
                {
                    worksheet.InsertRow(currentRow, 1, rowFrom);
                    worksheet.Cells[currentRow, 1].Value = index;
                    worksheet.Cells[currentRow, 2].Value = Date.ToString("dd/MM/yy");
                    worksheet.Cells[currentRow, 3].Value = restaurantBooking.Agency != null ? restaurantBooking.Agency.TradingName : "";
                    worksheet.Cells[currentRow, 4].Value = restaurantBooking.PartOfDayString;
                    worksheet.Cells[currentRow, 5].Value = GetNumberOfPaxString(restaurantBooking);
                    worksheet.Cells[currentRow, 6].Value = GetFOC(restaurantBooking);
                    worksheet.Cells[currentRow, 7].Value = GetCostPerPerson(restaurantBooking);
                    worksheet.Cells[currentRow, 8].Value = restaurantBooking.TotalPriceOfSet;
                    worksheet.Cells[currentRow, 9].Value = GetCommission(restaurantBooking);
                    worksheet.Cells[currentRow, 10].Value = GetServiceOutside(restaurantBooking);
                    worksheet.Cells[currentRow, 11].Value = restaurantBooking.ActuallyCollected;
                    worksheet.Cells[currentRow, 12].Value = GetCancelledOrChangeDateInformation(restaurantBooking).Replace("<br/>", "\r\n");
                    index++;
                    currentRow++;
                }
                worksheet.DeleteRow(rowFrom);
                excelPackage.SaveAs(mem);
            }
            string strFileName = String.Format("DoanhThuNgay{0}.xlsx", Date.ToString("dd_MM_yyyy"));
            Response.Clear();
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "atachement; filename=" + strFileName);
            mem.Position = 0;
            byte[] buffer = mem.ToArray();
            Response.BinaryWrite(buffer);
            Response.End();
        }
        public string GetServiceOutside(RestaurantBooking restaurantBooking)
        {
            var serviceOutsideString = "";
            if (restaurantBooking.ListServiceOutside.Count <= 0)
            {
                return "0";
            }
            foreach (var serviceOutside in restaurantBooking.ListServiceOutside)
            {
                serviceOutsideString += serviceOutside.TotalPrice.ToString("#,##0.##") + "\r\n";
            }
            return serviceOutsideString;
        }

        public string GetServiceOutsideDetail(RestaurantBooking restaurantBooking)
        {
            var serviceOutsideDetailString = "";
            foreach (var serviceOutside in restaurantBooking.ListServiceOutside)
            {
                foreach (var serviceOutsideDetail in serviceOutside.ListServiceOutsideDetail)
                {
                    serviceOutsideDetailString += String.Format("{0} : {1} x {2} = {3}", serviceOutsideDetail.Name,
                        serviceOutsideDetail.UnitPrice.ToString("#,##0.##"), serviceOutsideDetail.Quantity, serviceOutsideDetail.TotalPrice.ToString("#,##0.##")) + "\r\n";
                }
            }
            if (serviceOutsideDetailString.EndsWith("\r\n"))
            {
                serviceOutsideDetailString = serviceOutsideDetailString.Substring(0, serviceOutsideDetailString.LastIndexOf("\r\n"));
            }
            return serviceOutsideDetailString;
        }

        public string GetCommission(RestaurantBooking restaurantBooking)
        {
            var commissionString = "";
            if (restaurantBooking.ListCommission.Count <= 0)
            {
                return "0";
            }
            else
            {
                foreach (var commission in restaurantBooking.ListCommission)
                {
                    commissionString += commission.Amount.ToString("#,##0.##") + "\r\n";
                }
            }
            if (commissionString.EndsWith("\r\n"))
            {
                commissionString = commissionString.Substring(0, commissionString.LastIndexOf("\r\n"));
            }
            return commissionString;
        }

        public string GetCostPerPerson(RestaurantBooking restaurantBooking)
        {
            var costPerPersonString = "";
            if (restaurantBooking.CostPerPersonAdult != 0.0)
            {
                costPerPersonString += restaurantBooking.CostPerPersonAdult.ToString("#,##0.##") + "\r\n";
            }
            if (restaurantBooking.CostPerPersonChild > 0)
            {
                costPerPersonString += restaurantBooking.CostPerPersonChild.ToString("#,##0.##") + "\r\n";
            }
            if (restaurantBooking.CostPerPersonBaby > 0)
            {
                costPerPersonString += restaurantBooking.CostPerPersonBaby.ToString("#,##0.##") + "\r\n";
            }
            if (costPerPersonString.EndsWith("\r\n"))
            {
                costPerPersonString = costPerPersonString.Substring(0, costPerPersonString.LastIndexOf("\r\n"));
            }
            return costPerPersonString;
        }

        public string GetFOC(RestaurantBooking restaurantBooking)
        {
            var FOCString = "";
            if (restaurantBooking.NumberOfDiscountedPaxAdult > 0)
            {
                FOCString += restaurantBooking.NumberOfDiscountedPaxAdult + "\r\n";
            }
            if (restaurantBooking.NumberOfDiscountedPaxChild > 0)
            {
                FOCString += restaurantBooking.NumberOfDiscountedPaxChild + "\r\n";
            }
            if (restaurantBooking.NumberOfDiscountedPaxBaby > 0)
            {
                FOCString += restaurantBooking.NumberOfDiscountedPaxBaby + "\r\n";
            }
            if (FOCString.EndsWith("\r\n"))
            {
                FOCString = FOCString.Substring(0, FOCString.LastIndexOf("\r\n"));
            }
            return FOCString;
        }
        public string GetNumberOfPaxString(RestaurantBooking restaurantBooking)
        {
            var numberOfPaxString = "";
            if (restaurantBooking.NumberOfPaxAdult > 0)
            {
                numberOfPaxString += restaurantBooking.NumberOfPaxAdult + "\r\n";
            }
            if (restaurantBooking.NumberOfPaxChild > 0)
            {
                numberOfPaxString += restaurantBooking.NumberOfPaxChild + "\r\n";
            }
            if (restaurantBooking.NumberOfPaxBaby > 0)
            {
                numberOfPaxString += restaurantBooking.NumberOfPaxBaby + "\r\n";
            }
            if (numberOfPaxString.EndsWith("\r\n"))
            {
                numberOfPaxString = numberOfPaxString.Substring(0, numberOfPaxString.LastIndexOf("\r\n"));
            }
            return numberOfPaxString;
        }
        public string GetServiceOutsideDetail(ServiceOutside serviceOutside)
        {
            var serviceOutsideDetailString = " ";
            foreach (var serviceOutsideDetail in serviceOutside.ListServiceOutsideDetail)
            {
                serviceOutsideDetailString += String.Format("{0} : {1} x {2} = {3}", serviceOutsideDetail.Name, serviceOutsideDetail.UnitPrice.ToString("#,##0.##"), serviceOutsideDetail.Quantity, serviceOutsideDetail.TotalPrice.ToString("#,##0.##")) + "<br/>";
            }
            return serviceOutsideDetailString;
        }

        protected void btnExportForKitchen_OnClick(object sender, EventArgs e)
        {
            if (!AllowExportForKitchen)
            {
                ShowErrors("Bạn không có quyền xuất lệnh bếp");
                return;
            }
            ExcelFile excelFile = ExcelFile.Load(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/lenh_cho_bep.xls"));
            GemBox.Spreadsheet.ExcelWorksheet sheet = excelFile.Worksheets[0];
            // Dòng dữ liệu đầu tiên
            const int firstrow = 6;
            int crow = firstrow;
            var list = RestaurantBookingByDateBLL.RestaurantBookingGetByKitchen(Date).OrderBy(x => x.PartOfDay).ThenBy(x => x.Time);

            sheet.Rows.InsertCopy(crow, list.Count(), sheet.Rows[firstrow]);

            sheet.Cells["F4"].Value = Date.ToString("dd/MM/yyyy");
            foreach (RestaurantBooking restaurantBooking in list)
            {
                sheet.Cells[crow, 0].Value = crow - firstrow + 1;
                sheet.Cells[crow, 1].Value = restaurantBooking.Code;
                if (restaurantBooking.Agency != null) sheet.Cells[crow, 2].Value = restaurantBooking.Agency.TradingName;
                var time = "";
                if (restaurantBooking.PartOfDay == 1) time = "Sáng";
                if (restaurantBooking.PartOfDay == 2) time = "Trưa";
                if (restaurantBooking.PartOfDay == 3) time = "Tối";
                sheet.Cells[crow, 3].Value = time;
                sheet.Cells[crow, 4].Value = restaurantBooking.NumberOfPaxAdult;
                sheet.Cells[crow, 5].Value = restaurantBooking.NumberOfPaxChild;
                sheet.Cells[crow, 6].Value = restaurantBooking.NumberOfPaxBaby;
                sheet.Cells[crow, 7].Value = restaurantBooking.CostPerPersonAdult.ToString("#,##0.##");
                sheet.Cells[crow, 8].Value = restaurantBooking.CostPerPersonChild.ToString("#,##0.##");
                //                sheet.Cells[crow, 9].Value = restaurantBooking.CostPerPersonBaby.ToString("#,##0.##");
                sheet.Cells[crow, 9].Value = (restaurantBooking.SpecialRequest + Environment.NewLine + restaurantBooking.MenuDetail).Trim();

                crow++;
            }
            excelFile.Save(Response, string.Format("lenh_bep_{0:dd_MM_yyy}.xls", Date));
        }

        public string GetLinkPartOfDayFilter(int partOfDay)
        {
            if (partOfDay == 0)
            {
                return "RestaurantBookingByDate.aspx" + QueryStringBuildByCriterion();
            }
            return "RestaurantBookingByDate.aspx" + QueryStringBuildByCriterion(partOfDay);
        }

        public string GetTotalBookingAndPaxByTime(int partOfDay)
        {
            var listRestaurantBooking = RestaurantBookingByDateBLL.RestaurantBookingGetAllByCriterion(Date, partOfDay, StatusEnum.Approved, StatusEnum.Pending);
            var totalPax = 0;
            foreach (var restaurantBooking in listRestaurantBooking)
            {
                totalPax += restaurantBooking.TotalSet;
            }
            return String.Format("{0} đoàn, {1} khách", listRestaurantBooking.Count, totalPax);
        }

        protected void rptBooking_OnItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var restaurantBookingId = -1;
            try
            {
                restaurantBookingId = Convert.ToInt32(e.CommandArgument);
            }
            catch { }
            var restaurantBooking = RestaurantBookingByDateBLL.RestaurantBookingGetById(restaurantBookingId);
            if (e.CommandName == "ExportByAgency")
            {
                ExcelFile excelFile = ExcelFile.Load(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/lenh_cho_bep.xls"));
                GemBox.Spreadsheet.ExcelWorksheet sheet = excelFile.Worksheets[0];
                // Dòng dữ liệu đầu tiên
                const int firstrow = 6;
                int crow = firstrow;
                sheet.Cells["F4"].Value = Date.ToString("dd/MM/yyyy");
                sheet.Cells[crow, 0].Value = crow - firstrow + 1;
                sheet.Cells[crow, 1].Value = restaurantBooking.Code;
                if (restaurantBooking.Agency != null)
                    sheet.Cells[crow, 2].Value = restaurantBooking.Agency.TradingName;
                var time = "";
                if (restaurantBooking.PartOfDay == 1) time = "Sáng";
                if (restaurantBooking.PartOfDay == 2) time = "Trưa";
                if (restaurantBooking.PartOfDay == 3) time = "Tối";
                sheet.Cells[crow, 3].Value = time;
                sheet.Cells[crow, 4].Value = restaurantBooking.NumberOfPaxAdult;
                sheet.Cells[crow, 5].Value = restaurantBooking.NumberOfPaxChild;
                sheet.Cells[crow, 6].Value = restaurantBooking.NumberOfPaxBaby;
                sheet.Cells[crow, 7].Value = restaurantBooking.CostPerPersonAdult.ToString("#,##0.##");
                sheet.Cells[crow, 8].Value = restaurantBooking.CostPerPersonChild.ToString("#,##0.##");
                //                sheet.Cells[crow, 9].Value = restaurantBooking.CostPerPersonBaby.ToString("#,##0.##");
                sheet.Cells[crow, 9].Value = (restaurantBooking.SpecialRequest + Environment.NewLine +
                                              restaurantBooking.MenuDetail).Trim();
                excelFile.Save(Response, string.Format("lenh_bep_{0:dd_MM_yyy}.xls", Date));
            }
            if (e.CommandName == "Lock")
            {
                restaurantBooking.LockStatus = LockStatusEnum.Locked;
                RestaurantBookingByDateBLL.RestaurantBookingSaveOrUpdate(restaurantBooking);
            }
            if (e.CommandName == "Unlock")
            {
                if (restaurantBooking.OnlyUnlockByRole != OnlyUnlockByRoleEnum.Null)
                {
                    if (!PermissionBLL.UserCheckRole(CurrentUser.Id, (int)OnlyUnlockByRoleEnum.Administrator))
                    {
                        ShowErrors("Booking này hiện chỉ có administrator có thể mở khóa!");
                        return;
                    }
                }
                restaurantBooking.LockStatus = LockStatusEnum.Unlocked;
                restaurantBooking.LastUnlockedTime = DateTime.Now;
                RestaurantBookingByDateBLL.RestaurantBookingSaveOrUpdate(restaurantBooking);
            }
            DisplayData();
        }

        protected void rptCancelledAndChangeDateBooking_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var btnLock = e.Item.FindControl("btnLock") as LinkButton;
                var btnUnlock = e.Item.FindControl("btnUnlock") as LinkButton;
                if (!PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowLockBooking))
                {
                    btnLock.Visible = false;
                }
                if (!PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowUnlockBooking))
                {
                    btnUnlock.Visible = false;
                }
                var restaurantBooking = (RestaurantBooking)e.Item.DataItem;
                if (restaurantBooking.OnlyUnlockByRole != OnlyUnlockByRoleEnum.Null)
                {
                    if (!PermissionBLL.UserCheckRole(CurrentUser.Id, (int)OnlyUnlockByRoleEnum.Administrator))
                    { btnUnlock.Visible = false; }
                }
                var rptServiceOutside = e.Item.FindControl("rptServiceOutside") as Repeater;
                rptServiceOutside.DataSource = ((IList<ServiceOutside>)DataBinder.Eval(e.Item.DataItem, "ListServiceOutside"));
                rptServiceOutside.DataBind();
            }
        }
        public string GetCancelledOrChangeDateInformation(RestaurantBooking restaurantBooking)
        {
            var output = "";
            var restaurantBookingCancelledOrChangeDate = restaurantBooking.CancelledOrChangeDate(Date);
            if (restaurantBookingCancelledOrChangeDate == "Cancel")
            {
                output += "Hủy" + "<br/>" + "Lý do hủy: "+ restaurantBooking.Reason + "<br/>";
            }
            if (restaurantBookingCancelledOrChangeDate == "ChangeDate")
            {
                output += "Chuyển sang ngày " + (restaurantBooking.Date.HasValue ? restaurantBooking.Date.Value.ToString("dd/MM/yyyy") : "") + "<br/>";
            }
            if (restaurantBookingCancelledOrChangeDate == "CancelChangeDate")
            {
                output += "Hủy" + "<br/>" + "Chuyển sang ngày " + (restaurantBooking.Date.HasValue ? restaurantBooking.Date.Value.ToString("dd/MM/yyyy") : "") + "<br/>";
            }
            output += "Chỉnh sửa lần cuối bởi " + (restaurantBooking.LastEditedBy != null ? restaurantBooking.LastEditedBy.FullName : "") + " vào lúc " + (restaurantBooking.LastEditedDate.HasValue ? restaurantBooking.LastEditedDate.Value.ToString("dd/MM/yyyy HH:mm:ss") : "");
            return output;
        }
        public String GetBookerAndGuides(RestaurantBooking restaurantBooking)
        {
            var output = "";
            if (restaurantBooking.Booker != null)
            {
                output += "<strong>Booker : </strong><br/>";
                if (!String.IsNullOrEmpty(restaurantBooking.Booker.Phone))
                {
                    output += restaurantBooking.Booker.Name + "<br/>" + restaurantBooking.Booker.Phone + "<br/>";
                }
                else
                {
                    output += restaurantBooking.Booker.Name;
                }
            }
            if (restaurantBooking.ListGuide != null)
            {
                if (restaurantBooking.ListGuide.Count > 0)
                {
                    output += "<strong>HDV : </strong><br/>";
                    foreach (var guide in restaurantBooking.ListGuide)
                    {
                        if (!String.IsNullOrEmpty(guide.Phone))
                        {
                            output += guide.Name + "<br/>" + guide.Phone + "<br/>";
                        }
                        else
                        {
                            output += guide.Name;
                        }
                    }
                    if (output.EndsWith("<br/>"))
                    {
                        output = output.Substring(0, output.LastIndexOf("<br/>"));
                    }

                }
            }
            return output;
        }
        public int GetTotalPaxByPartOfDay(int partOfDay)
        {
            var listRestaurantBooking = RestaurantBookingByDateBLL.RestaurantBookingGetAllByCriterion(Date, partOfDay, StatusEnum.Approved, StatusEnum.Pending);
            var totalPax = 0;
            foreach (var restaurantBooking in listRestaurantBooking)
            {
                totalPax += restaurantBooking.TotalSet;
            }
            return totalPax;
        }

        protected void btnLockDate_Click(object sender, EventArgs e)
        {
            if (!PermissionBLL.UserCheckRole(CurrentUser.Id, (int)Roles.Administrator))
            {
                ShowErrors("Bạn không phải administrator không thể sử dụng chức năng này");
                return;
            }
            foreach (var restaurantBooking in ListRestaurantBooking)
            {
                restaurantBooking.LockStatus = LockStatusEnum.Locked;
                restaurantBooking.OnlyUnlockByRole = OnlyUnlockByRoleEnum.Administrator;
                RestaurantBookingByDateBLL.RestaurantBookingSaveOrUpdate(restaurantBooking);
            }
            foreach (var restaurantBooking in ListCancelledAndChangeDateBooking)
            {
                restaurantBooking.LockStatus = LockStatusEnum.Locked;
                restaurantBooking.OnlyUnlockByRole = OnlyUnlockByRoleEnum.Administrator;
                RestaurantBookingByDateBLL.RestaurantBookingSaveOrUpdate(restaurantBooking);
            }
            ShowSuccess("Đã khóa tất cả booking trong ngày " + Date.ToString("dd/MM/yyyy"));
            DisplayData();
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

        protected void rptCancelledAndChangeDateBooking_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var restaurantBookingId = -1;
            try
            {
                restaurantBookingId = Convert.ToInt32(e.CommandArgument);
            }
            catch { }
            var restaurantBooking = RestaurantBookingByDateBLL.RestaurantBookingGetById(restaurantBookingId);
            if (e.CommandName == "Lock")
            {
                restaurantBooking.LockStatus = LockStatusEnum.Locked;
                RestaurantBookingByDateBLL.RestaurantBookingSaveOrUpdate(restaurantBooking);
            }
            if (e.CommandName == "Unlock")
            {
                if (restaurantBooking.OnlyUnlockByRole != OnlyUnlockByRoleEnum.Null)
                {
                    if (!PermissionBLL.UserCheckRole(CurrentUser.Id, (int)OnlyUnlockByRoleEnum.Administrator))
                    {
                        ShowErrors("Booking này hiện chỉ có administrator có thể mở khóa!");
                        return;
                    }
                }
                restaurantBooking.LockStatus = LockStatusEnum.Unlocked;
                restaurantBooking.LastUnlockedTime = DateTime.Now;
                RestaurantBookingByDateBLL.RestaurantBookingSaveOrUpdate(restaurantBooking);
            }
            DisplayData();
        }
    }
}