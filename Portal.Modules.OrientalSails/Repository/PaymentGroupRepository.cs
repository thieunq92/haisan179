using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class PaymentGroupRepository : RepositoryBase<PaymentGroup>
    {
        public PaymentGroupRepository() : base() { }
        public PaymentGroupRepository(ISession session) : base(session) { }
    }
}