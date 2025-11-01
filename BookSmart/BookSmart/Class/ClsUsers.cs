using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookSmart.Class
{
    public class ClsUser
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int Country { get; set; }
        public DateTime DateCreated { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Role { get; set; }
    }

}
