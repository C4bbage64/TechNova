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
        string connStr = ConfigurationManager.ConnectionStrings["TechNovaDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsAdmin"] == null || !(bool)Session["IsAdmin"])
                Response.Redirect("~/Login.aspx");

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
                SqlCommand cmd = new SqlCommand("SELECT CategoryID, CategoryName FROM Categories", conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                ddlCategory.DataSource = reader;
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryID";
                ddlCategory.DataBind();
            }
        }

        private void LoadProducts()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT P.ProductID, P.ProductName, P.Price, P.ImagePath, C.CategoryName, C.CategoryID
                                 FROM Products P JOIN Categories C ON P.CategoryID = C.CategoryID";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvProducts.DataSource = dt;
                gvProducts.DataBind();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            decimal price = decimal.Parse(txtPrice.Text.Trim());
            int categoryId = int.Parse(ddlCategory.SelectedValue);
            string image = txtImage.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Products (ProductName, Price, CategoryID, ImagePath) VALUES (@Name, @Price, @Cat, @Img)", conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Cat", categoryId);
                cmd.Parameters.AddWithValue("@Img", image);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            txtName.Text = "";
            txtPrice.Text = "";
            txtImage.Text = "";

            LoadProducts();
        }

        protected void gvProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvProducts.EditIndex = e.NewEditIndex;
            LoadProducts();

            // Populate categories for the row being edited
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

                // Set selected category
                // Ensure DataKeyNames="ProductID,CategoryID" in your GridView markup
                object catIdObj = gvProducts.DataKeys[e.NewEditIndex].Values["CategoryID"];
                if (catIdObj != null)
                {
                    ddl.SelectedValue = catIdObj.ToString();
                }
            }
        }

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
                SqlCommand cmd = new SqlCommand("UPDATE Products SET ProductName=@Name, Price=@Price, CategoryID=@Cat, ImagePath=@Img WHERE ProductID=@ID", conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Cat", categoryId);
                cmd.Parameters.AddWithValue("@Img", image);
                cmd.Parameters.AddWithValue("@ID", productId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            gvProducts.EditIndex = -1;
            LoadProducts();
        }

        protected void gvProducts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvProducts.EditIndex = -1;
            LoadProducts();
        }

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

            LoadProducts();
        }
    }
}