using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class BankAccountRepository : RepositoryBase<BankAccount>
    {
        public BankAccountRepository() : base() { }
        public BankAccountRepository(ISession session) : base(session) { }

        public IQueryOver<BankAccount, BankAccount> BankAccountGetAll()
        {
            return _session.QueryOver<BankAccount>();
        }

        public BankAccount BankAccountGetById(int bankAccountId)
        {
            return _session.QueryOver<BankAccount>().Where(x => x.Id == bankAccountId).FutureValue().Value;
        }
    }
}