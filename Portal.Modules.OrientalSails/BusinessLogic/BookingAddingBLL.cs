using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    public class BookingAddingBLL
    {
        RestaurantBookingRepository RestaurantBookingRepository { get; set; }
        AgencyRepository AgencyRepository { get; set; }
        ActivityLoggingRepository ActivityLoggingRepository { get; set; }
        public BookingAddingBLL()
        {
            RestaurantBookingRepository = new RestaurantBookingRepository();
            AgencyRepository = new AgencyRepository();
            ActivityLoggingRepository = new ActivityLoggingRepository();
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
            if (ActivityLoggingRepository != null)
            {
                ActivityLoggingRepository.Dispose();
                ActivityLoggingRepository = null;
            }
        }

        public void RestaurantBookingSaveOrUpdate(RestaurantBooking restaurantBooking)
        {
            RestaurantBookingRepository.SaveOrUpdate(restaurantBooking);
        }

        public Agency AgencyGetById(int agencyId)
        {
            return AgencyRepository.GetById(agencyId);
        }

        public void ActivityLoggingSaveOrUpdate(ActivityLogging activityLogging)
        {
            ActivityLoggingRepository.SaveOrUpdate(activityLogging);
        }
    }
}