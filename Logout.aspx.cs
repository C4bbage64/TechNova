using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace technova
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();                    // Remove all session variables
            Session.Abandon();                  // End the session
            Response.Redirect("Login.aspx");    // Redirect to login
        }
    }
}