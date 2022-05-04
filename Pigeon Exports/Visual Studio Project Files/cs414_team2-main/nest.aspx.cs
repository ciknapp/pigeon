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
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Threading;

namespace CS414_Team2
{
    public partial class nest : System.Web.UI.Page
    {
        protected DataTable messageTable;
        private DataSet nestsForUserDataSource;
        private bool ContactsTab
        {
            get
            {
                if (Session["contactsTab"] == null)
                {
                    Session["contactsTab"] = false;
                }

                return (bool)Session["contactsTab"];
            }

            set
            {
                Session["contactsTab"] = value;
            }
        }

        private bool GroupsTab
        {
            get
            {
                if (Session["groupsTab"] == null)
                {
                    Session["groupsTab"] = true;
                }

                return (bool)Session["groupsTab"];
            }

            set
            {
                Session["groupsTab"] = value;
            }
        }

        private bool NotificationsTab
        {
            get
            {
                if (Session["NotificationsTab"] == null)
                {
                    Session["NotificationsTab"] = false;
                }

                return (bool)Session["NotificationsTab"];
            }

            set
            {
                Session["NotificationsTab"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CookieJar.Eat("NestRole");
                    messageTable = new DataTable();
                    Session["messageTable"] = messageTable;
                }
                else
                {
                    if (Session["messageTable"] != null)
                    {
                        messageTable = (DataTable)Session["messageTable"];
                    }
                    else
                    {
                        messageTable = new DataTable();
                        Session["messageTable"] = messageTable;
                    }
                }
            }
            catch (InvalidCastException)
            {
                Session["messageTable"] = null;
                Response.Redirect("nest.aspx");
            }

            if (PigeonMaster.CurrentUser.IsLoggedIn)
            {
                if (DataProvider.UserIsBanned(PigeonMaster.CurrentUser.UserID))
                {
                    PigeonMaster.CurrentUser.Logout();
                    Response.Redirect("index.aspx");
                }

                divAdminButton.Visible = DataProvider.GetUserSystemRole(PigeonMaster.CurrentUser.UserID) == "admin";

                lblNavBarUsername.Text = PigeonMaster.CurrentUser.UserName;

                if (DataProvider.GetNotificationNestsForUser(PigeonMaster.CurrentUser.UserID).Tables[0].Rows.Count > 0)
                {
                    btnNewActivity.CssClass += " new-notifs";
                }
                else
                {
                    btnNewActivity.Visible = false;
                }

                imgProfilePicture.Src = PigeonMaster.CurrentUser.UserIconUrl;

                DataProvider.NotifyUser(PigeonMaster.CurrentUser.UserID);

                if (DataProvider.UserHasNewNotifications(PigeonMaster.CurrentUser.UserID))
                {
                    iconExclam.Visible = true;
                    iconNotifs.ForeColor = Color.Red;
                }
                else
                {
                    iconExclam.Visible = false;
                    iconNotifs.ForeColor = Color.White;
                }

                //nestsForUserDataSource = ContactsTab ? DataProvider.GetPrivateNestsForUser(PigeonMaster.CurrentUser.UserID) : DataProvider.GetGroupNestsForUser(PigeonMaster.CurrentUser.UserID);

                if (ContactsTab)
                {
                    nestsForUserDataSource = DataProvider.GetPrivateNestsForUser(PigeonMaster.CurrentUser.UserID);
                    btnAddNest.Visible = false;

                }
                else if (GroupsTab)
                {
                    nestsForUserDataSource = DataProvider.GetGroupNestsForUser(PigeonMaster.CurrentUser.UserID);
                    btnAddNest.Visible = true;
                }
                else
                {
                    nestsForUserDataSource = DataProvider.GetNotificationNestsForUser(PigeonMaster.CurrentUser.UserID);
                    btnAddNest.Visible = false;
                }

                rptNestsForUser.DataSource = nestsForUserDataSource;
                nestsForUserHeading.InnerText = ContactsTab ? "Contacts" : GroupsTab ? "Nests" : "New Activity";
                rptNestsForUser.DataBind();

                try
                {
                    int nestId = PigeonMaster.CurrentUser.CurrentNestID;
                    int memberCounts = DataProvider.GetMemberCount(nestId);
                    if (nestId != -999)
                    {
                        if(DataProvider.UserIsKicked(PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.CurrentNestID) || !DataProvider.NestIsVisible(PigeonMaster.CurrentUser.CurrentNestID))
                        {
                            PigeonMaster.CurrentUser.CurrentNestID = -999;
                            Response.Redirect("nest.aspx");
                        }

                        btnLockNest.Visible = PigeonMaster.CurrentUser.CurrentNestRole == 0 || (PigeonMaster.CurrentUser.CurrentNestRole == 1 && PigeonMaster.CurrentUser.CurrentNestID == -1);

                        if (!IsPostBack)
                        {
                            bool nestIsLocked = DataProvider.NestIsLocked(PigeonMaster.CurrentUser.CurrentNestID);

                            messageContentMultiview.SetActiveView(nestIsLocked ? viewLockedNest : viewNormalMessage);

                            lblLockIcon.CssClass = $"d-flex fa-solid fa-lock{(!nestIsLocked ? "-open" : "")}";

                            lblLockText.Text = $"{(nestIsLocked ? " Locked" : " Unlocked")}";

                            lblLockText.ForeColor = lblLockIcon.ForeColor = nestIsLocked ? Color.Red : ColorTranslator.FromHtml("#4A87C6");

                            bool nestIsMuted = DataProvider.UserHasNestMuted(PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.CurrentNestID);

                            notifBell.CssClass = $"d-flex fa-solid fa-bell{(nestIsMuted ? "-slash" : "")}";

                            notifBellText.Text = $"{(nestIsMuted ? " Muted" : "")}";

                            notifBellText.ForeColor = notifBell.ForeColor = nestIsMuted ? Color.Red : ColorTranslator.FromHtml("#4A87C6");

                            messageTable = DataProvider.GetAllMessages(PigeonMaster.CurrentUser.UserName, PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.CurrentNestID).Tables[0];
                            messageTable.Columns.Add("DECRYPTED", typeof(string));

                            // Set all to encrypted
                            foreach (DataRow message in messageTable.Rows)
                            {
                                message["DECRYPTED"] = "N";
                            }

                            messageTable.AcceptChanges();

                            Session["messageTable"] = messageTable;

                            rptMessages.DataSource = messageTable;
                            rptMessages.DataBind();
                        }

                        int roleUser = DataProvider.GetUserRole(PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.CurrentNestID);
                        if (roleUser > 2)
                        {
                            btnSendInvite.Visible = false;
                        }

                        DataSet memberSet = DataProvider.GetMembersInNest(PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.CurrentNestID);

                        if (memberSet.Tables.Count == 1)
                        {
                            memberCounts = memberSet.Tables[0].Rows.Count;
                        }
                        else
                        {
                            memberCounts = 0;
                        }

                        rptNestMembers.DataSource = memberSet;
                        rptNestMembers.DataBind();

                        nestMembersHeader.InnerText = DataProvider.GetNestName(PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.CurrentNestID);

                        timerDBPoll.Enabled = true;
                        txtComment.Visible = btnSendMessage.Visible = btnAttachFile.Visible = !DataProvider.NestIsLocked(PigeonMaster.CurrentUser.CurrentNestID);

                        bool NestIsPrivate = DataProvider.IsNestPrivate(PigeonMaster.CurrentUser.CurrentNestID);

                        if (NestIsPrivate)
                        {
                            btnDeleteNest.Visible = btnLeaveNest.Visible = false;
                        }
                        else
                        {
                            if (roleUser > 1)
                            {
                                btnLeaveNest.Visible = true;
                            }
                            else
                            {
                                btnDeleteNest.Visible = true;
                            }
                        }

                        //if(memberCounts > 6 && (roleUser == 3 || roleUser == 2))
                        //{
                        //    btnLeaveNest.Visible = true;
                        //}
                        //else if(memberCounts < 5 && roleUser == 0)
                        //{
                        //    btnDeleteNest.Visible = true;
                        //}
                        //else if(roleUser == 1)
                        //{
                        //    btnDeleteNest.Visible = true;
                        //}
                    }
                    else
                    {
                        btnDeleteNest.Visible = false;
                        btnLeaveNest.Visible = false;
                        txtbox.Visible = false;
                        btnSendInvite.Visible = false;
                        btnToggleNestMute.Visible = false;
                        btnLockNest.Visible = false;

                        GeneralMultiView.SetActiveView(viewEmpty);
                    }

                }
                catch (FormatException)
                {
                    // They don't have a nest
                    timerDBPoll.Enabled = false;
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        private void appendNewMessages()
        {
            DataTable newMessages = DataProvider.GetNewMessages(PigeonMaster.CurrentUser.UserName, PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.CurrentNestID);
            newMessages.Columns.Add("DECRYPTED", typeof(string));

            foreach (DataRow message in newMessages.Rows)
            {
                message["DECRYPTED"] = "N";
                newMessages.AcceptChanges();
                messageTable.Rows.Add(message.ItemArray);
            }

            messageTable.AcceptChanges();

            Session["messageTable"] = messageTable;

            rptMessages.DataSource = messageTable;
            rptMessages.DataBind();
        }

        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);

                if (!DataProvider.UserIsBanned(PigeonMaster.CurrentUser.UserID) || !DataProvider.NestIsLocked(PigeonMaster.CurrentUser.CurrentNestID) || !DataProvider.UserIsKicked(PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.CurrentNestID))
                {
                    string processedMessage = Regex.Replace(txtComment.Text, @"  +", " ").Trim();

                    if (processedMessage != string.Empty)
                    {
                        if (fileUpload.HasFile)
                        {
                            if (fileUpload.PostedFile.ContentLength <= 1097152)
                            {
                                DataProvider.SendMessage(
                                    userID: PigeonMaster.CurrentUser.UserID,
                                    nestID: PigeonMaster.CurrentUser.CurrentNestID,
                                    messageText: processedMessage,
                                    fileB64: Convert.ToBase64String(fileUpload.FileBytes),
                                    fileName: Regex.Match(fileUpload.FileName, "^[^.]+").Value,
                                    extension: Regex.Match(fileUpload.FileName, "([.]\\w+)+$").Value
                                );

                                lblFileError.Text = "";
                            }
                            else
                            {
                                lblFileError.Text = "Your file is too big. Files must be under 1MB.";
                            }
                        }
                        else
                        {
                            DataProvider.SendMessage(
                                userID: PigeonMaster.CurrentUser.UserID,
                                nestID: PigeonMaster.CurrentUser.CurrentNestID,
                                messageText: processedMessage
                            );
                        }

                        appendNewMessages();
                        txtComment.Text = txtFileComment.Text = string.Empty;
                        messageContentMultiview.SetActiveView(viewNormalMessage);
                    }
                }
                else
                {
                    Response.Redirect("nest.aspx");
                }
            }
        }

