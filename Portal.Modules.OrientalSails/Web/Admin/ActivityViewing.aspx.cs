using CMS.Core.Domain;
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class ActivityViewing : System.Web.UI.Page
    {
        private ActivityViewingBLL activityViewingBLL;
        public ActivityViewingBLL ActivityViewingBLL
        {
            get
            {
                if (activityViewingBLL == null)
                {
                    activityViewingBLL = new ActivityViewingBLL();
                }
                return activityViewingBLL;
            }
        }
        public new User User
        {
            get
            {
                int userId = -1;
                User user = null;
                try
                {
                    userId = Int32.Parse(Request.QueryString["ui"]);
                    user = ActivityViewingBLL.UserGetById(userId);
                }
                catch { }
                if (user == null)
                {
                    Response.Redirect("404.aspx");
                }
                return user;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.Title = "Theo dõi hoạt động";
            if (!IsPostBack)
            {
                var from = DateTime.Now.AddDays(-10);
                try
                {
                    from = DateTime.ParseExact(Request.QueryString["f"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch { }
                txtFrom.Text = from.ToString("dd/MM/yyyy");
                var to = DateTime.Now;
                try
                {
                    to = DateTime.ParseExact(Request.QueryString["t"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch { }
                txtTo.Text = to.ToString("dd/MM/yyyy");
                var dayValid = to.Subtract(from).Days;
                if (dayValid > 62)
                {
                    ShowErrors("Khoảng cách ngày lớn hơn 62 ngày không thể hiển thị");
                    return;
                }
                rptActivity.DataSource = ActivityViewingBLL.ActivityLoggingGetAllByCriterion(from, to, User).Future().ToList();
                rptActivity.DataBind();
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (activityViewingBLL != null)
            {
                activityViewingBLL.Dispose();
                activityViewingBLL = null;
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

        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            Response.Redirect("ActivityViewing.aspx" + QueryStringBuildByCriterion(User));
        }
        public string QueryStringBuildByCriterion(User user)
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
            if (user != null)
            {
                nvcQueryString.Add("ui", user.Id.ToString());
            }
            var criterions = (from key in nvcQueryString.AllKeys
                              from value in nvcQueryString.GetValues(key)
                              select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value))).ToArray();

            return "?" + string.Join("&", criterions);
        }
        public string GetLink(ActivityLogging activityLogging)
        {
            var output = "";
            var splitArray = activityLogging.ObjectId.Split(new char[] { ':' });
            if (splitArray.Count() > 0)
            {
                var objectType = splitArray[0];
                var objectId = -1;
                try
                {
                    objectId = Int32.Parse(splitArray[1]);
                }
                catch { }
                if (objectType == "BookingId")
                {
                    var bookingId = objectId;
                    var booking = ActivityViewingBLL.RestaurantBookingGetById(bookingId);
                    if (booking != null)
                    {
                        output = String.Format("<a href = 'BookingViewing.aspx?NodeId=1&SectionId=15&bi={0}'>{1}</a>", bookingId, booking.Code);
                    }
                }
                if (objectType == "AgencyId")
                {
                    var agencyId = objectId;
                    var agency = ActivityViewingBLL.AgencyGetById(agencyId);
                    if (agency != null)
                    {
                        output = String.Format("<a href = 'AgencyView.aspx?NodeId=1&SectionId=15&AgencyId={0}'>{1}</a>", agencyId, String.IsNullOrEmpty(agency.TradingName) ? (String.IsNullOrEmpty(agency.Name) ? agency.Id.ToString() : agency.Name) : agency.TradingName);
                    }
                }
                if (objectType == "MenuId")
                {
                    var menuId = objectId;
                    var menu = ActivityViewingBLL.MenuGetById(menuId);
                    if (menu != null)
                    {
                        output = String.Format("<a href = 'MenuViewing.aspx?NodeId=1&SectionId=15&mi={0}'>{1}</a>", menuId, menu.Name);
                    }
                }
            }
            return output;
        }
    }
}