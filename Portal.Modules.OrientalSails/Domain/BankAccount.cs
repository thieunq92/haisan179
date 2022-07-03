using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Domain
{
    public class BankAccount
    {
        public virtual int Id { get; set; }
        public virtual string AccName { get; set; }
        public virtual string AccNo { get; set; }
        public virtual string AccDetail { get; set; }
    }
}