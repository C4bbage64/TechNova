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
        // Get connection string from Web.config
        string connStr = ConfigurationManager.ConnectionStrings["TechNovaDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Only load cart data on initial page load, not on postbacks
            if (!IsPostBack)
                LoadCart();
        }

        // Loads cart items from session and binds them to the GridView
        private void LoadCart()
        {
            var cart = Session["Cart"] as List<CartItem>;

            // If cart is empty or null, show appropriate message and disable checkout
            if (cart == null || cart.Count == 0)
            {
                gvCart.DataSource = null;
                gvCart.DataBind();
                lblTotal.Text = "Your cart is empty.";
                lblTotal.ForeColor = System.Drawing.Color.Red;
                btnCheckout.Enabled = false;
                return;
            }

            // Bind cart items to the GridView
            gvCart.DataSource = cart;
            gvCart.DataBind();

            // Calculate and display the total price
            decimal total = 0;
            foreach (var item in cart)
                total += item.Total; // Total is likely a property: Quantity * Price

            lblTotal.Text = $"Grand Total: RM {total:0.00}";
            lblTotal.ForeColor = System.Drawing.Color.Black;
            btnCheckout.Enabled = true;
        }

        // Handles item removal from cart
        protected void gvCart_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "RemoveItem")
            {
                // Get the index of the item to remove from CommandArgument
                int index = Convert.ToInt32(e.CommandArgument);
                var cart = Session["Cart"] as List<CartItem>;

                // Remove the item at the specified index and reload the cart
                if (cart != null && index >= 0 && index < cart.Count)
                {
                    cart.RemoveAt(index);
                    Session["Cart"] = cart;
                    LoadCart(); // Refresh the GridView and total
                }
            }
        }

        // Handles the checkout process
        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            // Redirect to login if user is not authenticated
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null || cart.Count == 0)
                return; // Nothing to checkout

            int userId = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Insert new order and get the newly generated OrderID
                SqlCommand orderCmd = new SqlCommand(
                    "INSERT INTO Orders (UserID, OrderDate) OUTPUT INSERTED.OrderID VALUES (@UserID, GETDATE())", conn);
                orderCmd.Parameters.AddWithValue("@UserID", userId);
                int orderId = (int)orderCmd.ExecuteScalar(); // Retrieve the new OrderID

                // Insert each cart item into OrderItems table
                foreach (var item in cart)
                {
                    SqlCommand itemCmd = new SqlCommand(
                        "INSERT INTO OrderItems (OrderID, ProductID, Quantity, UnitPrice) " +
                        "VALUES (@OrderID, @ProductID, @Qty, @Price)", conn);

                    itemCmd.Parameters.AddWithValue("@OrderID", orderId);
                    itemCmd.Parameters.AddWithValue("@ProductID", item.ProductID);
                    itemCmd.Parameters.AddWithValue("@Qty", item.Quantity);
                    itemCmd.Parameters.AddWithValue("@Price", item.Price);

                    itemCmd.ExecuteNonQuery(); // Execute insert for each item
                }
            }

            // Clear the cart from session and redirect to order history page
            Session["Cart"] = null;
            Response.Redirect("OrderHistory.aspx");
        }
    }
}
