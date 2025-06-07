using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace technova
{
    public partial class OrderHistory : System.Web.UI.Page
    {
        // Fetch the database connection string from Web.config
        string connStr = ConfigurationManager.ConnectionStrings["TechNovaDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // If user is not logged in, redirect to login page
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // Load order history only on initial load, not on postbacks
            if (!IsPostBack)
                LoadOrderHistory();
        }

        // Loads the current user's order history from the database
        private void LoadOrderHistory()
        {
            // Get the currently logged-in user's ID from session
            int userId = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // SQL query to fetch order details for the user
                string query = @"
                    SELECT O.OrderID, O.OrderDate, P.ProductName, 
                           OI.Quantity, OI.UnitPrice, 
                           (OI.Quantity * OI.UnitPrice) AS Total,
                           O.OrderStatus
                    FROM Orders O
                    JOIN OrderItems OI ON O.OrderID = OI.OrderID
                    JOIN Products P ON OI.ProductID = P.ProductID
                    WHERE O.UserID = @UserID
                    ORDER BY O.OrderDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId); // Parameterized query to prevent SQL injection

                conn.Open(); // Open the database connection
                SqlDataReader reader = cmd.ExecuteReader(); // Execute the query

                if (reader.HasRows)
                {
                    // If there are results, bind them to the GridView
                    gvOrders.DataSource = reader;
                    gvOrders.DataBind();
                }
                else
                {
                    // Show message if no orders are found
                    lblMessage.Text = "You have no orders yet.";
                }
            }
        }
    }
}
