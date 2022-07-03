using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class AgencyRepository : RepositoryBase<Agency>
    {
        public AgencyRepository() : base() { }

        public AgencyRepository(ISession session) : base(session) { }

        public IList<Agency> AgencyGetAll()
        {
            return _session.QueryOver<Agency>().Where(x => x.Deleted == false).Future().ToList();
        }

        public Agency AgencyGetById(int agencyId)
        {
            return _session.QueryOver<Agency>().Where(x => x.Deleted == false)
                .Where(x => x.Id == agencyId).FutureValue().Value;
        }

        public IList<Agency> ViewActivitiesBLL_AgencyGetAllBy(int userId, DateTime? from, DateTime? to)
        {
            var query = _session.QueryOver<Agency>();
            if (userId > -1)
            {
                query = query.Where(x => x.CreatedBy.Id == userId || x.ModifiedBy.Id == userId);
            }

            if (from != null)
            {
                query = query.Where(x => x.CreatedDate >= from || x.ModifiedDate >= from);
            }

            if (to != null)
            {
                query = query.Where(x => x.CreatedDate <= to || x.ModifiedDate <= from);
            }

            return query.Future().ToList();
        }

        public IQueryOver<Agency, Agency> AgencyGetAllByCreatedDate(DateTime? today, DateTime? endOfToday)
        {
            var query = _session.QueryOver<Agency>();
            if (today.HasValue)
            {
                query = query.Where(x => x.CreatedDate >= today);
            }
            if (endOfToday.HasValue)
            {
                query = query.Where(x => x.CreatedDate <= endOfToday);
            }
            return query;
        }
    }
}