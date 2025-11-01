using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using BookSmart.Class;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace BookSmart.User
{
    public partial class BrowseBooks : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();

                string categoryFromUrl = Request.QueryString["category"];
                if (!string.IsNullOrEmpty(categoryFromUrl))
                {
                    // Pre-select the dropdown
                    ddlCategory.SelectedValue = categoryFromUrl;

                    // Load books filtered by this category
                    LoadBooks(categoryFromUrl, txtSearch.Text.Trim());
                }
                else
                {
                    LoadBooks();
                }
            }
        }

        private void LoadCategories()
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = "SELECT DISTINCT Category FROM Books";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ddlCategory.Items.Add(dr["Category"].ToString());
                }
            }
        }

        private void LoadBooks(string category = "", string search = "")
        {
            string query = "SELECT * FROM Books WHERE 1=1";

            if (!string.IsNullOrEmpty(category))
            {
                query += " AND Category = @Category";
            }

            if (!string.IsNullOrEmpty(search))
            {
                query += " AND (Title LIKE @Search OR Author LIKE @Search)";
            }

            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                if (!string.IsNullOrEmpty(category))
                {
                    cmd.Parameters.AddWithValue("@Category", category);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    cmd.Parameters.AddWithValue("@Search", "%" + search + "%");
                }

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(dr);

                gvBooks.DataSource = dt;
                gvBooks.DataBind();
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBooks(ddlCategory.SelectedValue, txtSearch.Text.Trim());
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadBooks(ddlCategory.SelectedValue, txtSearch.Text.Trim());
        }

        protected void gvBooks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int bookId = Convert.ToInt32(gvBooks.DataKeys[index].Value);
            string email = HttpContext.Current.User.Identity.Name;

            int userId = GetUserIdByEmail(email);
            if (userId > 0)
            {
                if (e.CommandName == "AddToCart")
                {
                    using (SqlConnection conn = new SqlConnection(cs))
                    {
                        string query = "INSERT INTO Cart (UserID, BookID, Quantity, DateAdded) VALUES (@UserID, @BookID, 1, GETDATE())";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@BookID", bookId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    Response.Write("<script>alert('Book added to cart successfully!');</script>");
                }
                else if (e.CommandName == "AddToWishlist")
                {
                    using (SqlConnection conn = new SqlConnection(cs))
                    {
                        string query = @"INSERT INTO Wishlist (UserID, BookID, DateAdded)
                                 VALUES (@UserID, @BookID, GETDATE())";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@BookID", bookId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    Response.Write("<script>alert('Book added to wishlist successfully!');</script>");
                }
            }
            else
            {
                Response.Redirect("~/Account/Login.aspx");
            }
        }


        private int GetUserIdByEmail(string email)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = "SELECT ID FROM Users WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }
    }
}
