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
        // Retrieve connection string from Web.config
        string connStr = ConfigurationManager.ConnectionStrings["TechNovaDB"].ConnectionString;

        // Runs when the page is loaded
        protected void Page_Load(object sender, EventArgs e)
        {
            // Redirect to login if not logged in
            if (Session["UserID"] == null)
                Response.Redirect("Login.aspx");

            // Load user data only on first page load (not on postback)
            if (!IsPostBack)
                LoadUserData();
        }

        // Loads current user details into the form
        private void LoadUserData()
        {
            int userId = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Get username and email of the user
                SqlCommand cmd = new SqlCommand("SELECT Username, Email FROM Users WHERE UserID = @UserID", conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Populate textboxes with user info
                    txtUsername.Text = reader["Username"].ToString();
                    txtEmail.Text = reader["Email"].ToString();
                }
            }
        }

        // Runs when the Update button is clicked
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            string email = txtEmail.Text.Trim();
            string newPwd = txtNewPassword.Text.Trim();
            string confirmPwd = txtConfirmPassword.Text.Trim();

            // Validate password confirmation if a new password is entered
            if (!string.IsNullOrEmpty(newPwd) && newPwd != confirmPwd)
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "Passwords do not match.";
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd;

                // Update email and password if password is provided
                if (!string.IsNullOrEmpty(newPwd))
                {
                    // ⚠️ Security Warning: Password should be hashed before storing
                    cmd = new SqlCommand("UPDATE Users SET Email = @Email, PasswordHash = @Pwd WHERE UserID = @UserID", conn);
                    cmd.Parameters.AddWithValue("@Pwd", newPwd); // Insecure in real-world scenarios
                }
                else
                {
                    // Only update email
                    cmd = new SqlCommand("UPDATE Users SET Email = @Email WHERE UserID = @UserID", conn);
                }

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@UserID", userId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Notify user of successful update
            lblStatus.ForeColor = System.Drawing.Color.Green;
            lblStatus.Text = "Profile updated successfully.";
        }
    }
}
