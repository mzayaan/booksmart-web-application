using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookSmart.Class
{
    public class ClsUserCredentials
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateAddedtimestamp { get; set; }
        public DateTime Lastmodifiedtimestamp { get; set; }
        public bool Status { get; set; }

        public bool Add()
        {
            return false;
        }

        public bool Update()
        {
            return false;
        }

        public bool Delete()
        {
            return false;
        }

        public bool Login()
        {
            if (Email == "s" && Password == "b")
                return true;
            else
                return false;
        }

    }



}