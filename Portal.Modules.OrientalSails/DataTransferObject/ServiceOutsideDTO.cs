using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules.OrientalSails.DataTransferObject
{
    public class ServiceOutsideDTO
    {
        public int id { get; set; }
        public string service { get; set; }
        public string unitPrice { get; set; }
        public int quantity { get; set; }
        public string totalPrice { get; set; }
        public bool vat { get; set; }
        public int numberOfReceiptVoucher { get; set; }
        public IList<ServiceOutsideDetailDTO> listServiceOutsideDetailDTO { get; set; }
        public int restaurantBookingId { get; set; }
    }
}