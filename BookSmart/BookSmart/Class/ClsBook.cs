using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

namespace BookSmart.Class
{
    public class ClsBook
    {
        public DateTime DateCreated { get; set; }

        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }

        public static List<ClsBook> GetAllBooks()
        {
            List<ClsBook> books = new List<ClsBook>();
            string cs = ConfigurationManager.ConnectionStrings["BookSmartDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = "SELECT ID, Title, Author, Category, Price FROM Books";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    books.Add(new ClsBook
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Title = dr["Title"].ToString(),
                        Author = dr["Author"].ToString(),
                        Category = dr["Category"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"])
                    });
                }
            }

            return books;
        }
    }
}
