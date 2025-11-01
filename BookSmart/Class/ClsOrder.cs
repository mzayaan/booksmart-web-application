using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookSmart.Class
{
    public class ClsOrder
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
    }

}