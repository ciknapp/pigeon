using CS414_Team2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CS414_Team2
{
    public partial class Team : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (PigeonMaster.CurrentUser.IsLoggedIn)
            {
                imgNavProfilePicture.Src = PigeonMaster.CurrentUser.UserIconUrl;
            }
            else
            {
                btnLogOut.Enabled = false;
            }
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            if (PigeonMaster.CurrentUser.IsLoggedIn)
            {
                DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
                PigeonMaster.CurrentUser.Logout();
            }

            Response.Redirect("index.aspx");
        }
    }
}