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
        string connStr = ConfigurationManager.ConnectionStrings["TechNovaDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                LoadProducts();
            }
        }

        private void LoadCategories()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Categories", conn);
                ddlCategory.DataSource = cmd.ExecuteReader();
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryID";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
            }
        }

        private void LoadProducts()
        {
            string query = "SELECT * FROM Products";
            if (ddlCategory.SelectedValue != "0")
            {
                query += " WHERE CategoryID = @CategoryID";
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                if (ddlCategory.SelectedValue != "0")
                {
                    cmd.Parameters.AddWithValue("@CategoryID", ddlCategory.SelectedValue);
                }

                SqlDataReader reader = cmd.ExecuteReader();
                rptProducts.DataSource = reader;
                rptProducts.DataBind();
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void btnAddToCart_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            int productId = Convert.ToInt32(e.CommandArgument);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Products WHERE ProductID = @ProductID", conn);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string name = reader["ProductName"].ToString();
                    decimal price = Convert.ToDecimal(reader["Price"]);

                    List<CartItem> cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();

                    var existing = cart.Find(i => i.ProductID == productId);
                    if (existing != null)
                    {
                        existing.Quantity += 1;
                    }
                    else
                    {
                        cart.Add(new CartItem
                        {
                            ProductID = productId,
                            ProductName = name,
                            Price = price,
                            Quantity = 1
                        });
                    }

                    Session["Cart"] = cart;
                }
            }

            Response.Redirect("Cart.aspx");
        }
    }
}