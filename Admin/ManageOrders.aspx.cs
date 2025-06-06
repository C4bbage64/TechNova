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
        string connStr = ConfigurationManager.ConnectionStrings["TechNovaDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsAdmin"] == null || !(bool)Session["IsAdmin"])
                Response.Redirect("~/Login.aspx");

            if (!IsPostBack)
                LoadOrders();
        }

        private void LoadOrders()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT O.OrderID, U.Username, O.OrderDate,
                        SUM(OI.Quantity * OI.UnitPrice) AS TotalAmount,
                        O.OrderStatus
                    FROM Orders O
                    JOIN Users U ON O.UserID = U.UserID
                    JOIN OrderItems OI ON O.OrderID = OI.OrderID
                    GROUP BY O.OrderID, U.Username, O.OrderDate, O.OrderStatus
                    ORDER BY O.OrderDate DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvOrders.DataSource = dt;
                gvOrders.DataBind();
            }
        }

        protected void gvOrders_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Correcting the issue by using the 'NewEditIndex' property of GridViewEditEventArgs  
            gvOrders.EditIndex = e.NewEditIndex;
            LoadOrders();
        }

        protected void gvOrders_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvOrders.EditIndex = -1;
            LoadOrders();
        }

        protected void gvOrders_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int orderId = Convert.ToInt32(gvOrders.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvOrders.Rows[e.RowIndex];
            DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatus");
            string newStatus = ddlStatus.SelectedValue;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Orders SET OrderStatus = @Status WHERE OrderID = @OrderID", conn);
                cmd.Parameters.AddWithValue("@Status", newStatus);
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            gvOrders.EditIndex = -1;
            lblMessage.Text = $"Order #{orderId} status updated to {newStatus}.";
            LoadOrders();
        }
    }
}