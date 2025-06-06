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
    public partial class Profile : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["TechNovaDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("Login.aspx");

            if (!IsPostBack)
                LoadUserData();
        }

        private void LoadUserData()
        {
            int userId = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT Username, Email FROM Users WHERE UserID = @UserID", conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtUsername.Text = reader["Username"].ToString();
                    txtEmail.Text = reader["Email"].ToString();
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            string email = txtEmail.Text.Trim();
            string newPwd = txtNewPassword.Text.Trim();
            string confirmPwd = txtConfirmPassword.Text.Trim();

            if (!string.IsNullOrEmpty(newPwd) && newPwd != confirmPwd)
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "Passwords do not match.";
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd;

                if (!string.IsNullOrEmpty(newPwd))
                {
                    // 🔒 Hash password in real app
                    cmd = new SqlCommand("UPDATE Users SET Email = @Email, PasswordHash = @Pwd WHERE UserID = @UserID", conn);
                    cmd.Parameters.AddWithValue("@Pwd", newPwd);
                }
                else
                {
                    cmd = new SqlCommand("UPDATE Users SET Email = @Email WHERE UserID = @UserID", conn);
                }

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@UserID", userId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            lblStatus.ForeColor = System.Drawing.Color.Green;
            lblStatus.Text = "Profile updated successfully.";
        }
    }
}