using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace technova.Admin
{
    public partial class Dashboard : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["TechNovaDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsAdmin"] == null || !(bool)Session["IsAdmin"])
                Response.Redirect("~/Login.aspx");

            if (!IsPostBack)
            {
                LoadStats();
                LoadRecentOrders();
            }
        }

        private void LoadStats()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("sp_GetDashboardStats", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lblProducts.Text = reader["TotalProducts"].ToString();
                    lblUsers.Text = reader["TotalUsers"].ToString();
                    lblOrders.Text = reader["TotalOrders"].ToString();

                    decimal revenue = reader["TotalRevenue"] != DBNull.Value
                        ? Convert.ToDecimal(reader["TotalRevenue"])
                        : 0;

                    lblRevenue.Text = $"RM {revenue:0.00}";
                }
            }
        }

        private void LoadRecentOrders()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("sp_GetRecentOrders", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                gvRecentOrders.DataSource = reader;
                gvRecentOrders.DataBind();
            }
        }

        protected void btnExportCSV_Click(object sender, EventArgs e)
        {
            string csv = "OrderID,Username,OrderDate,Total\n";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
            SELECT TOP 5 O.OrderID, U.Username, O.OrderDate,
                SUM(OI.Quantity * OI.UnitPrice) AS Total
            FROM Orders O
            JOIN Users U ON O.UserID = U.UserID
            JOIN OrderItems OI ON O.OrderID = OI.OrderID
            GROUP BY O.OrderID, U.Username, O.OrderDate
            ORDER BY O.OrderDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string line = $"{reader["OrderID"]},{reader["Username"]},{Convert.ToDateTime(reader["OrderDate"]):yyyy-MM-dd},{Convert.ToDecimal(reader["Total"]):0.00}";
                    csv += line + "\n";
                }
            }

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment;filename=RecentOrders.csv");
            Response.Write(csv);
            Response.End();
        }
    }
}