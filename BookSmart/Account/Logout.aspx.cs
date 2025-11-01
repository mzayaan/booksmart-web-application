using System;
using System.Web.Security;

namespace BookSmart.Account
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            if (Response.Cookies["UserID"] != null)
                Response.Cookies["UserID"].Expires = DateTime.Now.AddDays(-1);

            if (Response.Cookies["Role"] != null)
                Response.Cookies["Role"].Expires = DateTime.Now.AddDays(-1);


            Response.Redirect("~/Account/Login.aspx");
        }
    }
}
