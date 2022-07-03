using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Domain
{
    public class Menu
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual double Cost { get; set; }
        public virtual string Details { get; set; }
        public virtual double CostOfAdult { get; set; }
        public virtual double CostOfChild { get; set; }
        public virtual double CostOfBaby { get; set; }
    }
}