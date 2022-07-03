using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    public class AgencyListBLL
    {
        public RoleRepository RoleRepository { get; set; }
        public AgencyRepository AgencyRepository { get; set; }
        public AgencyListBLL()
        {
            RoleRepository = new RoleRepository();
            AgencyRepository = new AgencyRepository();
        }
        public void Dispose() {
            if (RoleRepository != null)
            {
                RoleRepository.Dispose();
                RoleRepository = null;
            }
            if (AgencyRepository != null)
            {
                AgencyRepository.Dispose();
                AgencyRepository = null;
            }
        }
        public Role RoleGetById(int roleId)
        {
            return RoleRepository.RoleGetById(roleId);
        }
        public Agency AgencyGetById(int agencyId)
        {
            return AgencyRepository.AgencyGetById(agencyId);
        }
    }
}