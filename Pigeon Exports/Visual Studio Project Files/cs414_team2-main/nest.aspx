<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PigeonMaster.Master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="nest.aspx.cs" Inherits="CS414_Team2.nest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Nest</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <script src="Resources/js/nest.js"></script>
    <asp:ScriptManager ID="scriptManager" runat="server" />

    <section style="height: 55px; color: white;">
        <nav style="background-color: #4A87C6;">
            <div class="container" style="width: 100%; margin: 0;">
                <img src="Resources/Images/FullLogo2.png" height="54" width="74" style="width: auto;" />
                <div class="search-bar" style="position: absolute; left: 38%;">
                    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
                        <i class="fa fa-search" style="color: black;"></i>
                        <asp:TextBox ID="SearchBox" Style="border-radius: 10px; padding-left: 5px; padding-right: 5px; width: 90%;" placeholder="Search for a username..." runat="server"></asp:TextBox>
                    </asp:Panel>
                    <asp:Button Style="display: none;" ID="btnSearch" runat="server" CssClass="fa fa-search" Text="" OnClick="btnSearch_Click" UseSubmitBehavior="false" />
                    <asp:Label ID="lblSearchError" Text="" runat="server" Style="display:flex; color:red;"></asp:Label>
                </div>
                <div class="create" style="position: absolute; right: 0; margin-right: 20px;">
                    <asp:UpdatePanel UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="tmrNotifs" EventName="Tick" />
                            <asp:PostBackTrigger ControlID="btnNotifications" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:LinkButton ID="btnNotifications" runat="server" OnClick="btnNotifications_Click" UseSubmitBehavior="false">
                                <asp:Label ID="iconNotifs" runat="server" class="fa fa-bell" Style="font-size: 1.4em;"></asp:Label>
                                <asp:Label ID="iconExclam" runat="server" class="fa fa-exclamation" Style="color: red; font-size: 1.4em;"></asp:Label>
                            </asp:LinkButton>
                            <asp:Timer ID="tmrNotifs" runat="server" Interval="2000" OnTick="tmrNotifs_Tick" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Label ID="lblNavBarUsername" Text="" runat="server" />
                    <div>
                        <button type="button" class="btn btn-secondary dropdown-toggle dropdown-toggle-split" style="background-color: #4A87C6; border: none;" id="MenuReference" data-bs-toggle="dropdown" aria-expanded="false" data-bs-reference="parent">
                            <span class="visually-hidden">Toggle Dropdown</span>
                            <img class="profile-photo" style="border: 2px solid white;" id="imgProfilePicture" runat="server" src="Resources/Images/FullLogo.png" />
                        </button>
                        <div class="dropdown-menu" aria-labelledby="MenuReference">
                            <a class="dropdown-item" href="Team.aspx"><i class="fa fa-users" aria-hidden="true"></i>&nbsp;Our Team</a>
                            <hr class="dropdown-divider">
                            <div id="divAdminButton" runat="server">
                                <a class="dropdown-item" href="admin.aspx" style="color:dodgerblue"><i class="fa fa-user-shield" aria-hidden="true"></i>&nbsp;Admin Dashboard</a>
                            </div>
                            <a class="dropdown-item" href="UserSettings.aspx"><i class="fa fa-cog" aria-hidden="true"></i>&nbsp;User settings</a>
                            <hr class="dropdown-divider">
                            <asp:LinkButton ID="btnLogOut" runat="server" OnClick="btnLogOut_Click" CssClass="dropdown-item" Style="text-decoration: none;">
                                <i class="fa fa-sign-out" aria-hidden="true"></i>
                                &nbsp;Logout
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </nav>
    </section>

    <!-- Top Navigation of Nest Ends Here -->
    <div class="container-flex">
        <div class="row">
            <!-- Side Contact Navigation of Nest -->
            <div class="col-sm-2 col-md-4 col-lg-2">
                <h1 class="header"></h1>
                <%--<input type="text" id="mySearch" placeholder="Search.." title="Type what you want to search">--%>
                <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnContacts" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnGroups" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnNewActivity" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="tmrNewActivity" EventName="Tick" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="btn-group btn-group-xs flex-row w-100">
                            <asp:Button ID="btnContacts" class="btn btn-primary" Text="Contacts" OnClick="rptNestsForUser_TabContactsClick" UseSubmitBehavior="false" runat="server" />
                            <asp:Button ID="btnGroups" class="btn btn-primary" Text="Nests" OnClick="rptNestsForUser_TabGroupsClick" UseSubmitBehavior="false" runat="server" />
                            <asp:Button ID="btnNewActivity" class="btn btn-primary" Text="New" OnClick="rptNestsForUser_TabAllNotificationsClick" UseSubmitBehavior="false" runat="server" />
                        </div>
                        <asp:Timer ID="tmrNewActivity" Enabled="true" Interval="2000" runat="server" OnTick="tmrNewActivity_Tick" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel UpdateMode="Conditional" class="message_list" runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="rptNestsForUser" EventName="ItemDataBound" />
                        <asp:PostBackTrigger ControlID="btnAddNest" />
                    </Triggers>
                    <ContentTemplate>
                        <!-- Tab panes -->
                        <h4 id="nestsForUserHeading" runat="server" class="px-1 pt-1" style="font-weight: bold;"></h4>
                        <ul class="sidebar-navigation">
                            <li>
                                <asp:LinkButton ID="btnAddNest" runat="server" Style="text-decoration: none; color: black;" OnClick="btnAddNest_Click" OnClientClick="btnAddNest_Click">
                                    <i class="fa fa-plus" aria-hidden="true"></i>
                                    <b>Create a Nest!</b>
                                </asp:LinkButton>
                            </li>
                            <asp:Repeater ID="rptNestsForUser" runat="server" OnItemCommand="rptNestsForUser_ItemCommand" OnItemDataBound="rptNestsForUser_ItemDataBound">
                                <ItemTemplate>
                                    <li>
                                        <asp:LinkButton CssClass=' <%#: Eval("is_activity_new").ToString() + Eval("group_css_class").ToString() %>' ID="btnNestName" runat="server" CommandName="SwitchNests" CommandArgument='<%#: Eval("group_id") %>' Style="text-decoration: none; color: black;" data-user-id='<%#: Eval("group_id") %>'>
                                            <div class="d-flex w-100" style="display:inline;">
                                                <img class="profile-photo" src='<%#: Eval("group_icon_url") %>' style="<%#: (Eval("group_icon_url").ToString() != "" ? "border:2px solid dodgerblue;" : "display:none;") %>" />
                                                <i class=' fa <%#: Eval("css_class") %>' aria-hidden="true" style="<%#: (Eval("group_icon_url").ToString() == "" ? "" : "display:none;") %>"></i>
                                                <b class='<%#: Eval("group_css_class") %>' style="margin-top:auto;margin-bottom:auto;margin-left:15px;"><%#: (Eval("group_name").ToString()) %></b>
                                            </div>
                                        </asp:LinkButton>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="defaultItem" Style="margin-left: 15px;" runat="server" Visible='<%# rptNestsForUser.Items.Count == 0 %>' Text="No more Nests to display." />
                                </FooterTemplate>
                            </asp:Repeater>
                        </ul>
                        <asp:Timer ID="tmrNestList" runat="server" Interval="3000" Enabled="true" OnTick="tmrNestList_Tick" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <!-- Message area of Nest -->
            <div class="col-sm-6 col-md-4 col-lg-6 messageArea" style="background-color: #dddd; margin-top: 10px;">
                <div id="multiviewRootDiv" class="row overflow-auto">
                    <!-- Multi view for All views -->
                    <asp:MultiView ID="GeneralMultiView" ActiveViewIndex="-1" runat="server">
                        <!-- VIEW:1 View for Create New Nest -->
                        <asp:View ID="NewNestDetails" runat="server">
                            <div class="login-wrap shadow p-0" style="height: 90vh;">
                                <div class="input-group">
                                    <h2 style="margin-left: auto; margin-right: auto; font-weight: bold;">Give your Nest a name!</h2>
                                    <div class="input-group">
                                        <asp:Panel ID="pnlNewNest" CssClass="d-inline" runat="server" DefaultButton="btnCreateGroupNest" Style="width: 100%; margin-left: auto; margin-right: auto;">
                                            <div class="d-flex" style="margin-left: auto; margin-right: auto;">
                                                <asp:TextBox Style="margin-left: auto; width: 30%;" ID="newNestName" type="groupname" ValidationGroup="NewNest" CausesValidation="true" class="d-flex form-control" placeholder="Nest Name" name="Nest" runat="server"></asp:TextBox>
                                                <asp:LinkButton ID="btnCreateGroupNest" CssClass="d-flex" runat="server" ValidationGroup="NewNest" CausesValidation="true" Style="margin-left: 5px; margin-right: auto; text-decoration: none; color: darkgreen;" OnClick="btnCreateGroupNest_Click">
                                                    <div class="d-flex" style="font-weight:bold;color:darkgreen;border:2px solid darkgreen;padding:5px;border-radius:5px;background-color:lightgreen;">
                                                        <i class="d-flex fa-solid fa-plus" style="color:darkgreen;margin-top:auto;margin-bottom:auto;"></i>
                                                        &nbsp;Create Nest
                                                    </div>
                                                </asp:LinkButton>
                                            </div>
                                            <asp:RequiredFieldValidator ValidationGroup="NewNest" Display="Dynamic" ID="NewNestNameValidators" ErrorMessage="" ControlToValidate="newNestName" runat="server" />
                                            <asp:CustomValidator ValidationGroup="NewNest" Style="margin-left: auto; margin-right: auto;" ID="NewNestNoProfanity" ErrorMessage="Group name can not contain profanity." Display="Dynamic" ControlToValidate="newNestName" ClientValidationFunction="validateNoProfanity" runat="server" />
                                            <asp:Label ID="lblNestNameError" runat="server" Visible="false"></asp:Label>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </asp:View>
                        <!-- VIEW:2 Search Result in Repeater-->
                        <asp:View ID="SearchViews" runat="server">
                            <div class="shadow" style="height: 90vh;">
                                <asp:Repeater ID="rptSearchResult" runat="server" OnItemCommand="rptSearchResult_ItemCommand">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnSearchUsername" runat="server" Style="text-decoration: none; color: black;" CommandArgument='<%#: Eval("user_name") %>' CommandName="privateMessage">
                                        <div style="display:inline;" class="list-group-item list-group-item-action flex-column align-items-start d-flex w-100">
                                            <div class="d-flex" style="display:inline;">
                                                <img style="margin-top:auto;margin-bottom:auto;border:2px solid dodgerblue;" src='<%#: Eval("user_icon_url") %>' class="profile-photo"/>
                                                <h5 style="margin-top:auto;margin-bottom:auto;margin-left:15px;" class="mb-1"><%#: Eval("user_name") %></h5>
                                            </div>
                                        </div>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </asp:View>
                        <!-- VIEW:3 Messages & Nests in Repeater -->
                        <asp:View ID="MessageViews" runat="server">
                            <div class="shadow position-relative" style="height: 77vh; width: 100%;">
                                <div class="position-absolute bottom-0 start-0 end-0">
                                    <div id="allMessagesDiv" class="messageDiv border align-self-end overflow-auto" style="max-height: 77vh;" onload="scrollToBottom()">
                                        <asp:UpdatePanel ID="messages" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="rptMessages" EventName="ItemDataBound" />
                                                <asp:AsyncPostBackTrigger ControlID="rptMessages" EventName="ItemCommand" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:Repeater ID="rptMessages" OnItemDataBound="rptMessages_ItemDataBound" OnItemCommand="rptMessages_ItemCommand" runat="server">
                                                    <ItemTemplate>
                                                        <div data-msg-id="<%#: Eval("text_id") %>" class="messageContent">
                                                            <div class="list-group-item list-group-item-action list-group-item-light rounded-0">
                                                                <div class="media">
                                                                    <div class="media-body ml-4">
                                                                        <div class="d-flex align-items-center mb-1" style="padding: 30px;">
                                                                            <div class="d-flex" style="position: absolute; left: 0; margin-left: 20px; display: inline;">
                                                                                <img class="profile-photo" style="border: 2px solid dodgerblue;" src='<%#: Eval("user_icon_url") %>' alt="User Icon" title='<%#: Eval("user_name") %>' />
                                                                                <i style='<%#: (Eval("local_role").ToString() == "0" ? "color:dodgerblue;font-weight:bold;" : "") %>' class="<%#: (Eval("local_role").ToString() == "0" ? "fa-solid fa-user-shield" : Eval("local_role").ToString() == "1" ? "fa-solid fa-dove" : Eval("local_role").ToString() == "2" ? "fa-solid fa-crow" : "") %>"></i>
                                                                                <h5 style='<%#: (Eval("local_role").ToString() == "0" ? "color:dodgerblue;font-weight:bold;" : "") %>margin-top:auto; margin-bottom: auto; margin-left: 15px;'><%#: Eval("user_name") %></h5>
                                                                                <small class="small font-weight-bold" style="margin-top: auto; margin-bottom: auto; margin-left: 15px;"><%#: Eval("time_sent") %></small>
                                                                            </div>
                                                                            <div style="margin-top: auto; margin-bottom: auto; position: absolute; right: 0;">
                                                                                <div style="margin-right: 20px; margin-top: auto; margin-bottom: auto; font-size: 1.2em;" class="small font-weight-bold d-flex">
                                                                                    &nbsp;
                                                                                <asp:LinkButton Style="text-decoration: none; color: darkgray; margin-top: auto; margin-bottom: auto;" CssClass="d-flex" CausesValidation="false" ID="FlagMessage" runat="server" CommandName="FlagMessage" CommandArgument='<%#: Eval("text_id") %>' UseSubmitBehavior="false">
                                                                                    <asp:Label ID="flagIcon" runat="server" CssClass='fa-solid fa-flag' Style="margin-top: auto; margin-bottom: auto;"></asp:Label>
                                                                                </asp:LinkButton>
                                                                                    <asp:LinkButton Style="text-decoration: none; color: darkgray; margin-top: auto; margin-bottom: auto;" CssClass="d-flex" CausesValidation="false" ID="DeleteMessage" Visible="false" runat="server" CommandName="DeleteMessage" CommandArgument='<%#: Eval("text_id") %>' UseSubmitBehavior="false">
                                                                                    &nbsp;&nbsp;<i class='fa-solid fa-trash-can'style="margin-top:auto;margin-bottom:auto;"></i>
                                                                                    </asp:LinkButton>
                                                                                    <asp:HiddenField ID="hdnUserId" runat="server" Value='<%#: Eval("user_id") %>' />
                                                                                    <asp:HiddenField ID="hdnReviewed" runat="server" Value='<%#: Eval("reviewed") %>' />
                                                                                    <asp:HiddenField ID="hdnHasFlagged" runat="server" Value='<%#: Eval("has_flagged") %>' />
                                                                                    <asp:HiddenField ID="hdnDecrypted" runat="server" Value='<%#: Eval("decrypted") %>' />
                                                                                    <asp:HiddenField ID="hdnMsgFileName" runat="server" Value='<%#: Eval("msg_file") %>' />
                                                                                    <asp:HiddenField ID="hdnFileId" runat="server" Value='<%#: Eval("file_id") %>' />
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <p class="messageLabel <%#: (Eval("decrypted").ToString() == "N" ? "encrypted" : "decrypted") %> font-italic text-muted mb-0 text-small"><%#: (Eval("decrypted").ToString() == "N" ? Eval("cypher_text") : Eval("msg_text")) %></p>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="fileDiv list-group-item list-group-item-action list-group-item-dark rounded-0" data-file-id='<%#: Eval("file_id") %>' style='<%#: ((Eval("msg_file").ToString() == "") ? "display:none;" : "") %>'>
                                                            <i class='<%#: (Eval("decrypted").ToString() == "N" ? "fa fa-file" : "fa fa-download") %>' style='<%#: (Eval("decrypted").ToString() == "N" ? "color: gray;" : "color: dodgerblue;") %>'></i>&nbsp;<asp:LinkButton CausesValidation="false" ID="btnDownloadFile" runat="server" Style='<%#: ((Eval("decrypted").ToString() == "N" || Eval("msg_file").ToString() == "") ? "display:none;" : "") %>' CommandArgument='<%#: Eval("file_id") %>' CommandName="GetFile" CssClass="fileLink" Text='<%#: Eval("msg_file") %>'></asp:LinkButton>
                                                            <div style="max-width: 600px; max-height: 450px;">
                                                                <asp:Image ID="imgFile" CssClass="shadow" runat="server" Style='<%#: ((Eval("decrypted").ToString() == "N" || Eval("msg_file").ToString() == "") ? "display:none;" : "display:block;max-height:450px;max-width:600px;") %>' />
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <asp:Timer ID="timerDBPoll" Interval="3000" Enabled="false" runat="server" OnTick="timerDBPoll_Tick" />
                                                <script>
                                                    let d = document.getElementById("allMessagesDiv");
                                                    d.scroll(0, d.scrollHeight);
                                                </script>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </asp:View>
                        <!-- VIEW:4 Nest: Searches for user to invite -->
                        <asp:View ID="SearchAndInviteView" runat="server">
                            <div class="shadow" style="height: 90vh;">
                                <div class="login-wrap p-0">
                                    <p>Enter a username to invite</p>
                                    <asp:Panel runat="server" ID="pnlSearchAndInvite" DefaultButton="btnSearchAndInvite">
                                        <asp:TextBox ID="txtSearchAndInvite" runat="server" type="username" class="form-control" placeholder="Username" name="Name"></asp:TextBox>
                                    </asp:Panel>
                                    <asp:LinkButton ID="btnSearchAndInvite" runat="server" Text="" OnClick="btnSearchAndInvite_Click" CommandArgument='<%#: Eval("user_name") %>'></asp:LinkButton>
                                </div>
                                <asp:Label ID="lblInvitedUser" Text="" runat="server" style="color:green;" />
                                <asp:Label ID="lblSearchInviteError" runat="server" Visible="false"></asp:Label>
                                <asp:Repeater ID="rptSearchAndInviteResults" runat="server" OnItemCommand="rptSearchAndInviteResults_ItemCommand">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnInviteToNest" runat="server" Style="text-decoration: none; color: black;" CommandArgument='<%#: Eval("user_name") %>' CommandName="InviteToNest">
                                        <div style="display:inline;" class="list-group-item list-group-item-action flex-column align-items-start d-flex w-100">
                                            <div class="d-flex" style="display:inline;">
                                                <img style="margin-top:auto;margin-bottom:auto;border:2px solid dodgerblue;" src='<%#: Eval("user_icon_url") %>' class="profile-photo"/>
                                                <h5 style="margin-top:auto;margin-bottom:auto;margin-left:15px;" class="mb-1"><%#: Eval("user_name") %></h5>
                                            </div>
                                        </div>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </asp:View>
                        <!-- VIEW:5 User Notifications: Invite requests -->
                        <asp:View ID="NotificationsView" runat="server">
                            <div class="shadow" style="height: 90vh; padding-top: 15px;">
                                <h4>Group Invitations</h4>
                                <div class="overflow-auto" style="max-height: auto;">
                                    <asp:Repeater ID="rptNotificationList" runat="server" OnItemCommand="rptNotificationList_ItemCommand">
                                        <ItemTemplate>
                                            <div class="list-group-item list-group-item-action align-items-start d-flex w-100 justify-content-between">
                                                <div class="d-flex mt-auto mb-auto p-2">
                                                    <i class="fa fa-users mt-auto mb-auto mx-2" aria-hidden="true"></i>
                                                    <h5 class="mb-1 pt-1"><%#: Eval("group_name") %></h5>
                                                </div>
                                                <div class="d-flex mb-auto mt-auto">
                                                    <asp:Button ID="btnAcceptInvite" class="d-flex btn btn-primary mx-2" Text="Accept" runat="server" CommandArgument='<%#: Eval("group_id") %>' CommandName="AcceptInvitation"></asp:Button>
                                                    <div class="vr"></div>
                                                    <asp:Button ID="btnRejectInvite" class="d-flex btn btn-danger mx-2" Text="Reject" runat="server" CommandArgument='<%#: Eval("group_id") %>' CommandName="RejectInvitation"></asp:Button>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label Visible="<%# rptNotificationList.Items.Count == 0 %>" Style="margin-left: 15px;" Text="No group invitations to show." runat="server" />
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                                <h4 style="padding-top: 30px;">Other Notifications</h4>
                                <div class="overflow-auto" style="max-height: auto">
                                    <asp:Repeater ID="rptCustomNotificationsList" runat="server">
                                        <ItemTemplate>
                                            <div class="list-group-item list-group-item-action flex-column align-items-start d-flex w-100 justify-content-between">
                                                <small><%#: Eval("time_sent") %></small>
                                                <h5><%#: Eval("notif_body") %></h5>
                                            </div>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label Visible="<%# rptCustomNotificationsList.Items.Count == 0 %>" Style="margin-left: 15px;" Text="No other notifications to show." runat="server" />
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </asp:View>
                        <!-- VIEW:6 Empty -->
                        <asp:View ID="viewEmpty" runat="server">
                            <div class="shadow" style="height: 90vh; padding-top: 15px;">
                                <h3>Welcome to&nbsp;<b>Pigeon</b>!</h3>
                                <p>From here, you can...</p>
                                <ul>
                                    <li>
                                        <p>Search for a user at the top of your screen to start a private conversation</p>
                                    </li>
                                    <li>
                                        <p>Click on an existing conversation using the menu on the left</p>
                                    </li>
                                    <li>
                                        <p>Create a new Nest (group chat) from the menu on the left</p>
                                    </li>
                                </ul>
                            </div>
                        </asp:View>
                    </asp:MultiView>
                </div>
                <!-- Textbox -->
                <div class="footer">
                    <div id="divMessageControls" style="width: 100%;" class="row align-self-end position-relative">
                        <div id="txtbox" runat="server" class="jumbotron">
                            <div class="mb-3 mt-3">
                                <asp:MultiView ID="messageContentMultiview" ActiveViewIndex="0" runat="server">
                                    <asp:View ID="viewLockedNest" runat="server">
                                        <h3 style="color: red;">This Nest has been locked by a system administrator.</h3>
                                    </asp:View>
                                    <asp:View ID="viewNormalMessage" runat="server">
                                        <small><em>Click on a message to toggle encryption! Decrypting a message will also begin loading attached files.</em></small>
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSendMessage" EventName="Click" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <div class="d-flex justify-content-end">
                                                    <asp:TextBox ID="txtComment" placeholder="Type your message, and press enter to send..." AutoCompleteType="Disabled" TextMode="SingleLine" ValidationGroup="vlgProfanityAPI" CssClass="txtMessage form-control" runat="server"></asp:TextBox>
                                                    <asp:Button ID="btnSendMessage" CssClass="sendMessageButton btn btn-primary mx-2" ValidationGroup="vlgProfanityAPI" Text="Send" OnClick="btnSendMessage_Click" CausesValidation="true" runat="server"></asp:Button>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:LinkButton ID="btnAttachFile" Text="Attach a file" OnClick="btnAttachFile_Click" runat="server" />
                                    </asp:View>
                                    <asp:View ID="viewFileMessage" runat="server">
                                        <small><b>Your message will be hidden until a system administrator has reviewed and approved the file.</b></small>
                                        <div class="d-flex justify-content-end">
                                            <asp:TextBox ID="txtFileComment" placeholder="A message must be sent with a file" AutoCompleteType="Disabled" TextMode="SingleLine" ValidationGroup="vlgProfanityAPI" CssClass="txtMessage form-control" runat="server"></asp:TextBox>
                                            <asp:Button ID="btnSendFileMessage" CssClass="sendMessageButton btn btn-primary mx-2" ValidationGroup="vlgProfanityAPI" Text="Send" OnClick="btnSendFileMessage_Click" CausesValidation="true" runat="server"></asp:Button>
                                        </div>
                                        <div class="content-flex">
                                            <b>File size limit: 1MB</b>
                                            <asp:FileUpload ID="fileUpload" AllowMultiple="false" runat="server" />
                                            <asp:LinkButton ID="btnCancelFile" CausesValidation="false" Text="Cancel" OnClick="btnCancelFile_Click" runat="server" />
                                            <asp:CustomValidator ID="csvFileSize" runat="server"
                                                ValidationGroup="vlgProfanityAPI"
                                                ToolTip="File size can not exceed 1MB."
                                                ErrorMessage="File size exceeds the 1MB limit."
                                                ControlToValidate="fileUpload"
                                                ClientValidationFunction="checkfilesize();"
                                                Display="Dynamic" />
                                            <asp:Label ID="lblFileError" Style="color: red;" Text="" runat="server" />
                                        </div>
                                    </asp:View>
                                </asp:MultiView>
                                <asp:CustomValidator ErrorMessage="Message can not contain profanity." Display="Dynamic" ControlToValidate="txtComment" ValidationGroup="vlgProfanityAPI" ClientValidationFunction="validateNoProfanity" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Message area of Nest Ends here-->

            <!-- Chat Member of Nest List-->
            <asp:MultiView ID="MembersMultiView" ActiveViewIndex="-1" runat="server">
                <asp:View ID="NestMembersDetail" runat="server">
                    <div class="col-sm-4 col-md-4 col-lg-4">
                        <div class="row">
                            <div class="Member">
                                <div id="nestmembers" class="list-group" runat="server">
                                    <h1 id="nestMembersHeader" runat="server" class="display-4 text-break" style="margin-top: 10px; color: black;"></h1>
                                    <div class="d-flex" style="margin-top: 15px;">
                                        <asp:LinkButton ID="btnLockNest" runat="server" Style="text-decoration: none;" OnClick="btnLockNest_Click">
                                            <div class="d-flex" style="font-weight: bold; color: dodgerblue; padding: 5px;">
                                                <asp:Label ID="lblLockIcon" runat="server" CssClass="d-flex fa-solid fa-lock-open" ForeColor="#4A87C6" Style="font-size: 1.4em; margin-top: auto; margin-bottom: auto;"></asp:Label>
                                                <asp:Label ID="lblLockText" runat="server" />
                                            </div>
                                        </asp:LinkButton>
                                    </div>
                                    <div class="d-flex justify-content-between w-100">
                                        <div class="d-flex" style="margin-top: 15px;">
                                            <asp:LinkButton ID="btnSendInvite" runat="server" Style="text-decoration: none;" OnClick="btnSendInvite_Click">
                                                <div class="d-flex" style="font-weight:bold;color:darkgreen;border:2px solid darkgreen;padding:5px;border-radius:5px;background-color:lightgreen;">
                                                    <i class="d-flex fa-solid fa-plus" style="color:darkgreen;margin-top:auto;margin-bottom:auto;"></i>
                                                    &nbsp;Invite User
                                                </div>
                                            </asp:LinkButton>
                                        </div>
                                        <div class="d-flex" style="margin-top: 15px;">
                                            <asp:LinkButton ID="btnToggleNestMute" runat="server" Style="text-decoration: none;" OnClick="btnToggleNestMute_Click">
                                                <div class="d-flex" style="font-weight: bold; color: dodgerblue; padding: 5px;">
                                                    <asp:Label ID="notifBell" runat="server" CssClass="d-flex fa-solid fa-bell" ForeColor="#4A87C6" Style="font-size: 1.4em; margin-top: auto; margin-bottom: auto;"></asp:Label>
                                                    <asp:Label ID="notifBellText" runat="server" />
                                                </div>
                                            </asp:LinkButton>
                                        </div>
                                        <div class="d-flex" style="margin: 15px;">
                                            <asp:LinkButton runat="server" ID="btnConfirm" Style="text-decoration: none;" Visible="false" OnClick="btnConfirm_Click" CausesValidation="false" UseSubmitBehavior="false" CssClass="px-1">
                                                <div class="d-flex" style="font-weight:bold;color:black;border:2px solid black;padding:5px;border-radius:5px;background-color:red;">
                                                    <i class="d-flex fa-solid fa-check" style="color:black;margin-top:auto;margin-bottom:auto;"></i>
                                                    &nbsp;Confirm
                                                </div>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" ID="btnCancel" Style="text-decoration: none;" Visible="false" OnClick="btnCancel_Click" CausesValidation="false" UseSubmitBehavior="false">
                                                <div class="d-flex" style="font-weight:bold;color:black;border:2px solid black;padding:5px;border-radius:5px;background-color:green;">
                                                    <i class="d-flex fa-solid fa-ban" style="color:black;margin-top:auto;margin-bottom:auto;"></i>
                                                    &nbsp;Cancel
                                                </div>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" ID="btnLeaveNest" Style="text-decoration: none;" Visible="false" OnClick="btnLeaveNest_Click" CausesValidation="false" UseSubmitBehavior="false">
                                                <div class="d-flex" style="font-weight:bold;color:darkred;border:2px solid darkred;padding:5px;border-radius:5px;background-color:lightcoral;">
                                                    <i class="d-flex fa-solid fa-arrow-right-from-bracket" style="color:darkred;margin-top:auto;margin-bottom:auto;"></i>
                                                    &nbsp;Leave Nest
                                                </div>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" Style="text-decoration: none;" ID="btnDeleteNest" Visible="false" CausesValidation="false" UseSubmitBehavior="false" OnClick="btnDeleteNest_Click">
                                                <div class="d-flex" style="font-weight:bold;color:darkred;border:2px solid darkred;padding:5px;border-radius:5px;background-color:lightcoral;">
                                                    <i class="d-flex fa-solid fa-trash-can" style="color:darkred;margin-top:auto;margin-bottom:auto;"></i>
                                                    &nbsp;Delete Nest
                                                </div>
                                            </asp:LinkButton>
                                            <asp:Label runat="server" ID="lblErrorLeaveNest" Text=""></asp:Label>
                                        </div>
                                    </div>
                                    <div class="position-relative" style="width: 100%; height: 70vh;">
                                        <div class="position-absolute start-0 end-0 top-0 overflow-auto" style="margin-right: 10px; max-height: 70vh;">
                                            <asp:Repeater ID="rptNestMembers" runat="server" OnItemCommand="rptNestMembers_ItemCommand" OnItemDataBound="rptNestMembers_ItemDataBound">
                                                <ItemTemplate>
                                                    <div class='list-group-item list-group-item-action flex-column align-items-start <%#: Eval("css_class") %>'>
                                                        <div class="d-flex w-100" style="display: inline;">
                                                            <img class="profile-photo" src='<%#: Eval("user_icon_url") %>' alt="User Icon" style="border: 2px solid dodgerblue;" title='<%#: Eval("user_name") %>' />
                                                            <h5 style="margin-top: auto; margin-bottom: auto; margin-left: 15px;">
                                                                <%#: Eval("user_name") %>
                                                                <asp:LinkButton Visible="false" Style="margin-right: 60px; right: 0; position: absolute;" runat="server" ID="btnGrantLocalAdmin" UseSubmitBehavior="false" ToolTip="Promote to Local Admin" CausesValidation="false" CommandArgument='<%#: Eval("user_name") %>' CommandName="GrantLocalAdmin">
                                                                    <i class="fa-regular fa-star" style="color:gold;border-radius:10px;padding:5px;margin-top:auto;margin-bottom:auto;"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton Visible="false" Style="margin-right: 60px; right: 0; position: absolute;" runat="server" ID="btnRevokeLocalAdmin" UseSubmitBehavior="false" ToolTip="Revoke Local Admin" CausesValidation="false" CommandArgument='<%#: Eval("user_name") %>' CommandName="RevokeLocalAdmin">
                                                                    <i class="fa-solid fa-star" style="color:gold;border-radius:10px;padding:5px;margin-top:auto;margin-bottom:auto;"></i>
                                                                </asp:LinkButton>
                                                                <asp:LinkButton Style="margin-right: 20px; right: 0; position: absolute;" ID="btnKickUser" runat="server" UseSubmitBehavior="false" ToolTip="Kick User" CausesValidation="false" CommandArgument='<%#: Eval("user_name") %>' CommandName="KickMember">
                                                                    <i class="fa-solid fa-user-xmark" style="color:red;background-color:white;border:1px solid white; border-radius:10px;padding:5px;margin-top:auto;margin-bottom:auto;"></i>
                                                                </asp:LinkButton>
                                                            </h5>
                                                        </div>
                                                        <small>
                                                            <%#: Eval("role_string") %>
                                                        </small>
                                                        <asp:HiddenField ID="hdnLocalRole" runat="server" Value='<%#: Eval("local_role") %>' />
                                                        <asp:HiddenField ID="hdnUserId" runat="server" Value='<%#: Eval("user_id") %>' />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="EmptyMembersView" runat="server">
                    <div class="col-sm-4 col-md-4 col-lg-4">
                        <div class="row">
                            <div>
                                <div id="EmptySide" class="list-group" runat="server">
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </div>
    </div>
</asp:Content>
