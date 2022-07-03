using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.Modules.OrientalSails.Web.UI;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class AgencyInvoiceInfo : SailsAdminBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var agency = Module.AgencyGetById(Convert.ToInt32(Request["id"]));
            if (agency != null)
            {
                litAgency.Text = agency.TradingName;
                litInvoice.Text = agency.Invoice;
            }
        }
    }
}