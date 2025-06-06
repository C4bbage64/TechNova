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
        string connStr = ConfigurationManager.ConnectionStrings["TechNovaDB"].ConnectionString;

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim(); // Add hash later

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Check for duplicate username
                SqlCommand check = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username", conn);
                check.Parameters.AddWithValue("@Username", username);
                int exists = (int)check.ExecuteScalar();

                if (exists > 0)
                {
                    lblMessage.Text = "Username already exists. Try another.";
                    return;
                }

                // Register new user
                SqlCommand insert = new SqlCommand(
                    "INSERT INTO Users (Username, Email, PasswordHash, IsAdmin) VALUES (@Username, @Email, @Password, 0)", conn);
                insert.Parameters.AddWithValue("@Username", username);
                insert.Parameters.AddWithValue("@Email", email);
                insert.Parameters.AddWithValue("@Password", password);

                insert.ExecuteNonQuery();
                lblMessage.CssClass = "success-message";
                lblMessage.Text = "Registration successful. You can now log in.";
            }
        }
    }
}