using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace technova
{
    public partial class Products : System.Web.UI.Page
    {
        // Connection string from Web.config
        string connStr = ConfigurationManager.ConnectionStrings["TechNovaDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Load data only on the first page load (not on postbacks like dropdown changes)
            if (!IsPostBack)
            {
                LoadCategories();
                LoadProducts();
            }
        }

        // Loads product categories into the dropdown list
        private void LoadCategories()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                // Select all categories from the database
                SqlCommand cmd = new SqlCommand("SELECT * FROM Categories", conn);

                // Bind categories to dropdown list
                ddlCategory.DataSource = cmd.ExecuteReader();
                ddlCategory.DataTextField = "CategoryName";  // Display text
                ddlCategory.DataValueField = "CategoryID";   // Value on selection
                ddlCategory.DataBind();

                // Add an "All" option with value 0
                ddlCategory.Items.Insert(0, new ListItem("All", "0"));
            }
        }

        // Loads products based on selected category
        private void LoadProducts()
        {
            string query = "SELECT * FROM Products";

            // Filter products by selected category if not "All"
            if (ddlCategory.SelectedValue != "0")
            {
                query += " WHERE CategoryID = @CategoryID";
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);

                // Add category filter parameter if needed
                if (ddlCategory.SelectedValue != "0")
                {
                    cmd.Parameters.AddWithValue("@CategoryID", ddlCategory.SelectedValue);
                }

                // Execute and bind data to Repeater control
                SqlDataReader reader = cmd.ExecuteReader();
                rptProducts.DataSource = reader;
                rptProducts.DataBind();
            }
        }

        // Triggered when the category dropdown selection changes
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Reload products based on new selection
            LoadProducts();
        }

        // Handles "Add to Cart" button command from product listing
        protected void btnAddToCart_Command(object sender, CommandEventArgs e)
        {
            // Get product ID from CommandArgument
            int productId = Convert.ToInt32(e.CommandArgument);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                // Retrieve product details by ID
                SqlCommand cmd = new SqlCommand("SELECT * FROM Products WHERE ProductID = @ProductID", conn);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Read product name and price from the record
                    string name = reader["ProductName"].ToString();
                    decimal price = Convert.ToDecimal(reader["Price"]);

                    // Get the cart from Session or create a new one
                    List<CartItem> cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();

                    // Check if the item already exists in the cart
                    var existing = cart.Find(i => i.ProductID == productId);
                    if (existing != null)
                    {
                        // If exists, increment quantity
                        existing.Quantity += 1;
                    }
                    else
                    {
                        // Otherwise, add new item to cart
                        cart.Add(new CartItem
                        {
                            ProductID = productId,
                            ProductName = name,
                            Price = price,
                            Quantity = 1
                        });
                    }

                    // Save updated cart back to Session
                    Session["Cart"] = cart;
                }
            }

            // Redirect to cart page after adding the item
            Response.Redirect("Cart.aspx");
        }
    }
}