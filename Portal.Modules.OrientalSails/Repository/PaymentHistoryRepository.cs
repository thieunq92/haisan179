using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class PaymentHistoryRepository : RepositoryBase<PaymentHistory>
    {
        public PaymentHistoryRepository() : base() { }
        public PaymentHistoryRepository(ISession session) : base(session) { }

        public IList<PaymentHistory> PaymentHistoryGetByBookingId(int restaurantBookingId)
        {
            return _session.QueryOver<PaymentHistory>()
                .Where(x => x.RestaurantBooking.Id == restaurantBookingId)
                .OrderBy(x=>x.Time).Desc
                .Future()
                .ToList();
        }
    }
}