using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace BookSmart.User
{
    public partial class OrderHistory : System.Web.UI.Page
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
                LoadOrders();
            }
        }

        private void LoadOrders()
        {
            int userId = GetUserIdByEmail(User.Identity.Name);
            string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = @"SELECT ID AS OrderID, OrderDate, TotalAmount, 'Paid' AS Status
                                 FROM Orders
                                 WHERE UserID = @UserID
                                 ORDER BY OrderDate DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);

                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvOrders.DataSource = dt;
                gvOrders.DataBind();
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

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected void btnBrowse_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/User/BrowseBooks.aspx");
        }

        protected void btnResetOrders_Click(object sender, EventArgs e)
        {
            int userId = GetUserIdByEmail(User.Identity.Name);
            string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();

                SqlCommand cmdLog = new SqlCommand("INSERT INTO OrderResetLogs (UserID, ResetTime) VALUES (@UserID, GETDATE())", conn);
                cmdLog.Parameters.AddWithValue("@UserID", userId);
                cmdLog.ExecuteNonQuery();

                // Step 1: Delete from Payments (because it references Orders)
                SqlCommand cmdPayments = new SqlCommand(
                    "DELETE FROM Payments WHERE OrderID IN (SELECT ID FROM Orders WHERE UserID = @UserID)", conn);
                cmdPayments.Parameters.AddWithValue("@UserID", userId);
                cmdPayments.ExecuteNonQuery();

                // Step 2: Delete from OrderDetails
                SqlCommand cmdDetails = new SqlCommand(
                    "DELETE FROM OrderDetails WHERE OrderID IN (SELECT ID FROM Orders WHERE UserID = @UserID)", conn);
                cmdDetails.Parameters.AddWithValue("@UserID", userId);
                cmdDetails.ExecuteNonQuery();

                // Step 3: Delete from Orders
                SqlCommand cmdOrders = new SqlCommand(
                    "DELETE FROM Orders WHERE UserID = @UserID", conn);
                cmdOrders.Parameters.AddWithValue("@UserID", userId);
                cmdOrders.ExecuteNonQuery();
            }

            lblStatus.Text = "✅ Order history has been reset successfully.";
            lblStatus.CssClass = "alert alert-success d-block mt-3";
            LoadOrders();
        }

        protected void btnDownloadPDF_Click(object sender, EventArgs e)
        {
            int userId = GetUserIdByEmail(User.Identity.Name);
            string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = @"SELECT ID AS OrderID, OrderDate, TotalAmount
                         FROM Orders
                         WHERE UserID = @UserID AND OrderDate >= DATEADD(MONTH, -3, GETDATE())
                         ORDER BY OrderDate DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            if (dt.Rows.Count == 0)
            {
                lblStatus.Text = "❌ No orders in the last 3 months.";
                lblStatus.CssClass = "alert alert-warning d-block";
                return;
            }

            // Create styled PDF
            Document pdfDoc = new Document(PageSize.A4, 40f, 40f, 60f, 50f);
            MemoryStream ms = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, ms);
            writer.CloseStream = false;

            pdfDoc.Open();

            // Title
            var titleFont = FontFactory.GetFont("Arial", 20, Font.BOLD, BaseColor.DARK_GRAY);
            var subFont = FontFactory.GetFont("Arial", 12, Font.ITALIC, BaseColor.GRAY);
            var cellFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);

            Paragraph header = new Paragraph("📚 BookSmart - Order History Report", titleFont);
            header.Alignment = Element.ALIGN_CENTER;
            pdfDoc.Add(header);

            Paragraph timestamp = new Paragraph("Generated on: " + DateTime.Now.ToString("dd MMM yyyy HH:mm"), subFont);
            timestamp.Alignment = Element.ALIGN_CENTER;
            timestamp.SpacingAfter = 20f;
            pdfDoc.Add(timestamp);

            // Table
            PdfPTable table = new PdfPTable(3);
            table.WidthPercentage = 100;
            table.SpacingBefore = 10f;
            table.SetWidths(new float[] { 1.2f, 2f, 2f });

            // Header row
            string[] headers = { "Order ID", "Order Date", "Total Amount" };
            foreach (var h in headers)
            {
                PdfPCell headerCell = new PdfPCell(new Phrase(h, FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.WHITE)))
                {
                    BackgroundColor = BaseColor.DARK_GRAY,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                };
                table.AddCell(headerCell);
            }

            // Data rows
            foreach (DataRow row in dt.Rows)
            {
                table.AddCell(new PdfPCell(new Phrase(row["OrderID"].ToString(), cellFont)));
                table.AddCell(new PdfPCell(new Phrase(Convert.ToDateTime(row["OrderDate"]).ToString("dd MMM yyyy"), cellFont)));
                table.AddCell(string.Format("{0:C}", row["TotalAmount"]));
            }

            pdfDoc.Add(table);

            // Footer
            Paragraph footer = new Paragraph("\nReport generated by BookSmart Online System © " + DateTime.Now.Year, subFont);
            footer.Alignment = Element.ALIGN_CENTER;
            pdfDoc.Add(footer);

            pdfDoc.Close();

            // Serve to browser
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=OrderHistory.pdf");
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.End();
        }


    }
}
