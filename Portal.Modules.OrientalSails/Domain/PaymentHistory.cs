using CMS.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Domain
{
    public class PaymentHistory
    {
        public virtual int Id { get; set; }
        public virtual Agency Payby { get; set; }
        public virtual double Amount { get; set; }
        public virtual User Createdby { get; set; }
        public virtual DateTime? Time { get; set; }
        public virtual RestaurantBooking RestaurantBooking { get; set; }
        public virtual bool IsPayByBankAccount { get; set; }
        public virtual BankAccount BankAccount { get; set; }
        public virtual PaymentGroup PaymentGroup { get; set; }
    }
}