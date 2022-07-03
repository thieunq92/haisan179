using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Enums.RestaurantBooking;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    public class RestaurantBookingByDateBLL
    {
        public RestaurantBookingRepository RestaurantBookingRepository { get; set; }
        public TrackingChangeBookingRepository TrackingChangeBookingRepository { get; set; }
        public RestaurantBookingByDateBLL()
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
        public IList<RestaurantBooking> RestaurantBookingGetAll()
        {
            return RestaurantBookingRepository.RestaurantBookingGetAll();
        }

        public IList<RestaurantBooking> RestaurantBookingGetAllByDate(DateTime date)
        {
            return RestaurantBookingRepository.RestaurantBookingGetAllByDate(date);
        }

        public IList<RestaurantBooking> RestaurantBookingGetAllByCriterion(DateTime Date, int partOfDay, params StatusEnum[] status)
        {
            return RestaurantBookingRepository.RestaurantBookingGetAllByCriterion(-1, Date, "", -1, -1, partOfDay, status);
        }
        public IList<RestaurantBooking> RestaurantBookingGetByKitchen(DateTime date)
        {

            return RestaurantBookingRepository.RestaurantBookingGetByKitchen(date);

        }

        public IList<TrackingChangeBooking> TrackingChangeBookingGetAllByCriterion(DateTime createdDate, string originValue, string columnName)
        {
            return TrackingChangeBookingRepository.TrackingChangeBookingGetAllByCriterion(createdDate, originValue, columnName);
        }

        public void RestaurantBookingSaveOrUpdate(RestaurantBooking restaurantBooking)
        {
            RestaurantBookingRepository.SaveOrUpdate(restaurantBooking);
        }
    }
}