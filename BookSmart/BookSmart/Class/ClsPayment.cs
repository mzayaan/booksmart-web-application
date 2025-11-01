using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookSmart.Class
{
    public class ClsPayment
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
    }

}