using CMS.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.Domain
{
    public class ActivityLogging
    {
        public virtual int Id { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual DateTime? CreatedTime { get; set; }
        public virtual string Function { get; set; }
        public virtual string Detail { get; set; }
        public virtual string ObjectId { get; set; }
    }
}