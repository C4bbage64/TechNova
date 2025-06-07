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
    public partial class Login : System.Web.UI.Page
    {
        // Fetch the database connection string from Web.config
        string connStr = ConfigurationManager.ConnectionStrings["TechNovaDB"].ConnectionString;

        // Triggered when the Login button is clicked
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Get username and password from text boxes
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // SQL command to validate user credentials
                SqlCommand cmd = new SqlCommand(
                    "SELECT UserID, IsAdmin FROM Users WHERE Username = @Username AND PasswordHash = @Password", conn);

                // Add parameters to prevent SQL injection
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password); // Assumes password is stored as a hash

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // If credentials match, set session variables
                    Session["UserID"] = reader["UserID"];
                    Session["Username"] = username;
                    Session["IsAdmin"] = (bool)reader["IsAdmin"];

                    // Redirect to homepage
                    Response.Redirect("Default.aspx");
                }
                else
                {
                    // Display error if login fails
                    lblMessage.Text = "Invalid login. Please check your credentials.";
                }
            }
        }
    }
}
