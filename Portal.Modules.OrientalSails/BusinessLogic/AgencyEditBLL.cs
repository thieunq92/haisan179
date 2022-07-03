using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    public class AgencyEditBLL
    {
        public AgencyLocationRepository AgencyLocationRepository { get; set; }
        public RoleRepository RoleRepository { get; set; }
        public ActivityLoggingRepository ActivityLoggingRepository { get; set; }
        public AgencyRepository AgencyRepository { get; set; }
        public AgencyHistoryRepository AgencyHistoryRepository { get; set; }
        public AgencyEditBLL()
        {
            AgencyLocationRepository = new AgencyLocationRepository();
            RoleRepository = new RoleRepository();
            ActivityLoggingRepository = new ActivityLoggingRepository();
            AgencyRepository = new AgencyRepository();
            AgencyHistoryRepository = new AgencyHistoryRepository();
        }

        public IList<AgencyLocation> AgencyLocationGetAll()
        {
            return AgencyLocationRepository.AgencyLocationGetAll();
        }

        public IList<Role> RoleGetAll()
        {
            return RoleRepository.RoleGetAll();
        }

        public void Dispose()
        {
            if (AgencyLocationRepository != null)
            {
                AgencyLocationRepository.Dispose();
                AgencyLocationRepository = null;
            }

            if (RoleRepository != null)
            {
                RoleRepository.Dispose();
                RoleRepository = null;
            }
            if (ActivityLoggingRepository != null)
            {
                ActivityLoggingRepository.Dispose();
                ActivityLoggingRepository = null;
            }
            if (AgencyRepository != null)
            {
                AgencyRepository.Dispose();
                AgencyRepository = null;
            }
            if (AgencyHistoryRepository != null)
            {
                AgencyHistoryRepository.Dispose();
                AgencyHistoryRepository = null;
            }
        }

        public Role RoleGetById(int roleId)
        {
            return RoleRepository.RoleGetById(roleId);
        }

        public void ActivityLoggingSaveOrUpdate(ActivityLogging activityLogging)
        {
            ActivityLoggingRepository.SaveOrUpdate(activityLogging);
        }

        public Agency AgencyGetById(int agencyId)
        {
            return AgencyRepository.AgencyGetById(agencyId);
        }

        public void AgencySaveOrUpdate(Agency agency)
        {
            AgencyRepository.SaveOrUpdate(agency);
        }

        public void AgencyHistorySaveOrUpdate(AgencyHistory agencyHistory)
        {
            AgencyHistoryRepository.SaveOrUpdate(agencyHistory);
        }
    }
}