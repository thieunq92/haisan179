using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.DataTransferObject
{
    public class ServiceOutsideDetailDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string unitPrice { get; set; }
        public int quantity { get; set; }
        public string totalPrice { get; set; }
    }
}