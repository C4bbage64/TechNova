using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace technova
{
	public partial class Cart : System.Web.UI.Page
	{
        string connStr = ConfigurationManager.ConnectionStrings["TechNovaDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadCart();
        }

        private void LoadCart()
        {
            var cart = Session["Cart"] as List<CartItem>;

            if (cart == null || cart.Count == 0)
            {
                gvCart.DataSource = null;
                gvCart.DataBind();
                lblTotal.Text = "Your cart is empty.";
                lblTotal.ForeColor = System.Drawing.Color.Red;
                btnCheckout.Enabled = false;
                return;
            }

            gvCart.DataSource = cart;
            gvCart.DataBind();

            decimal total = 0;
            foreach (var item in cart)
                total += item.Total;

            lblTotal.Text = $"Grand Total: RM {total:0.00}";
            lblTotal.ForeColor = System.Drawing.Color.Black;
            btnCheckout.Enabled = true;
        }

        protected void gvCart_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "RemoveItem")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                var cart = Session["Cart"] as List<CartItem>;

                if (cart != null && index >= 0 && index < cart.Count)
                {
                    cart.RemoveAt(index);
                    Session["Cart"] = cart;
                    LoadCart(); // Rebind and refresh total
                }
            }
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null || cart.Count == 0)
                return;

            int userId = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Insert order
                SqlCommand orderCmd = new SqlCommand(
                    "INSERT INTO Orders (UserID, OrderDate) OUTPUT INSERTED.OrderID VALUES (@UserID, GETDATE())", conn);
                orderCmd.Parameters.AddWithValue("@UserID", userId);
                int orderId = (int)orderCmd.ExecuteScalar();

                // Insert each item
                foreach (var item in cart)
                {
                    SqlCommand itemCmd = new SqlCommand(
                        "INSERT INTO OrderItems (OrderID, ProductID, Quantity, UnitPrice) " +
                        "VALUES (@OrderID, @ProductID, @Qty, @Price)", conn);

                    itemCmd.Parameters.AddWithValue("@OrderID", orderId);
                    itemCmd.Parameters.AddWithValue("@ProductID", item.ProductID);
                    itemCmd.Parameters.AddWithValue("@Qty", item.Quantity);
                    itemCmd.Parameters.AddWithValue("@Price", item.Price);

                    itemCmd.ExecuteNonQuery();
                }
            }

            // Clear cart and redirect
            Session["Cart"] = null;
            Response.Redirect("OrderHistory.aspx");
        }
    }
}