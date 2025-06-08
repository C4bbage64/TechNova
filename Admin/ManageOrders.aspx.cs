using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace technova.Admin
{
    public partial class ManageOrders : System.Web.UI.Page
    {
        // Get connection string from Web.config
        string connStr = ConfigurationManager.ConnectionStrings["TechNovaDB"].ConnectionString;

        // Runs on every page load
        protected void Page_Load(object sender, EventArgs e)
        {
            // Redirect non-admin users
            if (Session["IsAdmin"] == null || !(bool)Session["IsAdmin"])
                Response.Redirect("~/Login.aspx");

            // Only load data on first load (not on postback events)
            if (!IsPostBack)
                LoadOrders();
        }

        // Load orders into GridView with order totals and statuses
        private void LoadOrders()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // SQL to fetch order details with total amount
                string query = @"
                    SELECT O.OrderID, U.Username, O.OrderDate,
                           SUM(OI.Quantity * OI.UnitPrice) AS TotalAmount,
                           O.OrderStatus
                    FROM Orders O
                    JOIN Users U ON O.UserID = U.UserID
                    JOIN OrderItems OI ON O.OrderID = OI.OrderID
                    GROUP BY O.OrderID, U.Username, O.OrderDate, O.OrderStatus
                    ORDER BY O.OrderDate DESC";

                // Fill data into DataTable and bind to GridView
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvOrders.DataSource = dt;
                gvOrders.DataBind();
            }
        }

        // Enables row editing in the GridView
        protected void gvOrders_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvOrders.EditIndex = e.NewEditIndex;
            LoadOrders(); // Reload to reflect edit state
        }

        // Cancels editing mode and rebinds GridView
        protected void gvOrders_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvOrders.EditIndex = -1; // Cancel editing
            LoadOrders(); // Refresh data
        }

        // Updates the OrderStatus field in the database
        protected void gvOrders_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Get OrderID from DataKey
            int orderId = Convert.ToInt32(gvOrders.DataKeys[e.RowIndex].Value);

            // Get the current row
            GridViewRow row = gvOrders.Rows[e.RowIndex];

            // Find the dropdown list inside the GridView row
            DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatus");
            string newStatus = ddlStatus.SelectedValue;

            // Update the order status in database
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Orders SET OrderStatus = @Status WHERE OrderID = @OrderID", conn);
                cmd.Parameters.AddWithValue("@Status", newStatus);
                cmd.Parameters.AddWithValue("@OrderID", orderId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Reset edit index and reload data
            gvOrders.EditIndex = -1;
            lblMessage.Text = $"Order #{orderId} status updated to {newStatus}.";
            LoadOrders();
        }
    }
}
