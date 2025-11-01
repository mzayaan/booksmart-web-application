using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BookSmart.User
{
    public partial class OrderConfirmation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
                LoadCartSummary();
        }

        private void LoadCartSummary()
        {
            int userId = GetUserIdByEmail(User.Identity.Name);
            decimal total = 0;

            string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = @"SELECT b.Title, b.Price, c.Quantity, (b.Price * c.Quantity) AS Subtotal 
                                 FROM Cart c 
                                 JOIN Books b ON c.BookID = b.ID 
                                 WHERE c.UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                    total += Convert.ToDecimal(row["Subtotal"]);

                gvSummary.DataSource = dt;
                gvSummary.DataBind();
            }

            lblTotal.Text = "Total: " + total.ToString("C");
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            int userId = GetUserIdByEmail(User.Identity.Name);
            string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;
            int orderId;

            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    // Insert Order
                    SqlCommand cmd = new SqlCommand("INSERT INTO Orders (UserID, OrderDate, TotalAmount) OUTPUT INSERTED.ID VALUES (@UserID, GETDATE(), @Total)", conn, trans);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@Total", GetCartTotal(userId));
                    orderId = (int)cmd.ExecuteScalar();

                    // Insert OrderDetails
                    SqlCommand cmdItems = new SqlCommand(@"INSERT INTO OrderDetails (OrderID, BookID, Quantity, UnitPrice)
                                                           SELECT @OrderID, BookID, Quantity, b.Price 
                                                           FROM Cart c JOIN Books b ON c.BookID = b.ID 
                                                           WHERE c.UserID = @UserID", conn, trans);
                    cmdItems.Parameters.AddWithValue("@OrderID", orderId);
                    cmdItems.Parameters.AddWithValue("@UserID", userId);
                    cmdItems.ExecuteNonQuery();

                    // Decrease Book Stock
                    SqlCommand cmdStock = new SqlCommand(@"UPDATE Books SET Stock = Stock - c.Quantity 
                                                           FROM Books b JOIN Cart c ON b.ID = c.BookID 
                                                           WHERE c.UserID = @UserID", conn, trans);
                    cmdStock.Parameters.AddWithValue("@UserID", userId);
                    cmdStock.ExecuteNonQuery();

                    // Clear Cart
                    SqlCommand cmdClear = new SqlCommand("DELETE FROM Cart WHERE UserID = @UserID", conn, trans);
                    cmdClear.Parameters.AddWithValue("@UserID", userId);
                    cmdClear.ExecuteNonQuery();

                    trans.Commit();
                    Response.Redirect("~/User/Payment.aspx");
                }
                catch
                {
                    if (trans.Connection != null)
                    {
                        trans.Rollback();
                    }
                    lblTotal.Text = "❌ Order failed. Try again.";
                }

            }
        }

        private decimal GetCartTotal(int userId)
        {
            string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"SELECT SUM(b.Price * c.Quantity) 
                                                  FROM Cart c 
                                                  JOIN Books b ON c.BookID = b.ID 
                                                  WHERE c.UserID = @UserID", conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }

        private int GetUserIdByEmail(string email)
        {
            string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("SELECT ID FROM Users WHERE Email = @Email", conn);
                cmd.Parameters.AddWithValue("@Email", email);
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }
    }
}
