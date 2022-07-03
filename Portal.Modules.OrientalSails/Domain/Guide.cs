using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Domain
{
    public class Guide
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Phone { get; set; }
        public virtual RestaurantBooking RestaurantBooking { get; set; }
    }
}