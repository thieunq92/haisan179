using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Enums.RestaurantBooking;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    public class BookingManagementBLL
    {
        public RestaurantBookingRepository RestaurantBookingRepository { get; set; }
        public BookingManagementBLL()
        {
            RestaurantBookingRepository = new RestaurantBookingRepository();
        }
        public void Dispose()
        {
            if (RestaurantBookingRepository != null)
            {
                RestaurantBookingRepository.Dispose();
                RestaurantBookingRepository = null;
            }
        }

        public IList<RestaurantBooking> RestaurantBookingGetAll()
        {
            return RestaurantBookingRepository.RestaurantBookingGetAll();
        }

        public IList<RestaurantBooking> RestaurantBookingGetAllByCriterion(int code, DateTime? date, string agency, int payment, int agencyId, params StatusEnum[] status)
        {
            return RestaurantBookingRepository.RestaurantBookingGetAllByCriterion(code, date, agency, payment, agencyId, status);
        }

        public IQueryOver<RestaurantBooking,RestaurantBooking> RestaurantBookingGetAllByCriterion(int code, DateTime? date, string agency, int partOfDay, int agencyId){
            return RestaurantBookingRepository.RestaurantBookingGetAllByCriterion(code, date, agency,-1, partOfDay, -1, agencyId);
        }
    }
}