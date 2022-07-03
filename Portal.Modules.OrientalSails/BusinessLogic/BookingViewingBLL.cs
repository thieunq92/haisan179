using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    public class BookingViewingBLL
    {
        public MenuRepository MenuRepository { set; get; }
        public RestaurantBookingRepository RestaurantBookingRepository { get; set; }
        public AgencyRepository AgencyRepository { get; set; }
        public CommissionRepository CommissionRepository { get; set; }
        public ServiceOutsideRepository ServiceOutsideRepository { get; set; }
        public GuideRepository GuideRepository { get; set; }
        public AgencyContactRepository AgencyContactRepository { get; set; }
        public ServiceOutsideDetailRepository ServiceOutsideDetailRepository { get; set; }
        public TrackingChangeBookingRepository TrackingChangeBookingRepository { get; set; }
        public ActivityLoggingRepository ActivityLoggingRepository { get; set; }
        public BookingViewingBLL()
        {
            MenuRepository = new MenuRepository();
            RestaurantBookingRepository = new RestaurantBookingRepository();
            AgencyRepository = new AgencyRepository();
            CommissionRepository = new CommissionRepository();
            ServiceOutsideRepository = new ServiceOutsideRepository();
            GuideRepository = new GuideRepository();
            AgencyContactRepository = new AgencyContactRepository();
            ServiceOutsideDetailRepository = new ServiceOutsideDetailRepository();
            TrackingChangeBookingRepository = new TrackingChangeBookingRepository();
            ActivityLoggingRepository = new ActivityLoggingRepository();
        }
        public void Dispose()
        {
            if (MenuRepository != null)
            {
                MenuRepository.Dispose();
                MenuRepository = null;
            }
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
            if (CommissionRepository != null)
            {
                CommissionRepository.Dispose();
                CommissionRepository = null;
            }
            if (ServiceOutsideRepository != null)
            {
                ServiceOutsideRepository.Dispose();
                ServiceOutsideRepository = null;
            }
            if (GuideRepository != null)
            {
                GuideRepository.Dispose();
                GuideRepository = null;
            }
            if (AgencyContactRepository != null)
            {
                AgencyContactRepository.Dispose();
                AgencyContactRepository = null;
            }
            if (ServiceOutsideDetailRepository != null)
            {
                ServiceOutsideDetailRepository.Dispose();
                ServiceOutsideDetailRepository = null;
            }
            if (TrackingChangeBookingRepository != null)
            {
                TrackingChangeBookingRepository.Dispose();
                TrackingChangeBookingRepository = null;
            }
            if (ActivityLoggingRepository != null)
            {
                ActivityLoggingRepository.Dispose();
                ActivityLoggingRepository = null;
            }
        }

        public IList<Menu> MenuGetAll()
        {
            return MenuRepository.MenuGetAll();
        }

        public RestaurantBooking RestaurantBookingGetById(int restaurantBookingId)
        {
            return RestaurantBookingRepository.GetById(restaurantBookingId);
        }

        public void RestaurantBookingSaveOrUpdate(RestaurantBooking restaurantBooking)
        {
            RestaurantBookingRepository.SaveOrUpdate(restaurantBooking);
            foreach(var trackingChangeBooking in restaurantBooking.ListPendingTrackingChangeBooking){
                TrackingChangeBookingRepository.SaveOrUpdate(trackingChangeBooking);
            }
        }

        public Agency AgencyGetById(int agencyId)
        {
            return AgencyRepository.AgencyGetById(agencyId);
        }

        public Menu MenuGetById(int menuId)
        {
            return MenuRepository.MenuGetById(menuId);
        }

        public Commission CommissionGetById(int commissionId)
        {
            return CommissionRepository.GetById(commissionId);
        }

        public void CommissionSaveOrUpdate(Commission commission)
        {
            CommissionRepository.SaveOrUpdate(commission);
        }

        public IList<Commission> CommissionGetAllByBookingId(int restaurantBookingId)
        {
            return CommissionRepository.CommissionGetAllByBookingId(restaurantBookingId);
        }

        public void CommissionDelete(Commission commission)
        {
            CommissionRepository.Delete(commission);
        }

        public ServiceOutside ServiceOutsideGetById(int serviceOutsideId)
        {
            return ServiceOutsideRepository.GetById(serviceOutsideId);
        }

        public void ServiceOutsideSaveOrUpdate(ServiceOutside serviceOutside)
        {
            ServiceOutsideRepository.SaveOrUpdate(serviceOutside);
        }

        public IList<ServiceOutside> ServiceOutsideGetAllByBookingId(int restaurantBookingId)
        {
            return ServiceOutsideRepository.ServiceOutsideGetAllByBookingId(restaurantBookingId);
        }

        public void ServiceOutsideDelete(ServiceOutside serviceOutside)
        {
            ServiceOutsideRepository.Delete(serviceOutside);
        }

        public Guide GuideGetById(int guideId)
        {
            return GuideRepository.GetById(guideId);
        }

        public void GuideSaveOrUpdate(Guide guide)
        {
            GuideRepository.SaveOrUpdate(guide);
        }

        public IList<Guide> GuideGetAllByBookingId(int restaurantBookingId)
        {
            return GuideRepository.GuideGetAllByBookingId(restaurantBookingId);
        }

        public void GuideDelete(Guide guide)
        {
            GuideRepository.Delete(guide);
        }

        public IList<AgencyContact> BookerGetAllByAgencyId(int agencyId)
        {
            return AgencyContactRepository.AgencyContactGetAllByAgency(agencyId);
        }

        public AgencyContact BookerGetById(int bookerId)
        {
            return AgencyContactRepository.BookerGetById(bookerId);
        }

        public ServiceOutsideDetail ServiceOutsideDetailGetById(int serviceOutsideDetailId)
        {
            return ServiceOutsideDetailRepository.GetById(serviceOutsideDetailId);
        }

        public void ServiceOutsideDetailSaveOrUpdate(ServiceOutsideDetail serviceOutsideDetail)
        {
            ServiceOutsideDetailRepository.SaveOrUpdate(serviceOutsideDetail);
        }

        public IList<ServiceOutsideDetail> ServiceOutsideDetailGetAllByServiceOutsideId(int serviceOutsideId)
        {
            return ServiceOutsideDetailRepository.ServiceOutsideDetailGetAllByServiceOutsideId(serviceOutsideId);
        }

        public void ServiceOutsideDetailDelete(ServiceOutsideDetail serviceOutsideDetail)
        {
            ServiceOutsideDetailRepository.Delete(serviceOutsideDetail);
        }

        public IList<TrackingChangeBooking> TrackingChangeBookingGetAllByBookingId(int bookingId)
        {
            return TrackingChangeBookingRepository.TrackingChangeBookingGetAllByBookingId(bookingId);
        }

        public void ActivityLoggingSaveOrUpdate(ActivityLogging activityLogging)
        {
            ActivityLoggingRepository.SaveOrUpdate(activityLogging);
        }
    }
}