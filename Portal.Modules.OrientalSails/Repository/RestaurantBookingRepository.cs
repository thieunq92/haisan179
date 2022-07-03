using NHibernate;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Enums.RestaurantBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class RestaurantBookingRepository : RepositoryBase<RestaurantBooking>
    {
        public RestaurantBookingRepository() : base() { }
        public RestaurantBookingRepository(ISession session) : base(session) { }

        public IList<RestaurantBooking> RestaurantBookingGetAll()
        {
            return _session.QueryOver<RestaurantBooking>().Future().ToList();
        }

        public IList<RestaurantBooking> RestaurantBookingGetAllByDate(DateTime date)
        {
            return _session.QueryOver<RestaurantBooking>()
                .Where(x => x.Status != StatusEnum.Cancel)
                .Where(x => x.Date == date)
                .Future()
                .ToList();
        }

        public IQueryOver<RestaurantBooking, RestaurantBooking> QueryOverRestaurantBookingGetAllByDate(DateTime date)
        {
            return _session.QueryOver<RestaurantBooking>()
               .Where(x => x.Status != StatusEnum.Cancel)
               .Where(x => x.Date == date);
        }

        public IList<RestaurantBooking> RestaurantBookingGetAllByDateRange(DateTime from, DateTime to, int agencyId)
        {
            var query = _session.QueryOver<RestaurantBooking>().Where(x => x.Date >= from && x.Date <= to);
            query = query.Where(x => x.Status == StatusEnum.Approved);
            Agency agencyAlias = null;
            query = query.JoinAlias(x => x.Agency, () => agencyAlias);
            if (agencyId != -1)
            {
                query = query.Where(x => agencyAlias.Id == agencyId);
            }
            return query.Future().ToList();
        }
        public IQueryOver<RestaurantBooking, RestaurantBooking> QueryOverRestaurantBookingGetAllByDateRange(DateTime from, DateTime to, int agencyId)
        {
            var query = _session.QueryOver<RestaurantBooking>().Where(x => x.Date >= from && x.Date <= to);
            query = query.Where(x => x.Status == StatusEnum.Approved);
            Agency agencyAlias = null;
            query = query.JoinAlias(x => x.Agency, () => agencyAlias);
            if (agencyId != -1)
            {
                query = query.Where(x => agencyAlias.Id == agencyId);
            }
            return query;
        }

        public IList<RestaurantBooking> RestaurantBookingGetByKitchen(DateTime date)
        {
            return _session.QueryOver<RestaurantBooking>()
                .Where(x => x.Date == date && x.Status == StatusEnum.Approved)
                .Future()
                .ToList();
        }
        public IList<RestaurantBooking> RestaurantBookingGetAllByCriterion(int code, DateTime? date, string agency, int payment, int agencyId, params StatusEnum[] status)
        {
            return RestaurantBookingGetAllByCriterion(code, date, agency, payment, agencyId, -1, status);
        }

        public IList<RestaurantBooking> RestaurantBookingGetAllByCriterion(int code, DateTime? date, string agency, int payment, int agencyId, int partOfDay, params StatusEnum[] status)
        {
            var query = _session.QueryOver<RestaurantBooking>();
            if (status.Length > 0)
            {
                query = query.WhereRestrictionOn(x => x.Status).IsIn(status);
            }

            if (code != -1)
            {
                query = query.Where(x => x.Id == code);
            }
            if (date.HasValue)
            {
                query = query.Where(x => x.Date == date);
            }
            Agency agencyAlias = null;
            query = query.JoinAlias(x => x.Agency, () => agencyAlias);
            if (!String.IsNullOrEmpty(agency))
            {
                query = query.WhereRestrictionOn(x => agencyAlias.Name).IsLike(agency, MatchMode.Anywhere);
            }
            if (payment != -1)
            {
                query = query.Where(x => x.Payment == payment);
            }
            if (agencyId != -1)
            {
                query = query.Where(x => agencyAlias.Id == agencyId);
            }
            if (partOfDay != -1)
            {
                query = query.Where(x => x.PartOfDay == partOfDay);
            }
            return query.Future().ToList();
        }


        public IQueryOver<RestaurantBooking, RestaurantBooking> RestaurantBookingGetAllByCriterion(int code, DateTime? date, string agency, int payment, int partOfDay, int tam, int agencyId, params StatusEnum[] status)
        {
            var query = QueryOver.Of<RestaurantBooking>();
            if (status.Length > 0)
            {
                query = query.WhereRestrictionOn(x => x.Status).IsIn(status);
            }

            if (code != -1)
            {
                query = query.Where(x => x.Id == code);
            }
            if (date.HasValue)
            {
                query = query.Where(x => x.Date == date);
            }
            Agency agencyAlias = null;
            query = query.JoinAlias(x => x.Agency, () => agencyAlias);
            if (!String.IsNullOrEmpty(agency))
            {
                query = query.WhereRestrictionOn(x => agencyAlias.Name).IsLike(agency, MatchMode.Anywhere);
            }
            if (partOfDay != -1)
            {
                query = query.Where(x => x.PartOfDay == partOfDay);
            }
            if (agencyId != -1)
            {
                query = query.Where(x => agencyAlias.Id == agencyId);
            }
            if (partOfDay != -1)
            {
                query = query.Where(x => x.PartOfDay == partOfDay);
            }
            query = query.Select(Projections.Distinct(Projections.Property<RestaurantBooking>(x => x.Id)));
            var mainQuery = _session.QueryOver<RestaurantBooking>().WithSubquery.WhereProperty(x => x.Id).In(query);
            return mainQuery;
        }

        public IQueryOver<RestaurantBooking, RestaurantBooking> RestaurantBookingGetAllOutstandingDebt(DateTime? outstandingDebtTo)
        {
            var query = _session.QueryOver<RestaurantBooking>();
            query = query.Where(x => x.Status == StatusEnum.Approved);
            if (outstandingDebtTo.HasValue)
            {
                query = query.Where(x => x.Date <= outstandingDebtTo);
            }
            query.Where(x => x.Receivable > 0 || x.MarkIsPaid == false);
            return query;
        }

        public IQueryOver<RestaurantBooking, RestaurantBooking> RestaurantBookingGetAllByCreatedDate(DateTime? from, DateTime? to)
        {
            var query = _session.QueryOver<RestaurantBooking>();
            if (from.HasValue)
            {
                query = query.Where(x => x.CreatedDate >= from);
            }
            if (to.HasValue)
            {
                query = query.Where(x => x.CreatedDate <= to);
            }
            return query;
        }

        public IQueryOver<RestaurantBooking,RestaurantBooking> RestaurantBookingGetAllByListId(List<int> listRestaurantBookingId)
        {
            var query = _session.QueryOver<RestaurantBooking>();
            query = query.AndRestrictionOn(x => x.Id).IsIn(listRestaurantBookingId);
            return query;
        }

        public RestaurantBooking RestaurantBookingGetById(int bookingId)
        {
            return _session.QueryOver<RestaurantBooking>().Where(x=>x.Id == bookingId).FutureValue().Value;
        }
    }
}