using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.DataTransferObject
{
    public class MenuDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public double costOfAdult { get; set; }
        public double costOfChild { get; set; }
        public double costOfBaby { get; set; }
        public string details { get; set; }

    }
}