using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.DataTransferObject
{
    public class CommissionDTO
    {
        public int id { get; set; }
        public string payFor { get; set; }
        public string amount { get; set; }
        public int restaurantBookingId { get; set; }
        public string paymentVoucher { get; set; }
        public bool transfer { get; set; }
    }
}