using System;
using System.Configuration;
using System.Data.SqlClient;

namespace BookSmart.User
{
    public partial class Payment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                decimal total = GetOrderTotal();
                lblTotal.Text = "Total Amount: " + total.ToString("C");
            }
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            string method = ddlMethod.SelectedValue;
            string status = "Paid"; 

            if (string.IsNullOrEmpty(method))
            {
                lblMsg.Text = "Please select a payment method.";
                return;
            }

            int orderId = GetLatestOrderID();
            if (orderId <= 0)
            {
                lblMsg.Text = "⚠️ Order not found.";
                return;
            }

            string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = @"INSERT INTO Payments (OrderID, AmountPaid, PaymentDate, PaymentMethod, Status)
                         VALUES (@OrderID, @AmountPaid, GETDATE(), @Method, @Status)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                cmd.Parameters.AddWithValue("@AmountPaid", GetOrderTotal());
                cmd.Parameters.AddWithValue("@Method", method);
                cmd.Parameters.AddWithValue("@Status", status);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Response.Redirect("~/User/OrderHistory.aspx");
        }

        private int GetLatestOrderID()
        {
            string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 ID FROM Orders ORDER BY OrderDate DESC", conn);
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }



        private decimal GetOrderTotal()
        {
            string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;
            decimal total = 0;
            int userId = GetUserIdByEmail(User.Identity.Name);

            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 TotalAmount FROM Orders WHERE UserID = @UserID ORDER BY OrderDate DESC", conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                conn.Open();
                object result = cmd.ExecuteScalar();
                total = result != null ? Convert.ToDecimal(result) : 0;
            }

            return total;
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
