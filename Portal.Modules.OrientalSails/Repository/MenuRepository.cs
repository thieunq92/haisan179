using NHibernate;
using Portal.Modules.OrientalSails.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Repository
{
    public class MenuRepository : RepositoryBase<Menu>
    {
        public MenuRepository() : base() { }
        public MenuRepository(ISession session) : base(session) { }


        public IList<Menu> MenuGetAll()
        {
            return _session.QueryOver<Menu>().Future().ToList();
        }

        public Menu MenuGetById(int menuId)
        {
            return _session.QueryOver<Menu>()
                .Where(x => x.Id == menuId).FutureValue().Value;
        }
    }
}