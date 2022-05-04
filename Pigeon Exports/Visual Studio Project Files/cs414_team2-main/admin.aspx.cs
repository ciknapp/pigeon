using CS414_Team2.Data;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Configuration;
using System.Globalization;

namespace CS414_Team2
{
    public partial class admin : System.Web.UI.Page
    {
        protected DataTable messageTable;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (PigeonMaster.CurrentUser.IsLoggedIn)
            {
                DataProvider.IsAdmin(PigeonMaster.CurrentUser.UserName, PigeonMaster.CurrentUser.UserID);
                rptMessages.DataSource = DataProvider.GetAllFlaggedMessages(PigeonMaster.CurrentUser.UserName, PigeonMaster.CurrentUser.UserID);
                rptMessages.DataBind();
                rptUsers.DataSource = DataProvider.GetAllUsers(PigeonMaster.CurrentUser.UserName, PigeonMaster.CurrentUser.UserID).Tables[0];
                rptUsers.DataBind();
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }
        protected void GetFile(string[] args)
        {
            int file_id;
            int group_id;

            if (int.TryParse(args[0], out file_id))
            {
                if (int.TryParse(args[1], out group_id))
                {
                    string base64String = DataProvider.GetFile(PigeonMaster.CurrentUser.UserID, group_id, file_id);
                    byte[] imageBytes = Convert.FromBase64String(base64String);

                    DataTable fileMetadata = DataProvider.GetFileMetadata(PigeonMaster.CurrentUser.UserID, group_id, file_id);

                    Response.ContentType = "image/jpeg";

                    Response.AddHeader("content-disposition", $"attachment;filename={fileMetadata.Rows[0]["name"]}{fileMetadata.Rows[0]["extension"]}");
                    Response.BinaryWrite(imageBytes);
                    Response.End();
                }
            }
        }
        protected void rptMessages_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "GetFile":
                    GetFile(e.CommandArgument.ToString().Split('|'));
                    break;
                case "UnFlag":
                    DataProvider.UnFlagMessage(PigeonMaster.CurrentUser.UserName, PigeonMaster.CurrentUser.UserID, int.Parse(e.CommandArgument.ToString()));
                    rptMessages.DataSource = DataProvider.GetAllFlaggedMessages(PigeonMaster.CurrentUser.UserName, PigeonMaster.CurrentUser.UserID);
                    rptMessages.DataBind();
                    break;
                case "DeleteMessage":
                    DataProvider.DeleteMessage(PigeonMaster.CurrentUser.UserID, int.Parse(e.CommandArgument.ToString()), PigeonMaster.CurrentUser.CurrentNestID);
                    rptMessages.DataSource = DataProvider.GetAllFlaggedMessages(PigeonMaster.CurrentUser.UserName, PigeonMaster.CurrentUser.UserID);
                    rptMessages.DataBind();
                    break;
                case "ShowContext":
                    ((Panel)e.Item.FindControl("MessageContextPanel")).Visible = true;
                    ((Button)e.Item.FindControl("ContextMenu")).Visible = false;
                    break;
            }
        }

        protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "BanMember":
                    int BannedUserID = DataProvider.GetUserID(e.CommandArgument.ToString());
                    DataProvider.BanUser(BannedUserID, PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.UserName);
                    rptUsers.DataSource = DataProvider.GetAllUsers(PigeonMaster.CurrentUser.UserName, PigeonMaster.CurrentUser.UserID);
                    rptUsers.DataBind();
                    break;
                case "UnBanMember":
                    int UnBannedUserID = DataProvider.GetUserID(e.CommandArgument.ToString());
                    DataProvider.UnBanUser(UnBannedUserID, PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.UserName);
                    rptUsers.DataSource = DataProvider.GetAllUsers(PigeonMaster.CurrentUser.UserName, PigeonMaster.CurrentUser.UserID);
                    rptUsers.DataBind();
                    break;
                case "ConfirmEmail":
                    string UserEmailToConfirm = e.CommandArgument.ToString();
                    DataProvider.AdminConfirmUserEmail(UserEmailToConfirm, PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.UserName);
                    rptUsers.DataSource = DataProvider.GetAllUsers(PigeonMaster.CurrentUser.UserName, PigeonMaster.CurrentUser.UserID);
                    rptUsers.DataBind();
                    break;
                default:
                    break;
            }
        }
        protected void rptUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Button BanButton = ((Button)e.Item.FindControl("btnBanUser"));
                Button UnBanButton = ((Button)e.Item.FindControl("btnUnBanUser"));

                LinkButton downloadButton = (LinkButton)e.Item.FindControl("btnDownloadFile");
                scriptManager.RegisterPostBackControl(downloadButton);

                string Banned = ((HiddenField)e.Item.FindControl("hdnBannedStatus")).Value;

                if (Banned == "Y")
                {
                    UnBanButton.Visible = true;
                }
                else
                {
                    BanButton.Visible = true;
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (SearchBox.Text.Trim() != string.Empty)
            {
                try
                {
                    if (int.TryParse(SearchBox.Text, out int GroupId))
                    {
                        rptSearchResult.DataSource = DataProvider.SearchForNest("", GroupId);
                        rptSearchResult.DataBind();
                    }
                    else
                    {
                        rptSearchResult.DataSource = DataProvider.SearchForNest(SearchBox.Text, null);
                        rptSearchResult.DataBind();
                    }
                    adminMultiView.SetActiveView(adminSearchAndViewGroups);
                }
                catch (OracleException oex)
                {
                    switch (oex.Number)
                    {
                        case 20002:
                            lblSearchError.Text = "Username is missing. Please try again.";
                            break;
                        default:
                            throw oex;
                    }
                }
                if (lblSearchError.Text != string.Empty && SearchBox.Text != string.Empty)
                {
                    lblSearchError.Text = string.Empty;
                }
            }
        }

        protected void rptSearchResult_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "viewMessages":
                    GroupMessages.DataSource = DataProvider.AdminGetSelectedNestMessages(PigeonMaster.CurrentUser.UserName, int.Parse(e.CommandArgument.ToString()), PigeonMaster.CurrentUser.UserID);
                    GroupMessages.DataBind();
                    rptUsersInNest.DataSource = DataProvider.AdminGetSelectedNestMembers(PigeonMaster.CurrentUser.UserID, int.Parse(e.CommandArgument.ToString()));
                    rptUsersInNest.DataBind();
                    break;

            }
        }

        protected void rptGroupMessages_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            List<string> ArgumentsForTextIdAndGroupId = new List<string>();
            ArgumentsForTextIdAndGroupId = e.CommandArgument.ToString().Split('|').ToList();

            switch (e.CommandName)
            {
                case "GetFile":
                    GetFile(e.CommandArgument.ToString().Split('|'));
                    break;
                case "DeleteMessage":
                    DataProvider.DeleteMessage(PigeonMaster.CurrentUser.UserID, int.Parse(ArgumentsForTextIdAndGroupId[0]), PigeonMaster.CurrentUser.CurrentNestID);
                    try
                    {
                        GroupMessages.DataSource = DataProvider.AdminGetSelectedNestMessages(PigeonMaster.CurrentUser.UserName, int.Parse(ArgumentsForTextIdAndGroupId[1]), PigeonMaster.CurrentUser.UserID);
                        GroupMessages.DataBind();
                    }
                    catch (OracleException oex)
                    {
                        switch (oex.Number)
                        {
                            case 20708:
                                adminMultiView.SetActiveView(adminSearchAndViewGroups);
                                break;
                            default:
                                throw oex;
                        }
                    }
                    break;
            }
        }

        protected void rptUsersInNest_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            List<string> ArgumentsForNameAndGroupId = new List<string>();
            ArgumentsForNameAndGroupId = e.CommandArgument.ToString().Split('|').ToList();

            switch (e.CommandName)
            {
                case "BanMember":
                    int BannedUserID = DataProvider.GetUserID(ArgumentsForNameAndGroupId[0]);
                    DataProvider.BanUser(BannedUserID, PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.UserName);
                    rptUsersInNest.DataSource = DataProvider.AdminGetSelectedNestMembers(PigeonMaster.CurrentUser.UserID, int.Parse(ArgumentsForNameAndGroupId[1]));
                    rptUsersInNest.DataBind();
                    break;
                case "UnBanMember":
                    int UnBannedUserID = DataProvider.GetUserID(ArgumentsForNameAndGroupId[0]);
                    DataProvider.UnBanUser(UnBannedUserID, PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.UserName);
                    rptUsersInNest.DataSource = DataProvider.AdminGetSelectedNestMembers(PigeonMaster.CurrentUser.UserID, int.Parse(ArgumentsForNameAndGroupId[1]));
                    rptUsersInNest.DataBind();
                    break;
                default:
                    break;
            }
        }

        protected void rptUsersInNest_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Button BanButton = ((Button)e.Item.FindControl("btnBanUser"));
                Button UnBanButton = ((Button)e.Item.FindControl("btnUnBanUser"));

                string Banned = ((HiddenField)e.Item.FindControl("hdnBannedStatus")).Value;

                if (Banned == "Y")
                {
                    UnBanButton.Visible = true;
                }
                else
                {
                    BanButton.Visible = true;
                }
            }
        }

        protected void btnMessageCount_Click(object sender, EventArgs e)
        {
            int user_id = 0;
            int group_id = 0;
            if (startDate.Text == "")
            {
                MessageCountTimeSpanGridView.DataSource = DataProvider.GetStatistics("all messages", DateTime.Now.AddDays(-1), DateTime.Now, group_id, user_id);
                MessageCountTimeSpanGridView.DataBind();
            }
            else if (endDate.Text == "")
            {
                DateTime StartTime = DateTime.Parse(startDate.Text + " " + startTime.Text);
                MessageCountTimeSpanGridView.DataSource = DataProvider.GetStatistics("all messages", StartTime, DateTime.Now, group_id, user_id);
                MessageCountTimeSpanGridView.DataBind();
            }
            else
            {
                DateTime StartTime = DateTime.Parse(startDate.Text + " " + startTime.Text);
                DateTime EndTime = DateTime.Parse(endDate.Text + " " + endTime.Text);
                MessageCountTimeSpanGridView.DataSource = DataProvider.GetStatistics("all messages", StartTime, EndTime, group_id, user_id);
                MessageCountTimeSpanGridView.DataBind();
            }
        }
        protected void btnResetMessagesCount_Click(object sender, EventArgs e)
        {
            endDate.Text = endTime.Text = startDate.Text = startTime.Text = String.Empty;
        }

        protected void btnGroupMostMessagesCount_Click(object sender, EventArgs e)
        {
            int user_id = 0;
            int group_id = 0;
            if (startDate2.Text == "")
            {
                MostMessagesGroupGridView.DataSource = DataProvider.GetStatistics("most active group", DateTime.Now.AddDays(-1), DateTime.Now, group_id, user_id);
                MostMessagesGroupGridView.DataBind();

            }
            else if (endDate2.Text == "")
            {
                DateTime StartTime = DateTime.Parse(startDate2.Text + " " + startTime2.Text);
                MostMessagesGroupGridView.DataSource = DataProvider.GetStatistics("most active group", StartTime, DateTime.Now, group_id, user_id);
                MostMessagesGroupGridView.DataBind();
            }
            else
            {
                DateTime StartTime = DateTime.Parse(startDate2.Text + " " + startTime2.Text);
                DateTime EndTime = DateTime.Parse(endDate2.Text + " " + endTime2.Text);
                MostMessagesGroupGridView.DataSource = DataProvider.GetStatistics("most active group", StartTime, EndTime, group_id, user_id);
                MostMessagesGroupGridView.DataBind();
            }
        }
        protected void btnResetGroupMostMessagesCount_Click(object sender, EventArgs e)
        {
            endDate2.Text = endTime2.Text = startDate2.Text = startTime2.Text = String.Empty;
        }
        protected void btnUserMostMessagesCount_Click(object sender, EventArgs e)
        {
            int user_id = 0;
            int group_id = 0;
            if (startDate3.Text == "")
            {
                MostMessagesUserGridView.DataSource = DataProvider.GetStatistics("most active user", DateTime.Now.AddDays(-1), DateTime.Now, group_id, user_id);
                MostMessagesUserGridView.DataBind();

            }
            else if (endDate3.Text == "")
            {
                DateTime StartTime = DateTime.Parse(startDate3.Text + " " + startTime3.Text);
                MostMessagesUserGridView.DataSource = DataProvider.GetStatistics("most active user", StartTime, DateTime.Now, group_id, user_id);
                MostMessagesUserGridView.DataBind();
            }
            else
            {
                DateTime StartTime = DateTime.Parse(startDate3.Text + " " + startTime3.Text);
                DateTime EndTime = DateTime.Parse(endDate3.Text + " " + endTime3.Text);
                MostMessagesUserGridView.DataSource = DataProvider.GetStatistics("most active user", StartTime, EndTime, group_id, user_id);
                MostMessagesUserGridView.DataBind();
            }
        }
        protected void btnResetUserMostMessagesCount_Click(object sender, EventArgs e)
        {
            endDate3.Text = endTime3.Text = startDate3.Text = startTime3.Text = String.Empty;
        }
        protected void btnSearchUserMessagesCount_Click(object sender, EventArgs e)
        {
            int user_id = PigeonMaster.CurrentUser.UserID;
            int? group_id = null;
            if (userID.Text != "")
            {
                user_id = int.Parse(userID.Text);
            }
            if (startDate4.Text == "")
            {
                SearchUserMessageCountGridView.DataSource = DataProvider.GetStatistics("user message count", DateTime.Now.AddDays(-1), DateTime.Now, group_id ?? 0, user_id);
                SearchUserMessageCountGridView.DataBind();

            }
            else if (endDate4.Text == "")
            {
                DateTime StartTime = DateTime.Parse(startDate4.Text + " " + startTime4.Text);
                SearchUserMessageCountGridView.DataSource = DataProvider.GetStatistics("user message count", StartTime, DateTime.Now, group_id ?? 0, user_id);
                SearchUserMessageCountGridView.DataBind();
            }
            else
            {
                DateTime StartTime = DateTime.Parse(startDate4.Text + " " + startTime4.Text);
                DateTime EndTime = DateTime.Parse(endDate4.Text + " " + endTime4.Text);
                SearchUserMessageCountGridView.DataSource = DataProvider.GetStatistics("user message count", StartTime, EndTime, group_id ?? 0, user_id);
                SearchUserMessageCountGridView.DataBind();
            }
        }
        protected void btnResetSearchUserMessagesCount_Click(object sender, EventArgs e)
        {
            userID.Text = endDate4.Text = endTime4.Text = startDate4.Text = startTime4.Text = String.Empty;
            if (PigeonMaster.CurrentUser.UserName == "admint")
            {
                SearchUserMessageCountGridView.DataSource = null;
                SearchUserMessageCountGridView.DataBind();
            }
        }
        protected void btnSearchGroupMessagesCount_Click(object sender, EventArgs e)
        {
            int? user_id = null;
            int group_id = PigeonMaster.CurrentUser.CurrentNestID;
            if (groupID.Text != "")
            {
                group_id = int.Parse(groupID.Text);
            }
            if (startDate5.Text == "")
            {
                SearchGroupMessageCountGridView.DataSource = DataProvider.GetStatistics("group message count", DateTime.Now.AddDays(-1), DateTime.Now, group_id, user_id ?? 0);
                SearchGroupMessageCountGridView.DataBind();

            }
            else if (endDate5.Text == "")
            {
                DateTime StartTime = DateTime.Parse(startDate5.Text + " " + startTime5.Text);
                SearchGroupMessageCountGridView.DataSource = DataProvider.GetStatistics("group message count", StartTime, DateTime.Now, group_id, user_id ?? 0);
                SearchGroupMessageCountGridView.DataBind();
            }
            else
            {
                DateTime StartTime = DateTime.Parse(startDate5.Text + " " + startTime5.Text);
                DateTime EndTime = DateTime.Parse(endDate5.Text + " " + endTime5.Text);
                SearchGroupMessageCountGridView.DataSource = DataProvider.GetStatistics("group message count", StartTime, EndTime, group_id, user_id ?? 0);
                SearchGroupMessageCountGridView.DataBind();
            }
        }
        protected void btnResetSearchGroupMessagesCount_Click(object sender, EventArgs e)
        {
            groupID.Text = endDate5.Text = endTime5.Text = startDate5.Text = startTime5.Text = String.Empty;
            if (PigeonMaster.CurrentUser.UserName == "admint")
            {
                SearchGroupMessageCountGridView.DataSource = null;
                SearchGroupMessageCountGridView.DataBind();
            }
        }
        protected void btnSwitchToMessagingStasticsView_Click(object sender, EventArgs e)
        {
            adminMultiView.SetActiveView(adminMessageStatistics);
        }
        protected void btnSwitchToAdminDashBoardView_Click(object sender, EventArgs e)
        {
            adminMultiView.SetActiveView(adminDashboard);
        }
        protected void btnSwitchToGroupMessagingSearchView_Click(object sender, EventArgs e)
        {
            adminMultiView.SetActiveView(adminSearchAndViewGroups);
        }
        protected void btnSwitchToNest_Click(object sender, EventArgs e)
        {
            Response.Redirect("nest.aspx");
        }

        protected void rptMessages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton downloadButton = (LinkButton)e.Item.FindControl("btnDownloadFile");
                scriptManager.RegisterPostBackControl(downloadButton);
            }
        }

        protected void GroupMessages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton downloadButton = (LinkButton)e.Item.FindControl("btnDownloadFile");
                scriptManager.RegisterPostBackControl(downloadButton);

                HiddenField hdnVisible = (HiddenField)e.Item.FindControl("hdnVisibility");

                if(hdnVisible.Value == "N")
                {
                    ((Button)e.Item.FindControl("DeleteMessage")).Visible = false;
                }
            }
        }

        protected void btnSwitchToUserMessagingSearchView_Click(object sender, EventArgs e)
        {
            adminMultiView.SetActiveView(adminSearchAndViewUsers);
        }

        protected void btnUsernameSearch_Click(object sender, EventArgs e)
        {
            if (txtUsernameSearch.Text.Trim() != string.Empty)
            {
                try
                {
                    rptUsernamesSearch.DataSource = DataProvider.SearchUser(txtUsernameSearch.Text.Trim());
                    rptUsernamesSearch.DataBind();

                    adminMultiView.SetActiveView(adminSearchAndViewUsers);
                }
                catch (OracleException oex)
                {
                    switch (oex.Number)
                    {
                        case 20002:
                            lblUsernameSearchError.Text = "Username is missing. Please try again.";
                            break;
                        default:
                            throw oex;
                    }
                }

                if (lblUsernameSearchError.Text != string.Empty && txtUsernameSearch.Text != string.Empty)
                {
                    lblUsernameSearchError.Text = string.Empty;
                }
            }
        }

        protected void rptUsernameMessages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField hdnVisible = (HiddenField)e.Item.FindControl("hdnVisibility");

            if (hdnVisible.Value == "N")
            {
                ((Button)e.Item.FindControl("DeleteMessage")).Visible = false;
            }
        }

        protected void rptUsernameMessages_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            List<string> ArgumentsForTextIdAndGroupId = new List<string>();
            ArgumentsForTextIdAndGroupId = e.CommandArgument.ToString().Split('|').ToList();

            switch (e.CommandName)
            {
                case "DeleteMessage":
                    DataProvider.DeleteMessage(PigeonMaster.CurrentUser.UserID, int.Parse(ArgumentsForTextIdAndGroupId[0]), int.Parse(ArgumentsForTextIdAndGroupId[2]));
                    try
                    {
                        rptUsernameMessages.DataSource = DataProvider.AdminGetSelectedUserMessages(int.Parse(ArgumentsForTextIdAndGroupId[1]));
                        rptUsernameMessages.DataBind();
                    }
                    catch (OracleException oex)
                    {
                        switch (oex.Number)
                        {
                            case 20708:
                                adminMultiView.SetActiveView(adminSearchAndViewUsers);
                                break;
                            default:
                                throw oex;
                        }
                    }
                    break;
            }
        }

        protected void rptUsernamesSearch_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "viewUserMessages":
                    rptUsernameMessages.DataSource = DataProvider.AdminGetSelectedUserMessages(int.Parse(e.CommandArgument.ToString()));
                    rptUsernameMessages.DataBind();

                    rptSelectedUserNests.DataSource = DataProvider.GetGroupNestsForUser(int.Parse(e.CommandArgument.ToString()));
                    rptSelectedUserNests.DataBind();
                    break;
            }
        }

        protected void rptSelectedUserNests_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void rptSelectedUserNests_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }
    }
}