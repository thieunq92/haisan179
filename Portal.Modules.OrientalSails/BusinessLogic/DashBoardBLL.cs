using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using Portal.Modules.OrientalSails.Domain;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    public class DashBoardBLL
    {
        public RestaurantBookingRepository RestaurantBookingRepository { get; set; }
        public AgencyRepository AgencyRepository { get; set; }
        public DashBoardBLL()
        {
            RestaurantBookingRepository = new RestaurantBookingRepository();
            AgencyRepository = new AgencyRepository();
        }

        public void Dispose()
        {
            if (RestaurantBookingRepository != null)
            {
                RestaurantBookingRepository.Dispose();
                RestaurantBookingRepository = null;
            }
            if (AgencyRepository != null)
            {
                AgencyRepository.Dispose();
                AgencyRepository = null;
            }
        }

        public IQueryOver<RestaurantBooking, RestaurantBooking> RestaurantBookingGetAllByDate(DateTime date)
        {
            return RestaurantBookingRepository.QueryOverRestaurantBookingGetAllByDate(date);
        }

        public IQueryOver<RestaurantBooking, RestaurantBooking> RestaurantBookingGetAllByDateRange(DateTime from, DateTime to)
        {
            return RestaurantBookingRepository.QueryOverRestaurantBookingGetAllByDateRange(from, to, -1);
        }

        public IQueryOver<RestaurantBooking, RestaurantBooking> RestaurantBookingGetAllByOutstandingDebt(DateTime outstandingDebtTo)
        {
            return RestaurantBookingRepository.RestaurantBookingGetAllOutstandingDebt(outstandingDebtTo);
        }

        public IQueryOver<RestaurantBooking, RestaurantBooking> RestaurantBookingGetAllByCreatedDate(DateTime today, DateTime endOfToDay)
        {
            return RestaurantBookingRepository.RestaurantBookingGetAllByCreatedDate(today, endOfToDay);
        }

        public IQueryOver<Agency, Agency> AgencyGetAllByCreatedDate(DateTime today, DateTime endOfToday)
        {
            return AgencyRepository.AgencyGetAllByCreatedDate(today, endOfToday);
        }
    }
}