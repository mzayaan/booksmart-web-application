using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization; 
using System.Web.UI.WebControls;

namespace BookSmart.Admin
{
    public partial class Admin : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            var role = Request.Cookies["Role"] != null ? Request.Cookies["Role"].Value : "";
            if (!User.Identity.IsAuthenticated || string.IsNullOrEmpty(role) || role.ToLower() != "admin")
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
                BindGrid();
        }

        private void BindGrid(string filter = "")
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Books";

                if (!string.IsNullOrEmpty(filter))
                {
                    query += " WHERE Title LIKE @filter OR Category LIKE @filter";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                if (!string.IsNullOrEmpty(filter))
                {
                    cmd.Parameters.AddWithValue("@filter", "%" + filter + "%");
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgBooks.DataSource = dt;
                dgBooks.DataKeyField = "ID";
                dgBooks.DataBind();

                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToInt32(row["Stock"]) <= 5)
                    {
                        lblStatus.Text = "⚠️ Warning: Some books have low stock (5 or fewer).";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                }

                lblStatus.Text = "";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            BindGrid(keyword);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            BindGrid();
        }

        protected void dgBooks_EditCommand(object source, DataGridCommandEventArgs e)
        {
            dgBooks.EditItemIndex = e.Item.ItemIndex;
            BindGrid(txtSearch.Text.Trim());
        }

        protected void dgBooks_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            dgBooks.EditItemIndex = -1;
            BindGrid(txtSearch.Text.Trim());
        }

        protected void dgBooks_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int id = Convert.ToInt32(dgBooks.DataKeys[e.Item.ItemIndex]);
            string title = ((TextBox)e.Item.Cells[1].Controls[0]).Text;
            string author = ((TextBox)e.Item.Cells[2].Controls[0]).Text;
            string category = ((TextBox)e.Item.Cells[3].Controls[0]).Text;

            string priceText = ((TextBox)e.Item.Cells[4].Controls[0]).Text.Trim();
            string stockText = ((TextBox)e.Item.Cells[5].Controls[0]).Text.Trim();

            if (!decimal.TryParse(priceText, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal price)
                || !int.TryParse(stockText, out int stock))
            {
                lblStatus.Text = "❌ Invalid format. Enter valid decimal price and whole number stock.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = @"UPDATE Books SET Title = @Title, Author = @Author, Category = @Category,
                                 Price = @Price, Stock = @Stock WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Author", author);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Stock", stock);
                cmd.Parameters.AddWithValue("@ID", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            dgBooks.EditItemIndex = -1;
            lblStatus.Text = "✅ Book updated successfully.";
            lblStatus.ForeColor = System.Drawing.Color.Green;
            BindGrid(txtSearch.Text.Trim());
        }

        protected void dgBooks_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = Convert.ToInt32(dgBooks.DataKeys[e.Item.ItemIndex]);

            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = "DELETE FROM Books WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            lblStatus.Text = "🗑️ Book deleted.";
            lblStatus.ForeColor = System.Drawing.Color.DarkRed;
            BindGrid(txtSearch.Text.Trim());
        }

        protected void btnAddBook_Click(object sender, EventArgs e)
        {
            string title = txtNewTitle.Text.Trim();
            string author = txtNewAuthor.Text.Trim();
            string category = txtNewCategory.Text.Trim();
            string priceText = txtNewPrice.Text.Trim();
            string stockText = txtNewStock.Text.Trim();

            if (!decimal.TryParse(priceText, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal price)
                || !int.TryParse(stockText, out int stock))
            {
                lblStatus.Text = "❌ Invalid input. Price must be a decimal and stock must be a whole number.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"INSERT INTO Books (Title, Author, Category, Price, Stock)
                                                  VALUES (@Title, @Author, @Category, @Price, @Stock)", conn);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Author", author);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Stock", stock);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            lblStatus.Text = "✅ Book added successfully.";
            lblStatus.ForeColor = System.Drawing.Color.DarkGreen;

            ClearInputs();
            BindGrid(txtSearch.Text.Trim());
        }

        private void ClearInputs()
        {
            txtNewTitle.Text = "";
            txtNewAuthor.Text = "";
            txtNewCategory.Text = "";
            txtNewPrice.Text = "";
            txtNewStock.Text = "";
        }
    }
}
