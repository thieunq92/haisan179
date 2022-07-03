using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class ServiceOutsideDetailRepository : RepositoryBase<ServiceOutsideDetail>
    {
        public ServiceOutsideDetailRepository() : base() { }
        public ServiceOutsideDetailRepository(ISession session) : base(session) { }


        public IList<ServiceOutsideDetail> ServiceOutsideDetailGetAllByServiceOutsideId(int serviceOutsideId)
        {
            return _session.QueryOver<ServiceOutsideDetail>().Where(x => x.ServiceOutside.Id == serviceOutsideId).Future().ToList();
        }
    }
}