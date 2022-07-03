using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    public class RestaurantBookingSelectedPaymentBLL
    {
        public RestaurantBookingRepository RestaurantBookingRepository { get; set; }
        public BankAccountRepository BankAccountRepository { get; set; }
        public PaymentHistoryRepository PaymentHistoryRepository { get; set; }
        public ActivityLoggingRepository ActivityLoggingRepository { get; set; }
        public PaymentGroupRepository PaymentGroupRepository { get; set; }
        public RestaurantBookingSelectedPaymentBLL()
        {
            RestaurantBookingRepository = new RestaurantBookingRepository();
            BankAccountRepository = new BankAccountRepository();
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
            if (BankAccountRepository != null)
            {
                BankAccountRepository.Dispose();
                BankAccountRepository = null;
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
            if (PaymentGroupRepository != null)
            {
                PaymentGroupRepository.Dispose();
                PaymentGroupRepository = null;
            }
        }
        public IQueryOver<RestaurantBooking,RestaurantBooking> RestaurantBookingGetAllByListId(List<int> listRestaurantBookingId)
        {
            return RestaurantBookingRepository.RestaurantBookingGetAllByListId(listRestaurantBookingId);
        }

        public void RestaurantBookingSaveOrUpdate(RestaurantBooking restaurantBooking)
        {
            RestaurantBookingRepository.SaveOrUpdate(restaurantBooking);
        }

        public IQueryOver<BankAccount,BankAccount> BankAccountGetAll()
        {
            return BankAccountRepository.BankAccountGetAll();
        }

        public void PaymentHistorySaveOrUpdate(PaymentHistory paymentHistory)
        {
            PaymentHistoryRepository.SaveOrUpdate(paymentHistory);
        }

        public BankAccount BankAccountGetById(int bankAccountId)
        {
            return BankAccountRepository.BankAccountGetById(bankAccountId);
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