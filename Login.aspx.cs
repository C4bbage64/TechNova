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
        string connStr = ConfigurationManager.ConnectionStrings["TechNovaDB"].ConnectionString;

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT UserID, IsAdmin FROM Users WHERE Username = @Username AND PasswordHash = @Password", conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Session["UserID"] = reader["UserID"];
                    Session["Username"] = username;
                    Session["IsAdmin"] = (bool)reader["IsAdmin"];
                    Response.Redirect("Default.aspx");
                }
                else
                {
                    lblMessage.Text = "Invalid login. Please check your credentials.";
                }
            }
        }
    }
}