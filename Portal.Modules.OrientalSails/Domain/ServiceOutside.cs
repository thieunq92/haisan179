using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Domain
{
    public class ServiceOutside
    {
        public virtual int Id { get; set; }
        public virtual string Service { get; set; }
        public virtual double UnitPrice { get; set; }
        public virtual int Quantity { get; set; }
        public virtual double TotalPrice { get; set; }
        public virtual RestaurantBooking RestaurantBooking { get; set; }
        public virtual bool VAT { get; set; }
        public virtual IList<ServiceOutsideDetail> ListServiceOutsideDetail { get; set; }
    }
}