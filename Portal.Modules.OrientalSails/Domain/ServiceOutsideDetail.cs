using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Domain
{
    public class ServiceOutsideDetail
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual double UnitPrice { get; set; }
        public virtual int Quantity { get; set; }
        public virtual double TotalPrice { get; set; }
        public virtual ServiceOutside ServiceOutside { get; set; }
    }
}