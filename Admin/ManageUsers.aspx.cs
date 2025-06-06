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
    public partial class ManageUsers : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["TechNovaDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsAdmin"] == null || !(bool)Session["IsAdmin"])
                Response.Redirect("~/Login.aspx");

            if (!IsPostBack)
                LoadUsers();
        }

        private void LoadUsers()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT UserID, Username, Email, IsAdmin FROM Users", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvUsers.DataSource = dt;
                gvUsers.DataBind();
            }
        }

        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            lblMessage.Text = "";
            LoadUsers();
        }

        protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            lblMessage.Text = "";
            LoadUsers();
        }

        protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);
                GridViewRow row = gvUsers.Rows[e.RowIndex];

                TextBox txtUsername = (TextBox)row.FindControl("txtEditUsername");
                TextBox txtEmail = (TextBox)row.FindControl("txtEditEmail");
                CheckBox chkAdmin = (CheckBox)row.FindControl("chkEditIsAdmin");

                string newUsername = txtUsername.Text.Trim();
                string newEmail = txtEmail.Text.Trim();
                bool isAdmin = chkAdmin.Checked;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE Users SET Username = @Username, Email = @Email, IsAdmin = @IsAdmin WHERE UserID = @UserID", conn);
                    cmd.Parameters.AddWithValue("@Username", newUsername);
                    cmd.Parameters.AddWithValue("@Email", newEmail);
                    cmd.Parameters.AddWithValue("@IsAdmin", isAdmin);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                gvUsers.EditIndex = -1;
                lblMessage.Text = "User updated successfully.";
                LoadUsers();
            }
            catch (Exception ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Update failed: " + ex.Message;
            }
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ResetPwd")
            {
                int userId = Convert.ToInt32(e.CommandArgument);
                string tempPassword = "TechNova123"; // ✅ Default reset password

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE Users SET PasswordHash = @Pwd WHERE UserID = @UserID", conn);
                    cmd.Parameters.AddWithValue("@Pwd", tempPassword); // 🔐 In real apps, hash this
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = $"Password reset to: {tempPassword}";
                LoadUsers();
            }
        }

        protected void gvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(gvUsers.SelectedDataKey.Value);
            string username = gvUsers.SelectedRow.Cells[1].Text;
            lblSelectedUser.Text = username;
            pnlOrders.Visible = true;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT O.OrderID, O.OrderDate,
                        SUM(I.Quantity * I.UnitPrice) AS TotalAmount
                    FROM Orders O
                    JOIN OrderItems I ON O.OrderID = I.OrderID
                    WHERE O.UserID = @UserID
                    GROUP BY O.OrderID, O.OrderDate
                    ORDER BY O.OrderDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                gvOrders.DataSource = reader;
                gvOrders.DataBind();
            }
        }
    }
}