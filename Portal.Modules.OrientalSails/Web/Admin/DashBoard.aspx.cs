using Portal.Modules.OrientalSails.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.BusinessLogic.Share;
using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Enums;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class DashBoard : System.Web.UI.Page
    {
        private DashBoardBLL dashBoardBLL;
        private PermissionBLL permissionBLL;
        private UserBLL userBLL;
        public DashBoardBLL DashBoardBLL
        {
            get
            {
                if (dashBoardBLL == null)
                {
                    dashBoardBLL = new DashBoardBLL();
                }
                return dashBoardBLL;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.Title = "Bảng tổng hợp";
            var allowAccess = PermissionBLL.UserCheckPermission(CurrentUser.Id, (int)PermissionEnum.AllowAccessDashboardPage);
            if (!allowAccess)
            {
                ShowErrors("Bạn không có quyền truy cập vào trang này");
                plhAdminContent.Visible = false;
                return;
            }
            if (!IsPostBack)
            {       
                DisplayData();
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (dashBoardBLL != null)
            {
                dashBoardBLL.Dispose();
                dashBoardBLL = null;
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

        public void DisplayData()
        {
            BindDataBookingToDay();
            BindDataBookingNext7Day();
            BindDataReportMonth();
            BindDataTop10Agency();
            BindDataNewRestaurantBooking();
            BindDataNewAgency();
        }

        public void BindDataNewAgency()
        {
            var today = DateTime.Now.Date;
            var endOfToday = DateTime.Now.Date.Add(new TimeSpan(23, 59, 59));
            var listAgency = DashBoardBLL.AgencyGetAllByCreatedDate(today, endOfToday).Future().ToList();
            rptNewAgency.DataSource = listAgency;
            rptNewAgency.DataBind();
        }

        public void BindDataNewRestaurantBooking()
        {
            var today = DateTime.Now.Date;
            var endOfToDay = DateTime.Now.Date.Add(new TimeSpan(23, 59, 59));
            var listRestaurantBooking = DashBoardBLL.RestaurantBookingGetAllByCreatedDate(today, endOfToDay).Future().ToList();
            rptNewRestaurantBooking.DataSource = listRestaurantBooking;
            rptNewRestaurantBooking.DataBind();
        }

        public void BindDataTop10Agency()
        {
            var from = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, 1);
            var to = from.AddMonths(1).AddDays(-1);
            var listRestaurantBooking = DashBoardBLL.RestaurantBookingGetAllByDateRange(from, to).Future().ToList();
            var listTop10Agency = listRestaurantBooking.GroupBy(x => x.Agency).Select(x => x.Select(
                y => new
                {
                    Agency = x.Key,
                    TotalOfTotalPrice = x.Select(z => z.TotalPrice).Sum(),
                    TotalOfTotalSet = x.Select(z => z.TotalSet).Sum()
                })).Select(x => x.FirstOrDefault()).OrderByDescending(x => x.TotalOfTotalPrice).ThenByDescending(x => x.TotalOfTotalSet).Take(10);
            rptTop10Agency.DataSource = listTop10Agency;
            rptTop10Agency.DataBind();
        }

        public void BindDataReportMonth()
        {
            var from = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, 1);
            var to = from.AddMonths(1).AddDays(-1);
            var listRestaurantBooking = DashBoardBLL.RestaurantBookingGetAllByDateRange(from, to).Future().ToList();
            lblTotalOfTotalPrice.Text = listRestaurantBooking.Select(x => x.TotalPrice).Sum().ToString("#,##0.##") + "₫";
            lblMonthlyVAT.Text = listRestaurantBooking.Where(x => x.VAT == true).Select(x => x.TotalPriceOfSet).Sum().ToString("#,##0.##") + "₫";
            lblTotalPaid.Text = listRestaurantBooking.Select(x => x.TotalPaid).Sum().ToString("#,##0.##") + "₫";
            lblReceivable.Text = listRestaurantBooking.Select(x => x.Receivable).Sum().ToString("#,##0.##") + "₫";
            var outstandingDebtTo = DateTime.Now.Date;
            var listRestaurantBookingOutstandingDebt = DashBoardBLL.RestaurantBookingGetAllByOutstandingDebt(outstandingDebtTo).Future().ToList();
            lblOutstandingDebt.Text = listRestaurantBookingOutstandingDebt.Select(x => x.Receivable).Sum().ToString("#,##0.##") + "₫";
        }

        public void BindDataBookingToDay()
        {
            var today = DateTime.Now.Date;
            var listGroupedBookingByParOfDay = DashBoardBLL.RestaurantBookingGetAllByDate(today)
                .Future()
                .GroupBy(x => x.PartOfDayString);
            var lookupGroupedBookingByPartOfDay = listGroupedBookingByParOfDay.ToLookup(x => x.Key);
            var dictionaryGroupedBookingByPartOfDay = listGroupedBookingByParOfDay.ToDictionary(x => x.Key, x => x.ToList());
            if (!lookupGroupedBookingByPartOfDay["Sáng"].Any())
            {
                dictionaryGroupedBookingByPartOfDay.Add("Sáng", new List<RestaurantBooking>());
            }
            if (!lookupGroupedBookingByPartOfDay["Trưa"].Any())
            {
                dictionaryGroupedBookingByPartOfDay.Add("Trưa", new List<RestaurantBooking>());
            }
            if (!lookupGroupedBookingByPartOfDay["Tối"].Any())
            {
                dictionaryGroupedBookingByPartOfDay.Add("Tối", new List<RestaurantBooking>());
            }
            rptBookingByPartOfDay.DataSource = dictionaryGroupedBookingByPartOfDay.OrderByDescending(x => x.Key);
            rptBookingByPartOfDay.DataBind();
        }
        public void BindDataBookingNext7Day()
        {
            var today = DateTime.Now.Date;
            var next7Day = today.AddDays(7);
            var listGroupedRestaurantBookingByDate = DashBoardBLL.RestaurantBookingGetAllByDateRange(today, next7Day)
                .Future()
                .GroupBy(x => x.Date);
            var lookupGroupedRestaurantBookingByDate = listGroupedRestaurantBookingByDate.ToLookup(x => x.Key);
            var dictionaryGroupedRestaurantBookingByDate = listGroupedRestaurantBookingByDate.ToDictionary(x => x.Key, x => x.ToList());
            for (var dt = today; dt <= next7Day; dt = dt.AddDays(1))
            {
                if (!lookupGroupedRestaurantBookingByDate[dt].Any())
                {
                    dictionaryGroupedRestaurantBookingByDate.Add(dt, new List<RestaurantBooking>());
                }
            }
            rptBookingNext7Day.DataSource = dictionaryGroupedRestaurantBookingByDate.OrderBy(x => x.Key);
            rptBookingNext7Day.DataBind();
        }

        public string GetDataPartOfDayColumn(KeyValuePair<string, List<RestaurantBooking>> dataItem)
        {
            var output = "";
            output += dataItem.Key;
            var listRestaurantBooking = dataItem.Value;
            output += "(" + listRestaurantBooking.Count() + " đoàn, ";
            var totalOfTotalSet = 0;
            foreach (var restaurantBooking in listRestaurantBooking)
            {
                totalOfTotalSet += restaurantBooking.TotalSet;
            }
            output += totalOfTotalSet + " khách)";
            return output;
        }

        int totalOfTotalSet = 0;
        double totalOfTotalPrice = 0.0;
        protected void rptBookingByPartOfDay_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var output = "";
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var partOfDayString = ((KeyValuePair<string, List<RestaurantBooking>>)e.Item.DataItem).Key;
                var listRestaurantBooking = ((KeyValuePair<string, List<RestaurantBooking>>)e.Item.DataItem).Value;
                var rowspan = listRestaurantBooking.Count();
                var firstBooking = true;
                var dataOfPartOfDayColumn = GetDataPartOfDayColumn((KeyValuePair<string, List<RestaurantBooking>>)e.Item.DataItem);
                if (listRestaurantBooking.Count <= 0)
                {
                    output += String.Format("<tr>" +
                                                "<td>{0}</td>" +
                                                "<td></td>" +
                                                "<td></td>" +
                                                "<td></td>" +
                                                "<td></td>" +
                                            "</tr>", dataOfPartOfDayColumn);
                }
                foreach (var restaurantBooking in listRestaurantBooking)
                {
                    var dataOfAgencyColumn = "";
                    try
                    {
                        dataOfAgencyColumn = restaurantBooking.Agency.TradingName;
                    }
                    catch { }
                    var dataOfCodeColumn = restaurantBooking.Code;
                    var dataOfTotalSetColumn = restaurantBooking.TotalSet;
                    var dataOfTotalPriceColumn = restaurantBooking.TotalPrice;
                    if (firstBooking)
                    {
                        output += String.Format("<tr>" +
                                                    "<td rowspan={0}>{1}</td>" +
                                                    "<td><a href='BookingViewing.aspx?NodeId=1&SectionId=15&bi={6}'>{2}</a></td>" +
                                                    "<td style='text-align:left !important'>{3}</td>" +
                                                    "<td>{4}</td><td style='text-align:right !important'>{5}</td>" +
                                                "</tr>", rowspan, dataOfPartOfDayColumn, dataOfCodeColumn, dataOfAgencyColumn, dataOfTotalSetColumn.ToString("#,##0.##"), dataOfTotalPriceColumn.ToString("#,##0.##") + "₫", restaurantBooking.Id);
                    }
                    else
                    {
                        output += String.Format("<tr>" +
                                                    "<td><a href='BookingViewing.aspx?NodeId=1&SectionId=15&bi={4}'>{0}</a></td>" +
                                                    "<td style='text-align:left !important'>{1}</td>" +
                                                    "<td>{2}</td><td style='text-align:right !important'>{3}</td>" +
                                                "</tr>", dataOfCodeColumn, dataOfAgencyColumn, dataOfTotalSetColumn.ToString("#,##0.##"), dataOfTotalPriceColumn.ToString("#,##0.##") + "₫", restaurantBooking.Id);
                    }
                    firstBooking = false;
                    totalOfTotalPrice += restaurantBooking.TotalPrice;
                    totalOfTotalSet += restaurantBooking.TotalSet;
                }
                var plhTableRow = (PlaceHolder)e.Item.FindControl("plhTableRow");
                plhTableRow.Controls.Add(new LiteralControl(output));
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                var lblTotalOfTotalSet = (Label)e.Item.FindControl("lblTotalOfTotalSet");
                var lblTotalOfTotalPrice = (Label)e.Item.FindControl("lblTotalOfTotalPrice");
                lblTotalOfTotalSet.Text = totalOfTotalSet.ToString("#,##0.##");
                lblTotalOfTotalPrice.Text = totalOfTotalPrice.ToString("#,##0.##") + "₫";
            }
        }

        protected void rptBookingNext7Day_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var totalOfTotalSet = 0;
                var totalOfTotalPrice = 0.0;
                var lblTotalOfTotalSet = (Label)e.Item.FindControl("lblTotalOfTotalSet");
                var lblTotalOfTotalPrice = (Label)e.Item.FindControl("lblTotalOfTotalPrice");
                var listRestaurantBooking = ((KeyValuePair<DateTime?, List<RestaurantBooking>>)e.Item.DataItem).Value;
                foreach (var restaurantBooking in listRestaurantBooking)
                {
                    totalOfTotalSet += restaurantBooking.TotalSet;
                    totalOfTotalPrice += restaurantBooking.TotalPrice;
                }
                lblTotalOfTotalPrice.Text = totalOfTotalPrice.ToString("#,##0.##") + "₫";
                lblTotalOfTotalSet.Text = totalOfTotalSet.ToString("#,##0.##");
            }
        }

        int total_TotalOfTotalSet = 0;
        double total_TotalOfTotalPrice = 0;
        protected void rptTop10Agency_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                total_TotalOfTotalSet += (int)DataBinder.Eval(e.Item.DataItem, "TotalOfTotalSet");
                total_TotalOfTotalPrice += (double)DataBinder.Eval(e.Item.DataItem, "TotalOfTotalPrice");
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                var lblTotal_TotalOfTotalSet = (Label)e.Item.FindControl("lblTotal_TotalOfTotalSet");
                var lblTotal_TotalOfTotalPrice = (Label)e.Item.FindControl("lblTotal_TotalOfTotalPrice");
                lblTotal_TotalOfTotalPrice.Text = total_TotalOfTotalPrice.ToString("#,##0.##") + "₫";
                lblTotal_TotalOfTotalSet.Text = total_TotalOfTotalSet.ToString("#,##0.##");
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
    }
}