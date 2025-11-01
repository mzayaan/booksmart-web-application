using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace BookSmart.User
{
    public partial class Cart : System.Web.UI.Page
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
                LoadCart();
            }
        }

        private void LoadCart()
        {
            string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;
            int userId = GetUserIdByEmail(User.Identity.Name);

            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = @"SELECT c.ID, b.Title, b.Price, c.Quantity, (b.Price * c.Quantity) AS Subtotal
                                 FROM Cart c 
                                 JOIN Books b ON c.BookID = b.ID 
                                 WHERE c.UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCart.DataSource = dt;
                gvCart.DataBind();

                decimal total = 0;
                foreach (DataRow row in dt.Rows)
                    total += Convert.ToDecimal(row["Subtotal"]);
                lblTotal.Text = "Total: " + total.ToString("C");
            }
        }

        
        protected void gvCart_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCart.EditIndex = e.NewEditIndex;
            LoadCart();
        }

        
        protected void gvCart_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvCart.EditIndex = -1;
            LoadCart();
        }

        
        protected void gvCart_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int cartId = Convert.ToInt32(gvCart.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvCart.Rows[e.RowIndex];
            TextBox txtQty = (TextBox)row.FindControl("txtQty");
            int newQty = Convert.ToInt32(txtQty.Text);

            string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Cart SET Quantity = @Quantity WHERE ID = @ID", conn);
                cmd.Parameters.AddWithValue("@Quantity", newQty);
                cmd.Parameters.AddWithValue("@ID", cartId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            gvCart.EditIndex = -1;
            LoadCart();
        }

        protected void gvCart_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int cartId = Convert.ToInt32(gvCart.DataKeys[e.RowIndex].Value);

            string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Cart WHERE ID = @ID", conn);
                cmd.Parameters.AddWithValue("@ID", cartId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadCart();
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/User/OrderConfirmation.aspx");
        }
        
        protected void btnProceed_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/User/Payment.aspx");
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
