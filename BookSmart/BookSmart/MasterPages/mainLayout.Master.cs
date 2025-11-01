using System;
using System.Web;
using System.Web.UI.WebControls;

namespace BookSmart.MasterPages
{
    public partial class mainLayout : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Disable browser cache for all pages
            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetRevalidation(System.Web.HttpCacheRevalidation.AllCaches);
            Response.AppendHeader("Pragma", "no-cache");

            if (!IsPostBack)
            {
                string role = Request.Cookies["Role"]?.Value;
                bool isAdmin = !string.IsNullOrEmpty(role) && role.Equals("admin", StringComparison.OrdinalIgnoreCase);
                bool isLoggedIn = HttpContext.Current.User.Identity.IsAuthenticated;

                var hlAdmin = (HyperLink)FindControl("hlAdmin");
                var hlDashboard = (HyperLink)FindControl("hlDashboard");
                var hlLogin = (HyperLink)FindControl("hlLogin");
                var hlRegister = (HyperLink)FindControl("hlRegister");
                var hlLogout = (HyperLink)FindControl("hlLogout");

                if (hlAdmin != null) hlAdmin.Visible = isAdmin;
                if (hlDashboard != null) hlDashboard.Visible = isAdmin;

                if (hlLogin != null) hlLogin.Visible = !isLoggedIn;
                if (hlRegister != null) hlRegister.Visible = !isLoggedIn;
                if (hlLogout != null) hlLogout.Visible = isLoggedIn;
            }
        }
    }
}
