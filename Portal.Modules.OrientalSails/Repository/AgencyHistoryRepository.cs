using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class AgencyHistoryRepository : RepositoryBase<AgencyHistory>
    {
        public AgencyHistoryRepository() { }
        public AgencyHistoryRepository(ISession session):base(session){}
    }
}