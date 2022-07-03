using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Domain
{
    public class Commission
    {
        public virtual int Id { get; set; }
        public virtual string PayFor { get; set; }
        public virtual double Amount { get; set; }
        public virtual RestaurantBooking RestaurantBooking { get; set; }
        public virtual string PaymentVoucher { get; set; }
        public virtual bool Transfer { get; set; }
    }
}