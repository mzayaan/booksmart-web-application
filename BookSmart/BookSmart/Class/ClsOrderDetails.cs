using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookSmart.Class
{
    public class ClsOrderDetail
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int BookID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

}