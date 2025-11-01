using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BookSmart.Admin
{
    public partial class Reports : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            var role = Request.Cookies["Role"]?.Value;
            if (!User.Identity.IsAuthenticated || string.IsNullOrEmpty(role) || role.ToLower() != "admin")
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadSummaryStats();
                LoadTopBooks();
                LoadMonthlySales();
                LoadReviews();
            }
        }

        private void LoadSummaryStats()
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();

                SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM Books", conn);
                lblBooks.Text = cmd1.ExecuteScalar().ToString();

                SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM Users", conn);
                lblUsers.Text = cmd2.ExecuteScalar().ToString();

                SqlCommand cmd3 = new SqlCommand("SELECT COUNT(*) FROM Orders", conn);
                lblOrders.Text = cmd3.ExecuteScalar().ToString();

                SqlCommand cmd4 = new SqlCommand("SELECT SUM(TotalAmount) FROM Orders", conn);
                object total = cmd4.ExecuteScalar();
                lblRevenue.Text = total != DBNull.Value ? string.Format("${0:N2}", total) : "$0.00";
            }
        }

        private void LoadTopBooks()
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = @"SELECT TOP 5 b.Title, SUM(od.Quantity) AS TotalSold
                                 FROM OrderDetails od
                                 JOIN Books b ON od.BookID = b.ID
                                 GROUP BY b.Title
                                 ORDER BY TotalSold DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvTopBooks.DataSource = dt;
                gvTopBooks.DataBind();
            }
        }

        private void LoadMonthlySales()
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = @"SELECT FORMAT(OrderDate, 'MMM yyyy') AS [Month], 
                                SUM(TotalAmount) AS Revenue
                         FROM Orders
                         GROUP BY FORMAT(OrderDate, 'MMM yyyy')
                         ORDER BY MIN(OrderDate)";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvMonthlyRevenue.DataSource = dt;
                gvMonthlyRevenue.DataBind();
            }
        }
        private void LoadReviews()
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = @"
            SELECT b.Title, u.FullName, r.Rating, r.ReviewText, r.ReviewDate
            FROM Reviews r
            INNER JOIN Books b ON r.BookID = b.ID
            INNER JOIN Users u ON r.UserID = u.ID
            ORDER BY r.ReviewDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                var dt = new System.Data.DataTable();
                dt.Load(dr);

                gvReviews.DataSource = dt;
                gvReviews.DataBind();
            }
        }
    }
}
