using CS414_Team2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CS414_Team2
{
    public partial class PigeonMaster : System.Web.UI.MasterPage
    {
        public static Pigeon.User CurrentUser { get; private set; } = new Pigeon.User();

        protected void Page_PreInit(object sender, EventArgs e)
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DataProvider.LogoutInactiveUsers();

            CurrentUser = new Pigeon.User();

            if (CurrentUser.IsLoggedIn)
            {
                if (!IsPostBack)
                {
                    DataProvider.LogUserActivity(CurrentUser.UserID);
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            if (PigeonMaster.CurrentUser.IsLoggedIn)
            {
                PigeonMaster.CurrentUser.Logout();
            }

            Response.Redirect("Index.aspx");
        }
    }
}