using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.DataTransferObject
{
    public class GuideDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public int restaurantBookingId { get; set; }
    }
}