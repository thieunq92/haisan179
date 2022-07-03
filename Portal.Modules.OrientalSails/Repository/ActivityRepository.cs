using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class ActivityRepository : RepositoryBase<Activity>
    {
        public ActivityRepository() : base() { }
        public ActivityRepository(ISession session)
            : base(session)
        {
        }
        public IList<Activity> MeetingGetAllBy(int userId, DateTime? from, DateTime? to)
        {
            var query = _session.QueryOver<Activity>()
                .Where(x => x.User.Id == userId);
            if (from != null)
            {
                query = query.Where(x => x.UpdateTime >= from);
            }

            if (to != null)
            {
                query = query.Where(x => x.UpdateTime <= to);
            }

            return query.Future().ToList();
        }
    }
}