using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    public class MenuManagementBLL
    {
        public MenuRepository MenuRepository { get; set; }
        public MenuManagementBLL() {
            MenuRepository = new MenuRepository();
        }
        public void Dispose() {
            if (MenuRepository != null)
            {
                MenuRepository.Dispose();
                MenuRepository = null;
            }
        }

        public IList<Menu> MenuGetAll()
        {
            return MenuRepository.MenuGetAll();
        }
    }
}