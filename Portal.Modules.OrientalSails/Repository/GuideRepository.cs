using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class GuideRepository:RepositoryBase<Guide>
    {
        public GuideRepository() : base() { }
        public GuideRepository(ISession session) : base(session) { }

        public IList<Guide> GuideGetAllByBookingId(int restaurantBookingId)
        {
            return _session.QueryOver<Guide>().Where(x => x.RestaurantBooking.Id == restaurantBookingId).Future().ToList();
        }
    }
}