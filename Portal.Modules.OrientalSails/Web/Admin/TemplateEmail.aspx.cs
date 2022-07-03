using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Modules.OrientalSails.BusinessLogic;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class TemplateEmail : SailsAdminBasePage
    {
        private BookingViewingBLL bookingViewingBLL;

        public BookingViewingBLL BookingViewingBLL
        {
            get
            {
                if (bookingViewingBLL == null)
                    bookingViewingBLL = new BookingViewingBLL();
                return bookingViewingBLL;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var booking = BookingViewingBLL.RestaurantBookingGetById(Convert.ToInt32(Request.QueryString["BookingId"]));
                StreamReader canReader = new StreamReader(Server.MapPath("/Modules/Sails/Admin/EmailTemplate/templateEmail.html"));
                string body = canReader.ReadToEnd();
                body = body.Replace("{CODE}", booking.Code);
                body = body.Replace("{NGAY}", booking.DateString);

                body = body.Replace("{THOIGIAN}", (booking.PartOfDay == 1
                    ? "Sáng"
                    : booking.PartOfDay == 2
                        ? "Trưa"
                        : booking.PartOfDay == 3
                            ? "Tối"
                            : "") + " " + booking.Time);
                body = body.Replace("{AGENCY}", booking.Agency.Name);
                if (booking.Booker != null) body = body.Replace("{NGUOIDAT}", booking.Booker.Name);
                else body = body.Replace("{NGUOIDAT}", "");
                var hdv = "";

                if (booking.ListGuide != null && booking.ListGuide.Count > 0)
                {
                    foreach (Guide guide in booking.ListGuide)
                    {
                        hdv += guide.Name + "  " + guide.Phone + Environment.NewLine;
                    }
                }
                body = body.Replace("{HDV}", hdv);

                body = body.Replace("{ADULT}", booking.NumberOfPaxAdult.ToString());
                body = body.Replace("{CHILD}", booking.NumberOfPaxChild.ToString());
                body = body.Replace("{BABY}", booking.NumberOfPaxBaby.ToString());
                body = body.Replace("{DGADULT}", booking.CostPerPersonAdult.ToString("#,##0.##"));
                body = body.Replace("{DGCHILD}", booking.CostPerPersonChild.ToString("#,##0.##"));
                body = body.Replace("{MENU}",
                    String.IsNullOrWhiteSpace(booking.MenuDetail)
                        ? "Chưa chốt menu"
                        : booking.MenuDetail.Replace("\n", "\n<br />"));
                    //.Replace("\r", "\r<br />"));
                body = body.Replace("{YCDB}", booking.SpecialRequest.Replace("\n", "\n<br />"));
                var dvn = "";
                if (booking.ListServiceOutside != null && booking.ListServiceOutside.Count > 0)
                {
                    StreamReader serviceOutsiteReader = new StreamReader(Server.MapPath("/Modules/Sails/Admin/EmailTemplate/serviceOutsite.txt"));
                    string serviceOutsite = serviceOutsiteReader.ReadToEnd();
                    var sro = "";
                    foreach (ServiceOutside outside in booking.ListServiceOutside)
                    {
                        sro += "<tr>";
                        sro += "<td style='width:90.9pt;padding:0in 5.4pt 0in 5.4pt'>" + outside.Service + "	</th>";
                        sro += "<td style='width:90.9pt;padding:0in 5.4pt 0in 5.4pt'>" + outside.UnitPrice.ToString("#,##0.##") + "</th>";
                        sro += "<td style='width:90.9pt;padding:0in 5.4pt 0in 5.4pt'>" + outside.Quantity + "	</th>";
                        sro += "<td style='width:90.9pt;padding:0in 5.4pt 0in 5.4pt'>" + outside.TotalPrice.ToString("#,##0.##") + "	</th>";
                        sro += "<td style='width:90.9pt;padding:0in 5.4pt 0in 5.4pt'>" + (outside.VAT ? "Có" : "Không") + "	</th>";
                        sro += "</tr>";
                    }
                    dvn = serviceOutsite.Replace("{DVNGOAI}", sro);
                }
                body = body.Replace("{DVNGOAI}", dvn);
                body = body.Replace("{TONGGIA}", booking.TotalPrice.ToString("#,##0.##"));
                var foc = "";
                if (booking.NumberOfDiscountedPaxAdult > 0 || booking.NumberOfDiscountedPaxChild > 0)
                {
                    StreamReader focReader = new StreamReader(Server.MapPath("/Modules/Sails/Admin/EmailTemplate/FOC.txt"));
                    string focToEnd = focReader.ReadToEnd();
                    focToEnd = focToEnd.Replace("{FOCADULT}", booking.NumberOfDiscountedPaxAdult.ToString());
                    foc = focToEnd.Replace("{FOCCHILD}", booking.NumberOfDiscountedPaxChild.ToString());
                }

                body = body.Replace("{FOC}", foc);

                body = body.Replace("{THUCTHU}", booking.ActuallyCollected.ToString("#,##0.##"));
                body = body.Replace("{THANHTOAN}", booking.Payment == 1 ? "Thanh toán ngay" : "Công nợ");
                var vat = "không bao gồm";
                if (booking.VAT)
                {
                    vat = "bao gồm";
                }
                body = body.Replace("{VAT}", vat);
                fckContent.Value = body;

            }

        }
    }
}