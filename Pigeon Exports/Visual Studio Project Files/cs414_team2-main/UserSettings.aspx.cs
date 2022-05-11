using CS414_Team2.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CS414_Team2
{
    public partial class UserSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (PigeonMaster.CurrentUser.IsLoggedIn)
            {
                hUserName.InnerText = PigeonMaster.CurrentUser.UserName;

                imgProfilePicture.ImageUrl = imgNavProfilePicture.Src = PigeonMaster.CurrentUser.UserIconUrl;

                string pigeonPointsString = string.Empty;
                DataSet userMessageCount = DataProvider.GetStatistics(
                    statisticType: "user message count",
                    startDate: DateTime.Now.AddDays(-365),
                    endDate: DateTime.Now,
                    nestID: 0,
                    userID: PigeonMaster.CurrentUser.UserID
                );

                if (userMessageCount.Tables.Count == 1 && userMessageCount.Tables[0].Rows.Count == 1)
                {
                    pigeonPointsString = userMessageCount.Tables[0].Rows[0]["cnt"].ToString();
                }
                else
                {
                    pigeonPointsString = "0";
                }

                int pigeonPoints = int.Parse(pigeonPointsString) * 3;

                lblPigeonPoints.Text = pigeonPoints.ToString();

                rptUserIconChoices.DataSource = DataProvider.GetAllUserIcons();
                rptUserIconChoices.DataBind();
            }
            else
            {
                Response.Redirect("index.aspx");
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

        protected void btnUpdatePassword_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                DataProvider.ChangePassword(PigeonMaster.CurrentUser.UserName, txtNewPassword.Text.Trim(), txtCurrentPassword.Text.Trim());
                lblUpdateStatus.Text = "Password successfully changed!";
            }
        }

        protected void rptUserIconChoices_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "changeUserIcon":
                    DataProvider.ChangeUserIcon(PigeonMaster.CurrentUser.UserID, int.Parse(e.CommandArgument.ToString()));
                    CookieJar.Eat("UserIconURL");
                    Response.Redirect("UserSettings.aspx");
                    break;
                default:
                    break;
            }
        }
    }
}