using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace BookSmart.Account
{
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (!Regex.IsMatch(email, @"^[\w\.\-]+@([\w\-]+\.)+[a-zA-Z]{2,4}$"))
            {
                lblMessage.Text = "Invalid email format.";
                return;
            }

            string password = txtPassword.Text.Trim();
            string country = ddlCountry.SelectedValue;
            string address1 = txtAddress1.Text.Trim();
            string address2 = txtAddress2.Text.Trim();
            string city = txtCity.Text.Trim();
            string state = txtState.Text.Trim();
            string postal = txtPostal.Text.Trim();
            DateTime dateCreated = DateTime.Now;
            string role = "Customer"; // default

            string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = @"INSERT INTO Users 
                (FullName, Email, PasswordHash, Country, DateCreated, AddressLine1, AddressLine2, City, State, PostalCode, Role)
                VALUES 
                (@Name, @Email, @Password, @Country, @DateCreated, @Address1, @Address2, @City, @State, @Postal, @Role)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Country", country);
                cmd.Parameters.AddWithValue("@DateCreated", dateCreated);
                cmd.Parameters.AddWithValue("@Address1", address1);
                cmd.Parameters.AddWithValue("@Address2", address2);
                cmd.Parameters.AddWithValue("@City", city);
                cmd.Parameters.AddWithValue("@State", state);
                cmd.Parameters.AddWithValue("@Postal", postal);
                cmd.Parameters.AddWithValue("@Role", role);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Response.Redirect("~/Account/Login.aspx");
        }
    }
}
