using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace BookSmart.User
{
    public partial class Reviews : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBooks();
                LoadReviews();
            }
        }

        private void LoadBooks()
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("SELECT ID, Title FROM Books", conn);
                conn.Open();
                ddlBooks.DataSource = cmd.ExecuteReader();
                ddlBooks.DataTextField = "Title";
                ddlBooks.DataValueField = "ID";
                ddlBooks.DataBind();
            }
        }

        private void LoadReviews()
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = @"SELECT u.FullName, r.Rating, r.ReviewText, r.ReviewDate
                                 FROM Reviews r
                                 INNER JOIN Users u ON r.UserID = u.ID
                                 WHERE r.BookID = @BookID
                                 ORDER BY r.ReviewDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@BookID", ddlBooks.SelectedValue);
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvReviews.DataSource = dt;
                gvReviews.DataBind();
            }
        }

        protected void btnSubmitReview_Click(object sender, EventArgs e)
        {
            string email = HttpContext.Current.User.Identity.Name;
            int userId = GetUserIdByEmail(email);
            int bookId = Convert.ToInt32(ddlBooks.SelectedValue);
            int rating = Convert.ToInt32(ddlRating.SelectedValue);
            string reviewText = txtReview.Text.Trim();

            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = @"INSERT INTO Reviews (UserID, BookID, Rating, ReviewText)
                                 VALUES (@UserID, @BookID, @Rating, @ReviewText)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@BookID", bookId);
                cmd.Parameters.AddWithValue("@Rating", rating);
                cmd.Parameters.AddWithValue("@ReviewText", reviewText);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            txtReview.Text = "";
            LoadReviews();
        }

        protected void ddlBooks_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReviews();
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
