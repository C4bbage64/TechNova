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
        string connStr = ConfigurationManager.ConnectionStrings["TechNovaDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
                LoadOrderHistory();
        }

        private void LoadOrderHistory()
        {
            int userId = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
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
                cmd.Parameters.AddWithValue("@UserID", userId);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    gvOrders.DataSource = reader;
                    gvOrders.DataBind();
                }
                else
                {
                    lblMessage.Text = "You have no orders yet.";
                }
            }
        }
    }
}