        protected void timerDBPoll_Tick(object sender, EventArgs e)
        {
            if (DataProvider.HasNewMessages(PigeonMaster.CurrentUser.CurrentNestID, PigeonMaster.CurrentUser.UserID))
            {
                appendNewMessages();
            }
            else
            {
                rptMessages.DataSource = messageTable;
                rptMessages.DataBind();
            }
        }

        protected void rptNestsForUser_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "SwitchNests":
                    DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
                    PigeonMaster.CurrentUser.CurrentNestID = int.Parse(e.CommandArgument.ToString());
                    Response.Redirect("nest.aspx");
                    break;
                default:
                    break;
            }
            GeneralMultiView.SetActiveView(MessageViews);
            MembersMultiView.SetActiveView(NestMembersDetail);
            txtbox.Visible = true;
        }

        protected void rptNestsForUser_TabContactsClick(object sender, EventArgs e)
        {
            nestsForUserDataSource = DataProvider.GetPrivateNestsForUser(PigeonMaster.CurrentUser.UserID);
            ContactsTab = true;
            GroupsTab = false;
            NotificationsTab = false;
            rptNestsForUser.DataSource = nestsForUserDataSource;
            nestsForUserHeading.InnerText = ContactsTab ? "Contacts" : GroupsTab ? "Nests" : "New Activity";
            rptNestsForUser.DataBind();

            btnAddNest.Visible = false;
            DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
        }

        protected void rptNestsForUser_TabGroupsClick(object sender, EventArgs e)
        {
            nestsForUserDataSource = DataProvider.GetGroupNestsForUser(PigeonMaster.CurrentUser.UserID);
            ContactsTab = false;
            GroupsTab = true;
            NotificationsTab = false;
            rptNestsForUser.DataSource = nestsForUserDataSource;
            nestsForUserHeading.InnerText = ContactsTab ? "Contacts" : GroupsTab ? "Nests" : "New Activity";
            rptNestsForUser.DataBind();

            btnAddNest.Visible = true;

            DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
        }

        protected void rptNestsForUser_TabAllNotificationsClick(object sender, EventArgs e)
        {
            nestsForUserDataSource = DataProvider.GetNotificationNestsForUser(PigeonMaster.CurrentUser.UserID);
            ContactsTab = false;
            GroupsTab = false;
            NotificationsTab = true;
            rptNestsForUser.DataSource = nestsForUserDataSource;
            nestsForUserHeading.InnerText = ContactsTab ? "Contacts" : GroupsTab ? "Nests" : "New Activity";
            rptNestsForUser.DataBind();

            btnAddNest.Visible = false;

            DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
        }

        protected void rptNestsForUser_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton nestButton = (LinkButton)e.Item.FindControl("btnNestName");

                if (int.Parse(nestButton.CommandArgument.ToString()) == PigeonMaster.CurrentUser.CurrentNestID)
                {
                    nestButton.BackColor = ColorTranslator.FromHtml("#0071b8");
                }

                GeneralMultiView.SetActiveView(MessageViews);
                MembersMultiView.SetActiveView(NestMembersDetail);
                txtbox.Visible = true;
            }
        }

        protected void rptNestMembers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            bool NestIsPrivate = DataProvider.IsNestPrivate(PigeonMaster.CurrentUser.CurrentNestID);
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField memberLocalRole = ((HiddenField)e.Item.FindControl("hdnLocalRole"));
                HiddenField memberUserId = ((HiddenField)e.Item.FindControl("hdnUserId"));

                if (int.Parse(memberUserId.Value) == PigeonMaster.CurrentUser.UserID || // If the member we are checking is actually the current user
                    PigeonMaster.CurrentUser.CurrentNestRole > 2 ||                     // If the current user's nest role is greater than or equal to 3
                    NestIsPrivate ||                                                    // If the nest is private
                    int.Parse(memberLocalRole.Value) < 2)                               // If the member's role is 0 or 1
                {
                    ((LinkButton)e.Item.FindControl("btnKickUser")).Visible = false;
                    if (PigeonMaster.CurrentUser.CurrentNestRole == 2)
                    {
                        ((LinkButton)e.Item.FindControl("btnRevokeLocalAdmin")).Visible = false;
                        ((LinkButton)e.Item.FindControl("btnGrantLocalAdmin")).Visible = false;
                    }
                }
                else if (int.Parse(memberLocalRole.Value) == 2)
                {
                    ((LinkButton)e.Item.FindControl("btnKickUser")).Visible = true;
                    ((LinkButton)e.Item.FindControl("btnRevokeLocalAdmin")).Visible = true;
                    if (PigeonMaster.CurrentUser.CurrentNestRole == 2)
                    {
                        ((LinkButton)e.Item.FindControl("btnKickUser")).Visible = false;
                        ((LinkButton)e.Item.FindControl("btnRevokeLocalAdmin")).Visible = false;
                        ((LinkButton)e.Item.FindControl("btnGrantLocalAdmin")).Visible = false;
                    }
                }
                else if (int.Parse(memberLocalRole.Value) > 2)
                {
                    ((LinkButton)e.Item.FindControl("btnGrantLocalAdmin")).Visible = true;
                    if (PigeonMaster.CurrentUser.CurrentNestRole == 2)
                    {
                        ((LinkButton)e.Item.FindControl("btnRevokeLocalAdmin")).Visible = false;
                        ((LinkButton)e.Item.FindControl("btnGrantLocalAdmin")).Visible = false;
                    }
                }
            }

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);

            try
            {
                rptSearchResult.DataSource = DataProvider.SearchUser(SearchBox.Text);
                rptSearchResult.DataBind();
                GeneralMultiView.SetActiveView(SearchViews);
                MembersMultiView.SetActiveView(EmptyMembersView);
                txtbox.Visible = false;
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

        protected void rptSearchResult_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "privateMessage":
                    createPrivateNest(e.CommandArgument.ToString());
                    break;
            }
        }

        private void createPrivateNest(string otherUsername)
        {
            int otherUserId = DataProvider.GetUserID(otherUsername);

            // Would check if not blocked here
            try
            {
                // Create the private nest
                int nestId = DataProvider.CreatePrivateNest(PigeonMaster.CurrentUser.UserID, otherUserId);

                // Set current nest to the new nest
                PigeonMaster.CurrentUser.CurrentNestID = nestId;

                ContactsTab = true;
                GroupsTab = false;
                NotificationsTab = false;

                // Refresh the page
                Response.Redirect("nest.aspx");
            }
            catch (OracleException oEx)
            {
                switch (oEx.Number)
                {
                    case 20601:
                        lblSearchError.Text = "You have already messaged this user.";
                        break;
                    default:
                        throw oEx;
                }
            }

            DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
        }

        protected void tmrNestList_Tick(object sender, EventArgs e)
        {
            if (GroupsTab)
            {
                nestsForUserDataSource = DataProvider.GetGroupNestsForUser(PigeonMaster.CurrentUser.UserID);
            }
            else if (ContactsTab)
            {
                nestsForUserDataSource = DataProvider.GetPrivateNestsForUser(PigeonMaster.CurrentUser.UserID);
            }
            else if (NotificationsTab)
            {
                nestsForUserDataSource = DataProvider.GetNotificationNestsForUser(PigeonMaster.CurrentUser.UserID);
            }
            else
            {
                throw new Exception("why");
            }

            rptNestsForUser.DataSource = nestsForUserDataSource;
            rptNestsForUser.DataBind();
        }

        [WebMethod]
        public static string[] ToggleCryptMessage(string text_id)
        {
            DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);

            string[] response = new[] { string.Empty, string.Empty };

            DataTable messageTable = (DataTable)HttpContext.Current.Session["messageTable"];

            DataRow[] queryResults = messageTable.Select($"TEXT_ID = {text_id}");

            bool decrypted = messageTable.Select($"TEXT_ID = {text_id}")[0]["DECRYPTED"].ToString() == "Y";

            messageTable.Select($"TEXT_ID = {text_id}")[0]["DECRYPTED"] = decrypted ? "N" : "Y";
            messageTable.AcceptChanges();

            decrypted = !decrypted;

            if (queryResults.Length == 1)
            {
                response = new[] { queryResults[0][(decrypted ? "MSG_TEXT" : "CYPHER_TEXT")].ToString(), (decrypted ? "decrypted" : "encrypted") };
            }

            HttpContext.Current.Session["messageTable"] = messageTable;

            return response;
        }
        /*********************************************/
        /* Button below the group collections (left) */
        /*********************************************/
        protected void btnAddNest_Click(object sender, EventArgs e)
        {
            GeneralMultiView.SetActiveView(NewNestDetails);
            MembersMultiView.SetActiveView(EmptyMembersView);
            txtbox.Visible = false;

            DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
        }
        /*********************************************/
        /*     Button next to group name text box    */
        /*********************************************/
        protected void btnCreateGroupNest_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(newNestName.Text))
            {
                GeneralMultiView.SetActiveView(NewNestDetails);
                MembersMultiView.SetActiveView(EmptyMembersView);
                txtbox.Visible = false;
                lblNestNameError.Text = "Nest Name is Missing";
                lblNestNameError.Visible = true;
            }
            else
            {
                if (Page.IsValid)
                {
                    try
                    {
                        int nestID = DataProvider.CreateGroupNest(newNestName.Text, PigeonMaster.CurrentUser.UserID);
                        PigeonMaster.CurrentUser.CurrentNestID = nestID;
                        CookieJar.Eat("NestRole");
                        lblNestNameError.Text = string.Empty;
                        Response.Redirect("nest.aspx");
                    }
                    catch (OracleException oEx)
                    {
                        switch (oEx.Number)
                        {
                            case 20605:
                                GeneralMultiView.SetActiveView(NewNestDetails);
                                MembersMultiView.SetActiveView(EmptyMembersView);
                                txtbox.Visible = false;
                                lblNestNameError.Visible = true;
                                lblNestNameError.Text = "Nest Limit Reached.";
                                DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
                                break;
                            default:
                                throw oEx;
                        }
                    }
                }
                else
                {
                    newNestName.Text = string.Empty;
                }
            }

            DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
        }
        /*********************************************/
        /*    Button below the member names(right)   */
        /*********************************************/
        protected void btnSendInvite_Click(object sender, EventArgs e)
        {
            GeneralMultiView.SetActiveView(SearchAndInviteView);
            txtbox.Visible = false;
            DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
        }

        /******************************************************/
        /* Button next to search text box for nest invitation */
        /******************************************************/
        protected void btnSearchAndInvite_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtSearchAndInvite.Text))
            {
                GeneralMultiView.SetActiveView(SearchAndInviteView);
                MembersMultiView.SetActiveView(NestMembersDetail);
                txtbox.Visible = false;
                lblSearchInviteError.Text = "Username is Missing.";
                lblSearchInviteError.Visible = true;
            }
            else
            {
                try
                {
                    rptSearchAndInviteResults.DataSource = DataProvider.InviteUsername(txtSearchAndInvite.Text, PigeonMaster.CurrentUser.CurrentNestID);
                    rptSearchAndInviteResults.DataBind();
                    GeneralMultiView.SetActiveView(SearchAndInviteView);
                    MembersMultiView.SetActiveView(NestMembersDetail);
                    txtbox.Visible = false;

                    lblSearchInviteError.Text = string.Empty;
                }
                catch (OracleException oEx)
                {
                    switch (oEx.Number)
                    {
                        case 20002:
                            lblSearchInviteError.Visible = true;
                            lblSearchInviteError.Text = "Username does not exists.";
                            break;
                        default:
                            throw oEx;
                    }
                }
            }

            DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);

        }
        /******************************************************/
        /* Button next to search text box for nest invitation */
        /******************************************************/
        protected void AddToNest(string otherusername)
        {
            int AddingUserId = DataProvider.GetUserID(otherusername);
            try
            {
                string isInvited = DataProvider.IsUserInvited(AddingUserId, PigeonMaster.CurrentUser.CurrentNestID);
                if (isInvited == "N")
                {
                    DataProvider.AddToNest(AddingUserID: AddingUserId, nestID: PigeonMaster.CurrentUser.CurrentNestID, RequestingUserID: PigeonMaster.CurrentUser.UserID);
                    lblInvitedUser.Text = $"An invite has been sent to {otherusername}!";
                    rptSearchAndInviteResults.DataSource = DataProvider.InviteUsername(txtSearchAndInvite.Text, PigeonMaster.CurrentUser.CurrentNestID);
                    rptSearchAndInviteResults.DataBind();
                }
            }
            catch (OracleException oEx)
            {
                switch (oEx.Number)
                {
                    case 20608:
                        GeneralMultiView.SetActiveView(SearchAndInviteView);
                        lblSearchInviteError.Text = "User is already in the group.";
                        break;
                    case 20810:
                        lblSearchInviteError.Text = "User is already invited to this group.";
                        break;
                    default:
                        throw oEx;
                }
            }
            finally
            {
                DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
            }
        }
        protected void rptSearchAndInviteResults_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "InviteToNest":
                    AddToNest(e.CommandArgument.ToString());
                    GeneralMultiView.SetActiveView(SearchAndInviteView);
                    txtbox.Visible = false;
                    lblSearchInviteError.Visible = true;
                    DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
                    break;
                default:
                    break;
            }
        }
        protected void rptNotificationList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "AcceptInvitation":
                    DataProvider.AcceptGroupInvitation(PigeonMaster.CurrentUser.UserID, Convert.ToInt32(e.CommandArgument), "Y");
                    btnNotifications_Click(source, e);
                    DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
                    break;
                case "RejectInvitation":
                    DataProvider.AcceptGroupInvitation(PigeonMaster.CurrentUser.UserID, Convert.ToInt32(e.CommandArgument), "N");
                    btnNotifications_Click(source, e);
                    DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
                    break;
                default:
                    break;
            }
        }

        protected void rptNestMembers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "KickMember":
                    int RemovingUserID = DataProvider.GetUserID(e.CommandArgument.ToString());
                    DataProvider.RemoveFromNest(RemovingUserID, PigeonMaster.CurrentUser.CurrentNestID, PigeonMaster.CurrentUser.UserID);
                    DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
                    Response.Redirect("nest.aspx");
                    break;
                case "GrantLocalAdmin":
                    int GrantingUserID = DataProvider.GetUserID(e.CommandArgument.ToString());
                    DataProvider.GrantLocalAdmin(PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.CurrentNestID, GrantingUserID);
                    DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
                    Response.Redirect("nest.aspx");
                    break;
                case "RevokeLocalAdmin":
                    int RevokingUserID = DataProvider.GetUserID(e.CommandArgument.ToString());
                    DataProvider.RevokeLocalAdmin(PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.CurrentNestID, RevokingUserID);
                    DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
                    Response.Redirect("nest.aspx");
                    break;
                default:
                    break;
            }
        }
        protected void btnNotifications_Click(object sender, EventArgs e)
        {
            GeneralMultiView.SetActiveView(NotificationsView);
            txtbox.Visible = false;

            rptNotificationList.DataSource = DataProvider.GetUserInvites(PigeonMaster.CurrentUser.UserID);
            rptNotificationList.DataBind();

            rptCustomNotificationsList.DataSource = DataProvider.GetNotifications(PigeonMaster.CurrentUser.UserID);
            rptCustomNotificationsList.DataBind();

            DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
        }

        protected void GetFile(string file_id)
        {
            int id;

            if (int.TryParse(file_id, out id))
            {
                DataRow[] querySet = messageTable.Select("file_id = " + id);

                if (querySet.Length == 1)
                {
                    DataRow message = querySet[0];

                    bool decrypted = message["decrypted"].ToString() == "Y";

                    if (decrypted)
                    {
                        try
                        {
                            string base64String = DataProvider.GetFile(PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.CurrentNestID, id);
                            byte[] imageBytes = Convert.FromBase64String(base64String);

                            DataTable fileMetadata = DataProvider.GetFileMetadata(PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.CurrentNestID, id);

                            Response.ContentType = "image/jpeg";

                            Response.AddHeader("content-disposition", $"attachment;filename={fileMetadata.Rows[0]["name"]}{fileMetadata.Rows[0]["extension"]}");
                            Response.BinaryWrite(imageBytes);
                            Response.End();
                        }
                        catch(FormatException fEx)
                        {
                            // Don't do anything. :)
                        }
                    }
                }
            }
        }

        protected void rptMessages_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int UserRole = DataProvider.GetUserRole(PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.CurrentNestID);
            switch (e.CommandName)
            {
                case "GetFile":
                    GetFile(e.CommandArgument.ToString());
                    DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
                    break;
                case "DeleteMessage":
                    DataProvider.DeleteMessage(PigeonMaster.CurrentUser.UserID, int.Parse(e.CommandArgument.ToString()), PigeonMaster.CurrentUser.CurrentNestID);
                    DataRow[] queryRows = messageTable.Select("text_id = " + int.Parse(e.CommandArgument.ToString()));
                    queryRows[0].Delete();
                    messageTable.AcceptChanges();
                    rptMessages.DataSource = messageTable;
                    rptMessages.DataBind();
                    DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
                    break;
                case "FlagMessage":
                    try
                    {
                        DataProvider.FlagMessage(PigeonMaster.CurrentUser.UserID, int.Parse(e.CommandArgument.ToString()));
                        DataRow[] flagQueryRows = messageTable.Select("text_id = " + int.Parse(e.CommandArgument.ToString()));
                        flagQueryRows[0]["has_flagged"] = "Y";
                        messageTable.AcceptChanges();
                        rptMessages.DataSource = messageTable;
                        rptMessages.DataBind();
                    }
                    catch (OracleException oEx)
                    {
                        switch (oEx.Number)
                        {
                            case 20702:
                                lblFileError.Visible = true;
                                lblFileError.Text = "You may only flag a message once.";
                                break;
                            default:
                                throw oEx;
                        }
                    }
                    DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
                    break;
            }
        }

        protected void rptMessages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                scriptManager.RegisterPostBackControl(e.Item.FindControl("btnDownloadFile"));

                LinkButton DeleteButton = ((LinkButton)e.Item.FindControl("DeleteMessage"));
                LinkButton FlagButton = ((LinkButton)e.Item.FindControl("FlagMessage"));
                scriptManager.RegisterAsyncPostBackControl(DeleteButton);
                scriptManager.RegisterAsyncPostBackControl(FlagButton);

                System.Web.UI.WebControls.Image image = ((System.Web.UI.WebControls.Image)e.Item.FindControl("imgFile"));

                int UserOwner = int.Parse(((HiddenField)e.Item.FindControl("hdnUserId")).Value);
                string Reviewed = ((HiddenField)e.Item.FindControl("hdnReviewed")).Value;
                string UserHasFlagged = ((HiddenField)e.Item.FindControl("hdnHasFlagged")).Value;
                string Decrypted = ((HiddenField)e.Item.FindControl("hdnDecrypted")).Value;
                string MsgFileName = ((HiddenField)e.Item.FindControl("hdnMsgFileName")).Value;
                string FileId = ((HiddenField)e.Item.FindControl("hdnFileId")).Value;

                if ((PigeonMaster.CurrentUser.CurrentNestRole < 3) || PigeonMaster.CurrentUser.UserID == UserOwner)
                {
                    DeleteButton.Visible = true;
                }
                else
                {
                    DeleteButton.Visible = false;
                }

                Label FlagIcon = ((Label)e.Item.FindControl("flagIcon"));

                if (Reviewed == "Y")
                {
                    FlagIcon.CssClass = "fa-solid fa-circle-check";
                    FlagIcon.ForeColor = Color.DarkGreen;
                    FlagButton.Enabled = false;
                }
                else if (UserHasFlagged == "Y")
                {
                    ((Label)e.Item.FindControl("flagIcon")).ForeColor = Color.LightCoral;
                    FlagButton.Enabled = false;
                }

                if (Decrypted == "Y")
                {
                    if (int.TryParse(FileId, out int id))
                    {
                        List<string> imageExtensions = new List<string>() { ".jpg", ".jpeg", ".png", ".webp", ".jfif" };

                        if (imageExtensions.Contains(Regex.Match(MsgFileName, "([.]\\w+)+$").Value))
                        {
                            try
                            {
                                string base64String = DataProvider.GetFile(PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.CurrentNestID, id);

                                base64String = "data:image/png;base64," + base64String;

                                image.ImageUrl = base64String;
                            }
                            catch(FormatException fEx)
                            {
                                image.Visible = false;
                            }
                        }
                    }
                }
            }
        }

        protected void btnAttachFile_Click(object sender, EventArgs e)
        {
            messageContentMultiview.SetActiveView(viewFileMessage);
            txtFileComment.Text = txtComment.Text;

            DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
        }

        protected void btnCancelFile_Click(object sender, EventArgs e)
        {
            messageContentMultiview.SetActiveView(viewNormalMessage);
            txtComment.Text = txtFileComment.Text;

            DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
        }

        protected void btnSendFileMessage_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);

                if (!DataProvider.UserIsBanned(PigeonMaster.CurrentUser.UserID) || !DataProvider.NestIsLocked(PigeonMaster.CurrentUser.CurrentNestID) && !DataProvider.UserIsKicked(PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.CurrentNestID))
                {
                    string processedMessage = Regex.Replace(txtFileComment.Text, @"  +", " ").Trim();

                    if (processedMessage != string.Empty)
                    {
                        if (fileUpload.HasFile)
                        {
                            List<string> acceptableExtensions = new List<string>() { ".txt", ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".jfif", ".png", ".webp" };

                            if (acceptableExtensions.Contains(Regex.Match(fileUpload.FileName, "([.]\\w+)+$").Value))
                            {
                                DataProvider.SendMessage(
                                    userID: PigeonMaster.CurrentUser.UserID,
                                    nestID: PigeonMaster.CurrentUser.CurrentNestID,
                                    messageText: processedMessage,
                                    fileB64: Convert.ToBase64String(fileUpload.FileBytes),
                                    fileName: Regex.Match(fileUpload.FileName, "^[^.]+").Value,
                                    extension: Regex.Match(fileUpload.FileName, "([.]\\w+)+$").Value
                                );

                                lblFileError.Text = string.Empty;
                            }
                            else
                            {
                                lblFileError.Text = "Filetype unsupported. Must be one of the following: ";

                                foreach (string extensionString in acceptableExtensions)
                                {
                                    lblFileError.Text += extensionString + " ";
                                }

                                return;
                            }
                        }
                        else
                        {
                            DataProvider.SendMessage(
                                userID: PigeonMaster.CurrentUser.UserID,
                                nestID: PigeonMaster.CurrentUser.CurrentNestID,
                                messageText: processedMessage
                            );
                        }

                        appendNewMessages();
                        txtComment.Text = txtFileComment.Text = string.Empty;
                        messageContentMultiview.SetActiveView(viewNormalMessage);
                    }
                }
                else
                {
                    Response.Redirect("nest.aspx");
                }
            }
        }

        protected void btnLeaveNest_Click(object sender, EventArgs e)
        {
            int roleUser = DataProvider.GetUserRole(PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.CurrentNestID);
            if (roleUser == 0)
            {
                lblErrorLeaveNest.Text = "Admins cannot leave nests";
            }
            else
            {
                btnCancel.Visible = btnConfirm.Visible = true;
                btnLeaveNest.Visible = false;
            }
        }

        protected void btnDeleteNest_Click(object sender, EventArgs e)
        {
            int roleUser = DataProvider.GetUserRole(PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.CurrentNestID);
            if (roleUser == 1 || roleUser == 0)
            {
                btnCancel.Visible = btnConfirm.Visible = true;
                btnDeleteNest.Visible = false;
            }
            else
            {
                lblErrorLeaveNest.Text = "You cannot do that!";
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

        protected void tmrNotifs_Tick(object sender, EventArgs e)
        {
            if (DataProvider.UserHasNewNotifications(PigeonMaster.CurrentUser.UserID) || DataProvider.GetUserInvites(PigeonMaster.CurrentUser.UserID).Tables[0].Rows.Count > 0)
            {
                iconExclam.Visible = true;
                iconNotifs.ForeColor = Color.Red;
            }
            else
            {
                iconExclam.Visible = false;
                iconNotifs.ForeColor = Color.White;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            int roleUser = DataProvider.GetUserRole(PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.CurrentNestID);
            btnCancel.Visible = btnConfirm.Visible = false;
            if (roleUser > 1)
            {
                btnLeaveNest.Visible = true;
            }
            else
            {
                btnDeleteNest.Visible = true;
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            int roleUser = DataProvider.GetUserRole(PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.CurrentNestID);
            if (roleUser > 1)
            {
                DataProvider.LeaveNest(PigeonMaster.CurrentUser.CurrentNestID, PigeonMaster.CurrentUser.UserID);
                PigeonMaster.CurrentUser.CurrentNestID = -999;
                DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
                Response.Redirect("nest.aspx");
            }
            else
            {
                DataProvider.DeleteGroupNest(PigeonMaster.CurrentUser.CurrentNestID, PigeonMaster.CurrentUser.UserID);
                PigeonMaster.CurrentUser.CurrentNestID = -999;
                DataProvider.LogUserActivity(PigeonMaster.CurrentUser.UserID);
                Response.Redirect("nest.aspx");
            }
        }

        protected void tmrNewActivity_Tick(object sender, EventArgs e)
        {
            if (DataProvider.GetNotificationNestsForUser(PigeonMaster.CurrentUser.UserID).Tables[0].Rows.Count > 0)
            {
                btnNewActivity.CssClass += " new-notifs";
                btnNewActivity.Visible = true;
            }
            else
            {
                btnNewActivity.Visible = false;
            }
        }

        protected void btnToggleNestMute_Click(object sender, EventArgs e)
        {
            DataProvider.ToggleNestMute(PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.CurrentNestID);
            bool nestIsMuted = DataProvider.UserHasNestMuted(PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.CurrentNestID);

            notifBell.CssClass = $"d-flex fa-solid fa-bell{(nestIsMuted ? "-slash" : "")}";

            notifBellText.Text = $"{(nestIsMuted ? " Muted" : "")}";

            notifBellText.ForeColor = notifBell.ForeColor = nestIsMuted ? Color.Red : ColorTranslator.FromHtml("#4A87C6");
        }

        protected void btnLockNest_Click(object sender, EventArgs e)
        {
            DataProvider.ToggleNestLock(PigeonMaster.CurrentUser.UserID, PigeonMaster.CurrentUser.CurrentNestID);
            bool nestIsLocked = DataProvider.NestIsLocked(PigeonMaster.CurrentUser.CurrentNestID);

            lblLockIcon.CssClass = $"d-flex fa-solid fa-lock{(!nestIsLocked ? "-open" : "")}";

            lblLockText.Text = $"{(nestIsLocked ? " Locked" : " Unlocked")}";

            lblLockText.ForeColor = lblLockIcon.ForeColor = nestIsLocked ? Color.Red : ColorTranslator.FromHtml("#4A87C6");

            Response.Redirect("nest.aspx");
        }
    }
}