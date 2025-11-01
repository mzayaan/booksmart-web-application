using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace BookSmart.Account
{
    public partial class ForgotPassword : Page
    {
        protected void btnRecover_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();

            if (!Regex.IsMatch(email, @"^[\w\.\-]+@([\w\-]+\.)+[a-zA-Z]{2,4}$"))
            {
                lblResult.Text = "❌ Invalid email format.";
                return;
            }

            string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;

            if (string.IsNullOrEmpty(email))
            {
                lblResult.Text = "❌ Please enter your email address.";
                return;
            }

            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                conn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count > 0)
                {
                    // User exists → show reset panel
                    resetPanel.Visible = true;
                    btnReset.Visible = true;
                    btnRecover.Visible = false;
                    lblResult.CssClass = "text-success";
                    lblResult.Text = "✅ User found. Please enter your new password below.";
                }
                else
                {
                    lblResult.CssClass = "text-danger";
                    lblResult.Text = "❌ Email not found.";
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                lblResult.Text = "❌ Please enter and confirm your new password.";
                return;
            }

            if (newPassword != confirmPassword)
            {
                lblResult.Text = "❌ Passwords do not match.";
                return;
            }

            string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = "UPDATE Users SET PasswordHash = @Password WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Password", newPassword);
                cmd.Parameters.AddWithValue("@Email", email);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Response.Redirect("~/Account/Login.aspx?msg=reset");


        }
    }
}
