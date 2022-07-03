using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class TrackingChangeBookingRepository : RepositoryBase<TrackingChangeBooking>
    {
        public TrackingChangeBookingRepository() : base() { }
        public TrackingChangeBookingRepository(ISession session) : base(session) { }


        public IList<TrackingChangeBooking> TrackingChangeBookingGetAllByBookingId(int bookingId)
        {
            return _session.QueryOver<TrackingChangeBooking>().Where(x => x.RestaurantBooking.Id == bookingId).Future().ToList();
        }

        public IList<TrackingChangeBooking> TrackingChangeBookingGetAllByCriterion(DateTime? createdDate, string originValueDate, string columnName)
        {
            var query = _session.QueryOver<TrackingChangeBooking>();
            if (createdDate.HasValue)
            {
                var startOfCreatedDate = createdDate.Value.Date;
                var endOfCreatedDate = startOfCreatedDate.Add(new TimeSpan(23, 59, 59));
                query = query.Where(x => x.CreatedDate >= startOfCreatedDate && x.CreatedDate <= endOfCreatedDate);
            }
            if (!String.IsNullOrEmpty(columnName))
            {
                query = query.Where(x => x.ColumnName == columnName);
                if (columnName == "Date")
                {
                    if (!String.IsNullOrEmpty(originValueDate))
                    {
                        query = query.Where(x => x.OriginValue == originValueDate);
                    }
                }
            }
            return query.Future().ToList();
        }
    }
}