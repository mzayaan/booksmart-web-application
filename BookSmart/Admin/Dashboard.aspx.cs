using System;
using System.Configuration;
using System.Data.SqlClient;

namespace BookSmart.Admin
{
    public partial class Dashboard : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            string role = Request.Cookies["Role"]?.Value;
            if (!User.Identity.IsAuthenticated || role?.ToLower() != "admin")
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadStats();
            }
        }

        private void LoadStats()
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();

                // 📚 Total Books
                SqlCommand cmdBooks = new SqlCommand("SELECT COUNT(*) FROM Books", conn);
                lblBooks.Text = cmdBooks.ExecuteScalar().ToString();

                // 👤 Total Users
                SqlCommand cmdUsers = new SqlCommand("SELECT COUNT(*) FROM Users", conn);
                lblUsers.Text = cmdUsers.ExecuteScalar().ToString();

                // 📦 Total Orders
                SqlCommand cmdOrders = new SqlCommand("SELECT COUNT(*) FROM Orders", conn);
                lblOrders.Text = cmdOrders.ExecuteScalar().ToString();

                // 💵 Total Revenue
                SqlCommand cmdRevenue = new SqlCommand("SELECT ISNULL(SUM(TotalAmount), 0) FROM Orders", conn);
                decimal revenue = Convert.ToDecimal(cmdRevenue.ExecuteScalar());
                lblRevenue.Text = "$" + revenue.ToString("N2");
            }
        }
        protected void btnViewReports_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/Reports.aspx");
        }

    }
}
