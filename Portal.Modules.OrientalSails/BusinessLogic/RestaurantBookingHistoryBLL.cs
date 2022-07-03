using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    public class RestaurantBookingHistoryBLL
    {
        public RestaurantBookingRepository RestaurantBookingRepository { get; set; }
        public TrackingChangeBookingRepository TrackingChangeBookingRepository { get; set; }
        public RestaurantBookingHistoryBLL()
        {
            RestaurantBookingRepository = new RestaurantBookingRepository();
            TrackingChangeBookingRepository = new TrackingChangeBookingRepository();
        }
        public void Dispose()
        {
            if (RestaurantBookingRepository != null)
            {
                RestaurantBookingRepository.Dispose();
                RestaurantBookingRepository = null;
            }
            if (TrackingChangeBookingRepository != null)
            {
                TrackingChangeBookingRepository.Dispose();
                TrackingChangeBookingRepository = null;
            }
        }
        public RestaurantBooking RestaurantBookingGetById(int restaurantBookingId)
        {
            return RestaurantBookingRepository.GetById(restaurantBookingId);
        }
        public IList<TrackingChangeBooking> TrackingChangeBookingGetAllByBookingId(int bookingId)
        {
            return TrackingChangeBookingRepository.TrackingChangeBookingGetAllByBookingId(bookingId);
        }
    }
}