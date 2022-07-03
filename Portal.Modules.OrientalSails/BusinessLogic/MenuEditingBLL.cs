using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.BusinessLogic
{
    public class MenuEditingBLL
    {
        public MenuRepository MenuRepository { get; set; }
        public ActivityLoggingRepository ActivityLoggingRepository { get; set; }
        public MenuEditingBLL()
        {
            MenuRepository = new MenuRepository();
            ActivityLoggingRepository = new ActivityLoggingRepository();
        }
        public void Dispose()
        {
            if (MenuRepository != null)
            {
                MenuRepository.Dispose();
                MenuRepository = null;
            }
            if (ActivityLoggingRepository != null)
            {
                ActivityLoggingRepository.Dispose();
                ActivityLoggingRepository = null;
            }
        }
        public Menu MenuGetById(int menuId)
        {
            return MenuRepository.GetById(menuId);
        }

        public void MenuSaveOrUpdate(Menu menu)
        {
            MenuRepository.SaveOrUpdate(menu);
        }

        public void ActivityLoggingSaveOrUpdate(ActivityLogging activityLogging)
        {
            ActivityLoggingRepository.SaveOrUpdate(activityLogging);
        }
    }
}