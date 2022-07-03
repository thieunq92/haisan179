using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class CommissionRepository:RepositoryBase<Commission>
    {
        public CommissionRepository() : base() { }
        public CommissionRepository(ISession session) : base(session) { }

        public IList<Commission> CommissionGetAllByBookingId(int restaurantBookingId)
        {
            return _session.QueryOver<Commission>().Where(x => x.RestaurantBooking.Id == restaurantBookingId).Future().ToList();
        }
    }
}