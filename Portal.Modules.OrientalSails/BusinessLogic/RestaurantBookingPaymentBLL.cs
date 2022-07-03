using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    public class RestaurantBookingPaymentBLL
    {
        public RestaurantBookingRepository RestaurantBookingRepository { get; set; }
        public PaymentHistoryRepository PaymentHistoryRepository { get; set; }
        public ActivityLoggingRepository ActivityLoggingRepository { get; set; }
        public PaymentGroupRepository PaymentGroupRepository { get; set; }
        public RestaurantBookingPaymentBLL()
        {
            RestaurantBookingRepository = new RestaurantBookingRepository();
            PaymentHistoryRepository = new PaymentHistoryRepository();
            ActivityLoggingRepository = new ActivityLoggingRepository();
            PaymentGroupRepository = new PaymentGroupRepository();
        }
        public void Dispose()
        {
            if (RestaurantBookingRepository != null)
            {
                RestaurantBookingRepository.Dispose();
                RestaurantBookingRepository = null;
            }
            if (PaymentHistoryRepository != null)
            {
                PaymentHistoryRepository.Dispose();
                PaymentHistoryRepository = null;
            }
            if (ActivityLoggingRepository != null)
            {
                ActivityLoggingRepository.Dispose();
                ActivityLoggingRepository = null;
            }
            if (PaymentGroupRepository != null) {
                PaymentGroupRepository.Dispose();
                PaymentGroupRepository = null;
            }
        }

        public RestaurantBooking RestaurantBookingGetById(int restaurantBookingId)
        {
            return RestaurantBookingRepository.GetById(restaurantBookingId);
        }

        public void RestaurantBookingSaveOrUpdate(RestaurantBooking restaurantBooking)
        {
            RestaurantBookingRepository.SaveOrUpdate(restaurantBooking);
        }

        public void PaymentHistorySaveOrUpdate(PaymentHistory paymentHistory)
        {
            PaymentHistoryRepository.SaveOrUpdate(paymentHistory);
        }

        public IList<PaymentHistory> PaymentHistoryGetByBookingId(int restaurantBookingId)
        {
            return PaymentHistoryRepository.PaymentHistoryGetByBookingId(restaurantBookingId);
        }

        public void ActivityLoggingSaveOrUpdate(ActivityLogging activityLogging)
        {
            ActivityLoggingRepository.SaveOrUpdate(activityLogging);
        }

        public void PaymentGroupSaveOrUpdate(PaymentGroup paymentGroup)
        {
            PaymentGroupRepository.SaveOrUpdate(paymentGroup);
        }
    }
}