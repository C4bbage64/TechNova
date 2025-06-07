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
    public partial class Register : System.Web.UI.Page
    {
        // Connection string to the TechNova database from Web.config
        string connStr = ConfigurationManager.ConnectionStrings["TechNovaDB"].ConnectionString;

        // Triggered when the Register button is clicked
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            // Get and trim user inputs from the registration form
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim(); // TODO: Add hashing before saving

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Check if the username already exists in the database
                SqlCommand check = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username", conn);
                check.Parameters.AddWithValue("@Username", username);
                int exists = (int)check.ExecuteScalar();

                if (exists > 0)
                {
                    // Username is already taken
                    lblMessage.Text = "Username already exists. Try another.";
                    return;
                }

                // Insert new user into the database
                SqlCommand insert = new SqlCommand(
                    "INSERT INTO Users (Username, Email, PasswordHash, IsAdmin) VALUES (@Username, @Email, @Password, 0)", conn);

                insert.Parameters.AddWithValue("@Username", username);
                insert.Parameters.AddWithValue("@Email", email);
                insert.Parameters.AddWithValue("@Password", password); // Note: Storing plaintext is insecure!

                insert.ExecuteNonQuery();

                // Inform the user that registration was successful
                lblMessage.CssClass = "success-message";
                lblMessage.Text = "Registration successful. You can now log in.";
            }
        }
    }
}
