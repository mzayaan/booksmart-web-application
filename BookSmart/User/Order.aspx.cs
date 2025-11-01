using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using BookSmart.Class;

namespace BookSmart.User
{
    public partial class Order : System.Web.UI.Page
    {
        private int userId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["uid"] != null)
                {
                    userId = Convert.ToInt32(Request.QueryString["uid"]);
                }
                else
                {
                    lblMessage.Text = "Missing User ID.";
                    btnPlaceOrder.Enabled = false;
                }
            }
        }

        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["uid"] == null)
            {
                lblMessage.Text = "User ID not found.";
                return;
            }

            userId = Convert.ToInt32(Request.QueryString["uid"]);
            string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;
            List<ClsCart> cartItems = new List<ClsCart>();
            Dictionary<int, decimal> bookPrices = new Dictionary<int, decimal>();
            decimal total = 0;

            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();

                // 1. Fetch cart items
                SqlCommand cmdCart = new SqlCommand("SELECT * FROM Cart WHERE UserID = @UserID", conn);
                cmdCart.Parameters.AddWithValue("@UserID", userId);
                SqlDataReader dr = cmdCart.ExecuteReader();
                while (dr.Read())
                {
                    cartItems.Add(new ClsCart
                    {
                        BookID = Convert.ToInt32(dr["BookID"]),
                        Quantity = Convert.ToInt32(dr["Quantity"])
                    });
                }
                dr.Close();

                if (cartItems.Count == 0)
                {
                    lblMessage.Text = "Cart is empty.";
                    return;
                }

                // 2. Fetch prices from Books table
                foreach (var item in cartItems)
                {
                    SqlCommand priceCmd = new SqlCommand("SELECT Price FROM Books WHERE ID = @BookID", conn);
                    priceCmd.Parameters.AddWithValue("@BookID", item.BookID);
                    object result = priceCmd.ExecuteScalar();

                    if (result != null)
                    {
                        decimal price = Convert.ToDecimal(result);
                        bookPrices[item.BookID] = price;
                        total += price * item.Quantity;
                    }
                    else
                    {
                        lblMessage.Text = $"Book with ID {item.BookID} not found.";
                        return;
                    }
                }

                // 3. Insert into Orders
                SqlCommand cmdOrder = new SqlCommand(
                    "INSERT INTO Orders (UserID, OrderDate, TotalAmount) OUTPUT INSERTED.ID VALUES (@UserID, GETDATE(), @Total)", conn);
                cmdOrder.Parameters.AddWithValue("@UserID", userId);
                cmdOrder.Parameters.AddWithValue("@Total", total);
                int orderId = (int)cmdOrder.ExecuteScalar();

                // 4. Insert OrderDetails
                foreach (var item in cartItems)
                {
                    // Insert into OrderDetails
                    SqlCommand cmdDetail = new SqlCommand(
                        "INSERT INTO OrderDetails (OrderID, BookID, Quantity, UnitPrice) VALUES (@OrderID, @BookID, @Qty, @Price)", conn);
                    cmdDetail.Parameters.AddWithValue("@OrderID", orderId);
                    cmdDetail.Parameters.AddWithValue("@BookID", item.BookID);
                    cmdDetail.Parameters.AddWithValue("@Qty", item.Quantity);
                    cmdDetail.Parameters.AddWithValue("@Price", bookPrices[item.BookID]);
                    cmdDetail.ExecuteNonQuery();

                    // ✅ Decrement Book Stock
                    SqlCommand cmdStock = new SqlCommand(
                        "UPDATE Books SET Stock = Stock - @Qty WHERE ID = @BookID", conn);
                    cmdStock.Parameters.AddWithValue("@Qty", item.Quantity);
                    cmdStock.Parameters.AddWithValue("@BookID", item.BookID);
                    cmdStock.ExecuteNonQuery();


                    // 5. Clear Cart
                    SqlCommand cmdClear = new SqlCommand("DELETE FROM Cart WHERE UserID = @UserID", conn);
                    cmdClear.Parameters.AddWithValue("@UserID", userId);
                    cmdClear.ExecuteNonQuery();

                    // ✅ Redirect to confirmation
                    Response.Redirect($"~/User/OrderConfirmation.aspx?orderId={orderId}");
                }
            }
        }
    }
}

