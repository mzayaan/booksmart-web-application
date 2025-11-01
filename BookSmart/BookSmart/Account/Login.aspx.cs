using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.UI;

namespace BookSmart.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["msg"] == "reset")
                {
                    lblMessage.CssClass = "text-success";
                    lblMessage.Text = "✅ Your password has been reset. Please log in.";
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (!Regex.IsMatch(email, @"^[\w\.\-]+@([\w\-]+\.)+[a-zA-Z]{2,4}$"))
            {
                lblMessage.Text = "Invalid email format.";
                return;
            }

            string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = "SELECT ID, FullName, PasswordHash, Role FROM Users WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    string dbPassword = dr["PasswordHash"].ToString();
                    string role = dr["Role"].ToString();

                    if (password == dbPassword)
                    {
                        // ✅ Set authentication cookie
                        FormsAuthentication.SetAuthCookie(email, false);

                        // ✅ Set role cookie
                        Response.Cookies["Role"].Value = role;
                        Response.Cookies["Role"].Expires = DateTime.Now.AddHours(1);

                        // ✅ Set user ID cookie (optional)
                        Response.Cookies["UserID"].Value = dr["ID"].ToString();
                        Response.Cookies["UserID"].Expires = DateTime.Now.AddHours(1);

                        // ✅ Redirect
                        if (role.ToLower() == "admin")
                            Response.Redirect("~/Admin/Dashboard.aspx");
                        else
                            Response.Redirect("~/Default.aspx");
                    }
                    else
                    {
                        lblMessage.Text = "Invalid password.";
                    }
                }
                else
                {
                    lblMessage.Text = "Email not found.";
                }
            }
        }
    }
}
