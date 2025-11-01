using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace BookSmart.User
{
    public partial class Wishlist : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadWishlist();
            }
        }

        private void LoadWishlist()
        {
            string email = HttpContext.Current.User.Identity.Name;
            int userId = GetUserIdByEmail(email);

            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = @"
            SELECT w.WishlistID, w.BookID, b.Title, b.Author, b.Price
            FROM Wishlist w
            INNER JOIN Books b ON w.BookID = b.ID
            WHERE w.UserID = @UserID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvWishlist.DataSource = dt;
                gvWishlist.DataBind();
            }
        }


        protected void gvWishlist_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int wishlistId = Convert.ToInt32(gvWishlist.DataKeys[index].Values["WishlistID"]);
            int bookId = Convert.ToInt32(gvWishlist.DataKeys[index].Values["BookID"]);
            string email = HttpContext.Current.User.Identity.Name;
            int userId = GetUserIdByEmail(email);

            if (e.CommandName == "Remove")
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Wishlist WHERE WishlistID = @WishlistID", conn);
                    cmd.Parameters.AddWithValue("@WishlistID", wishlistId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                LoadWishlist();
            }
            else if (e.CommandName == "AddToCart")
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    string query = @"INSERT INTO Cart (UserID, BookID, Quantity, DateAdded)
                             VALUES (@UserID, @BookID, 1, GETDATE())";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@BookID", bookId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                // Optionally remove from wishlist after adding to cart
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Wishlist WHERE WishlistID = @WishlistID", conn);
                    cmd.Parameters.AddWithValue("@WishlistID", wishlistId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                Response.Write("<script>alert('Book added to cart successfully!');</script>");
                LoadWishlist();
            }
        }


        private int GetUserIdByEmail(string email)
        {
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
