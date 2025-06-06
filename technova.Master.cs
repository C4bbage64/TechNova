using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace technova
{
	public partial class technova : System.Web.UI.MasterPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                bool isLoggedIn = Session["Username"] != null;
                bool isAdmin = Session["IsAdmin"] != null && (bool)Session["IsAdmin"];

                // Show or hide placeholders
                phLoggedIn.Visible = isLoggedIn;
                phLoggedOut.Visible = !isLoggedIn;
                phAdminLinks.Visible = isAdmin;

                // Optional: Show welcome message
                if (isLoggedIn)
                    lblWelcome.Text = $"Welcome, {Session["Username"]}";
            }
        }
	}
}