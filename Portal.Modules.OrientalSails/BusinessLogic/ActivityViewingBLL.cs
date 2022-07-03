using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using Portal.Modules.OrientalSails.Domain;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    public class ActivityViewingBLL
    {
        public UserRepository UserRepository { get; set; }
        public ActivityLoggingRepository ActivityLoggingRepository { get; set; }
        public RestaurantBookingRepository RestaurantBookingRepository { get; set; }
        public AgencyRepository AgencyRepository { get; set; }
        public MenuRepository MenuRepository { get; set; }
        public ActivityViewingBLL()
        {
            UserRepository = new UserRepository();
            ActivityLoggingRepository = new ActivityLoggingRepository();
            RestaurantBookingRepository = new RestaurantBookingRepository();
            AgencyRepository = new AgencyRepository();
            MenuRepository = new MenuRepository();
        }

        public void Dispose()
        {
            if (UserRepository != null)
            {
                UserRepository.Dispose();
                UserRepository = null;
            }
            if (ActivityLoggingRepository != null)
            {
                ActivityLoggingRepository.Dispose();
                ActivityLoggingRepository = null;
            }
            if(RestaurantBookingRepository != null){
                RestaurantBookingRepository.Dispose();
                RestaurantBookingRepository = null;
            }
            if (AgencyRepository != null)
            {
                AgencyRepository.Dispose();
                AgencyRepository = null;
            }
            if (MenuRepository != null)
            {
                MenuRepository.Dispose();
                MenuRepository = null;
            }
        }

        public User UserGetById(int userId)
        {
            return UserRepository.UserGetById(userId);
        }

        public IQueryOver<ActivityLogging> ActivityLoggingGetAllByCriterion(DateTime from, DateTime to, User user)
        {
            return ActivityLoggingRepository.ActivityLoggingGetAllByCriterion(from, to, user);
        }

        public RestaurantBooking RestaurantBookingGetById(int bookingId)
        {
            return RestaurantBookingRepository.RestaurantBookingGetById(bookingId);
        }

        public Agency AgencyGetById(int agencyId)
        {
            return AgencyRepository.AgencyGetById(agencyId);
        }

        public Menu MenuGetById(int menuId)
        {
            return MenuRepository.MenuGetById(menuId);
        }
    }
}