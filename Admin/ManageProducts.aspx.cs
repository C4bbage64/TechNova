using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace technova
{
    public partial class ManageProducts : System.Web.UI.Page
    {
        // Connection string from Web.config
        string connStr = ConfigurationManager.ConnectionStrings["TechNovaDB"].ConnectionString;

        // On every page load
        protected void Page_Load(object sender, EventArgs e)
        {
            // Redirect non-admins
            if (Session["IsAdmin"] == null || !(bool)Session["IsAdmin"])
                Response.Redirect("~/Login.aspx");

            // Only load on first visit, not on postbacks (e.g., form submissions)
            if (!IsPostBack)
            {
                LoadCategories();
                LoadProducts();
            }
        }

        // Load categories into the Add Product dropdown
        private void LoadCategories()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT CategoryID, CategoryName FROM Categories", conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlCategory.DataSource = reader;
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryID";
                ddlCategory.DataBind();
            }
        }

        // Load all products into the GridView
        private void LoadProducts()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT P.ProductID, P.ProductName, P.Price, P.ImagePath,
                           C.CategoryName, C.CategoryID
                    FROM Products P
                    JOIN Categories C ON P.CategoryID = C.CategoryID";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvProducts.DataSource = dt;
                gvProducts.DataBind();
            }
        }

        // Add new product to the database
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            decimal price = decimal.Parse(txtPrice.Text.Trim());
            int categoryId = int.Parse(ddlCategory.SelectedValue);
            string image = txtImage.Text.Trim(); // Ideally: Validate and save uploaded image path

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Products (ProductName, Price, CategoryID, ImagePath) VALUES (@Name, @Price, @Cat, @Img)", conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Cat", categoryId);
                cmd.Parameters.AddWithValue("@Img", image);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Clear form fields after insert
            txtName.Text = "";
            txtPrice.Text = "";
            txtImage.Text = "";

            LoadProducts(); // Refresh product list
        }

        // Enable editing mode in GridView
        protected void gvProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvProducts.EditIndex = e.NewEditIndex;
            LoadProducts();

            // Load categories into dropdown for the current edit row
            DropDownList ddl = (DropDownList)gvProducts.Rows[e.NewEditIndex].FindControl("ddlCategoryEdit");
            if (ddl != null)
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand("SELECT CategoryID, CategoryName FROM Categories", conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    ddl.DataSource = reader;
                    ddl.DataTextField = "CategoryName";
                    ddl.DataValueField = "CategoryID";
                    ddl.DataBind();
                }

                // Preselect the current category in dropdown
                object catIdObj = gvProducts.DataKeys[e.NewEditIndex].Values["CategoryID"];
                if (catIdObj != null)
                    ddl.SelectedValue = catIdObj.ToString();
            }
        }

        // Update product info in database
        protected void gvProducts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int productId = (int)gvProducts.DataKeys[e.RowIndex].Value;
            GridViewRow row = gvProducts.Rows[e.RowIndex];

            string name = ((TextBox)row.FindControl("txtNameEdit")).Text.Trim();
            decimal price = decimal.Parse(((TextBox)row.FindControl("txtPriceEdit")).Text.Trim());
            string image = ((TextBox)row.FindControl("txtImageEdit")).Text.Trim();
            int categoryId = int.Parse(((DropDownList)row.FindControl("ddlCategoryEdit")).SelectedValue);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Products SET ProductName=@Name, Price=@Price, CategoryID=@Cat, ImagePath=@Img WHERE ProductID=@ID", conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Cat", categoryId);
                cmd.Parameters.AddWithValue("@Img", image);
                cmd.Parameters.AddWithValue("@ID", productId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            gvProducts.EditIndex = -1;
            LoadProducts(); // Refresh updated data
        }

        // Cancel editing mode
        protected void gvProducts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvProducts.EditIndex = -1;
            LoadProducts();
        }

        // Delete product from database
        protected void gvProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int productId = (int)gvProducts.DataKeys[e.RowIndex].Value;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Products WHERE ProductID = @ID", conn);
                cmd.Parameters.AddWithValue("@ID", productId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadProducts(); // Refresh list
        }
    }
}
