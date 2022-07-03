using CMS.Core.Domain;
using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class ActivityLoggingRepository : RepositoryBase<ActivityLogging>
    {
        public ActivityLoggingRepository() : base() { }
        public ActivityLoggingRepository(ISession session) : base(session) { }

        public IQueryOver<ActivityLogging> ActivityLoggingGetAllByCriterion(DateTime? from, DateTime? to, User user)
        {
            var query = _session.QueryOver<ActivityLogging>();
            if (from != null)
            {
                query = query.Where(x => x.CreatedTime >= from);
            }
            if (to != null)
            {
                query = query.Where(x => x.CreatedTime <= to);
            }
            if (user != null)
            {
                query = query.Where(x => x.CreatedBy == user);
            }
            return query;
        }
    }
}