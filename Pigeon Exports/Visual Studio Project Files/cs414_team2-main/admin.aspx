<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PigeonMaster.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="CS414_Team2.admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Admin Dashboard</title>

    <style>
        .banned:hover, .banned {
            background-color: coral;
        }

        body {
            overflow: auto;
        }
    </style>

    <script>
        let d = document.getElementById("scrolly");
        d.scroll(0, d.scrollHeight);
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <asp:ScriptManager ID="scriptManager" runat="server" />
    <div class="m-4">
        <nav class="navbar navbar-expand-lg navbar-light" style="background-color:#4A87C6;">
            <div class="container">
                <a class="navbar-brand" href="nest.aspx">
                    <img src="Resources/Images/FullLogo2.png" height="54" width="74" style="width: auto;" />
                </a>
                <button type="button" class="navbar-toggler" data-bs-toggle="collapse" data-bs-target="#navbarCollapse">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarCollapse">
                    <div class="navbar-nav">
                        <asp:Button ID="Button3" runat="server" Text="Admin Dashboard" Class="btn btn-primary mx-1" OnClick="btnSwitchToAdminDashBoardView_Click" UseSubmitBehavior="false" />
                        <asp:Button ID="Button2" runat="server" Text="Nest Messaging Search" Class="btn btn-primary mx-1" OnClick="btnSwitchToGroupMessagingSearchView_Click" UseSubmitBehavior="false" />
                        <asp:Button ID="Button5" runat="server" Text="User Messaging Search" Class="btn btn-primary mx-1" OnClick="btnSwitchToUserMessagingSearchView_Click" UseSubmitBehavior="false" />
                        <asp:Button ID="Button1" runat="server" Text="Messaging Statistics" Class="btn btn-primary mx-1" OnClick="btnSwitchToMessagingStasticsView_Click" UseSubmitBehavior="false" />

                    </div>
                    <div class="navbar-nav ms-auto">
                        <asp:Button runat="server" Text="Nests" Class="btn btn-primary mx-1" OnClick="btnSwitchToNest_Click" UseSubmitBehavior="false" />
                    </div>
                </div>
            </div>
        </nav>
    </div>

    <section class="container-flex">
        <asp:MultiView ID="adminMultiView" ActiveViewIndex="0" runat="server">
            <asp:View ID="adminDashboard" runat="server">
                <div class="container">
                    <div class="row">
                        <div class="col-sm">
                            Flagged Messages
                        </div>
                        <div class="col-sm">
                            All Users
                        </div>
                    </div>
                </div>
                <div class="container">
                    <div class="row">
                        <div class="col-sm-6 col-md-4 col-lg-6 p-2 border" style="height: 70vh;">
                            <div style="overflow-y: auto; max-height: 70vh;">
                                <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="rptMessages" EventName="ItemDataBound" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:Repeater ID="rptMessages" OnItemCommand="rptMessages_ItemCommand" OnItemDataBound="rptMessages_ItemDataBound" runat="server">
                                            <ItemTemplate>
                                                <div data-msg-id="<%#: Eval("text_id") %>" class="messageContent">
                                                    <a class="list-group-item list-group-item-action list-group-item-light rounded-0">
                                                        <div class="media">
                                                            <i class="fa fa-user"></i>
                                                            <div class="media-body ml-4">
                                                                <div class="d-flex align-items-center justify-content-between mb-1">
                                                                    <h6 id="msgUserName" class="mb-0"><%#: Eval("user_name") %></h6>
                                                                    <small class="small font-weight-bold"><%#: Eval("time_sent") %>&nbsp;<asp:Button class="btn btn-info" CausesValidation="false" ID="ApproveMessage" runat="server" Text="Approve" CommandName="UnFlag" CommandArgument='<%#: Eval("text_id") %>' UseSubmitBehavior="false"></asp:Button><asp:Button CausesValidation="false" ID="DeleteMessage" class="btn btn-danger" runat="server" Text="Delete" CommandName="DeleteMessage" CommandArgument='<%#: Eval("text_id") %>' UseSubmitBehavior="false"></asp:Button></small>
                                                                </div>
                                                                <p class="messageLabel font-italic text-muted mb-0 text-small"><%#: Eval("msg_text") %></p>
                                                            </div>
                                                        </div>
                                                    </a>
                                                </div>
                                                <div class="fileDiv list-group-item list-group-item-action list-group-item-dark rounded-0" data-file-id='<%#: Eval("file_id") %>' style="<%#: ((Eval("msg_file").ToString() == "") ? "display:none;" : "") %>">
                                                    <i class="fa fa-download" style="color: dodgerblue;"></i>&nbsp;<asp:LinkButton CausesValidation="false" ID="btnDownloadFile" runat="server" CommandArgument='<%#: (Eval("file_id") + "|" + Eval("group_id")) %>' CommandName="GetFile" CssClass="fileLink" Text='<%#: Eval("msg_file") %>'></asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="col-sm-4 col-md-4 col-lg-4 border">
                            <div class="row">
                                <div style="height: 70vh;">
                                    <div id="nestmembers" class="list-group" style="overflow-y: auto; max-height: 70vh;" runat="server">
                                        <h1 id="nestMembersHeader" runat="server" class="display-4"></h1>
                                        <small></small>
                                        <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="rptUsers" EventName="ItemDataBound" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:Repeater ID="rptUsers" runat="server" OnItemCommand="rptUsers_ItemCommand" OnItemDataBound="rptUsers_ItemDataBound">
                                                    <ItemTemplate>
                                                        <div class='list-group-item list-group-item-action flex-column align-items-start color-red <%#: Eval("css_class") %>'>
                                                            <div class="d-flex w-100 justify-content-between overflow-hidden">
                                                                <h6 style="margin-top: auto; margin-bottom: auto; margin-right: 105px;">
                                                                    <%#: Eval("user_name") %>
                                                                    <asp:Button Style="margin-right: 20px; right: 0; position: absolute;" ID="btnBanUser" class="btn btn-danger" Visible="false" runat="server" UseSubmitBehavior="false" CausesValidation="false" Text="Ban" CommandArgument='<%#: Eval("user_name") %>' CommandName="BanMember" />
                                                                    <asp:Button Style="margin-right: 20px; right: 0; position: absolute;" ID="btnUnBanUser" class="btn btn-info" Visible="false" runat="server" UseSubmitBehavior="false" CausesValidation="false" Text="UnBan" CommandArgument='<%#: Eval("user_name") %>' CommandName="UnBanMember" />
                                                                    <asp:HiddenField ID="hdnBannedStatus" runat="server" Value='<%#: Eval("banned") %>' />
                                                                </h6>
                                                            </div>
                                                            <small>
                                                                <div><%#: "User Id: " + Eval("user_id") %></div>
                                                                <div><%#: Eval("user_email") %></div>
                                                                <%#: Eval("role_string") %>
                                                            </small>
                                                        </div>
                                                        <div class="fileDiv list-group-item list-group-item-action list-group-item-dark rounded-0" style="<%#: ((Eval("email_confirmation").ToString() == "") ? "display:none;" : "") %>">
                                                            <i class="fa-solid fa-envelope-circle-check" style="color: dodgerblue;"></i>&nbsp;<asp:LinkButton CausesValidation="false" ID="btnDownloadFile" runat="server" Text='<%#: Eval("user_email") %>' CommandArgument='<%#: (Eval("user_email")) %>' CommandName="ConfirmEmail"></asp:LinkButton>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:View>

            <asp:View ID="adminSearchAndViewUsers" runat="server">
                <div class="container">
                    <asp:Panel runat="server" ID="Panel1" DefaultButton="btnUsernameSearch">
                        <i class="fa fa-search" style="color: black;"></i>
                        <asp:TextBox ID="txtUsernameSearch" CssClass="mx-auto" Style="border-radius: 10px; width: 40%;" runat="server"></asp:TextBox>
                        <asp:Button Style="display: none;" ID="btnUsernameSearch" runat="server" CssClass="fa fa-search" Text="" OnClick="btnUsernameSearch_Click" UseSubmitBehavior="false" />
                    </asp:Panel>
                    <asp:Label ID="lblUsernameSearchError" Text="" runat="server" Style="position: absolute; right: 25%"></asp:Label>
                    <br>
                    <br />

                    <div class="row">
                        <div class="col-sm-3 col-md-4 col-lg-3">
                            User Search
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-6">
                            Active Searched User Messages
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            Active Searched User Nest
                        </div>
                    </div>
                    <div id="scrolly" class="row">
                        <div class="col-sm-3 col-md-4 col-lg-3 p-2 border align-self-start overflow-auto" style="max-height: 500px;">
                            <asp:Repeater ID="rptUsernamesSearch" runat="server" OnItemCommand="rptUsernamesSearch_ItemCommand">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnSearchUsername" runat="server" Style="text-decoration: none; color: black;" CommandArgument='<%#: Eval("user_id") %>' CommandName="viewUserMessages">
                                            <div style="display:inline;" class="list-group-item list-group-item-action flex-column align-items-start d-flex w-100">
                                                <div class="d-flex" style="display:inline;">
                                                    <h5 style="margin-top:auto;margin-bottom:auto;margin-left:15px;" class="mb-1"> <div><%#: "User Id: " + Eval("user_id") %></div><div><%#: "User Name: " + Eval("user_name") %></div></h5>
                                                </div>
                                            </div>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-6 p-2 border align-self-start overflow-auto" style="max-height: 500px;">
                            <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="rptUsernameMessages" EventName="ItemDataBound" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Repeater ID="rptUsernameMessages" OnItemCommand="rptUsernameMessages_ItemCommand" OnItemDataBound="rptUsernameMessages_ItemDataBound" runat="server">
                                        <ItemTemplate>
                                            <div data-msg-id="<%#: Eval("text_id") %>" class="messageContent">
                                                <div class="list-group-item list-group-item-action list-group-item-light rounded-0">
                                                    <div class="media">
                                                        <i class="fa fa-user"></i>
                                                        <div class="media-body ml-4">
                                                            <div class="d-flex align-items-center justify-content-between mb-1">
                                                                <h6 id="msgGroupName" class="mb-0"><%#: Eval("group_name") %> [<%#: Eval("group_id") %>]</h6>
                                                                <h6 id="msgUserName2" class="mb-0"><%#: Eval("user_name") %></h6>
                                                                <small class="small font-weight-bold"><%#: Eval("time_sent") %>
                                                                    &nbsp;
                                                                    <asp:Button class="btn btn-danger" CausesValidation="false" ID="DeleteMessage" runat="server" Text="Delete" CommandName="DeleteMessage" CommandArgument='<%#: (Eval("text_id") + "|" + Eval("user_id") + "|" + Eval("group_id")) %>' UseSubmitBehavior="false"></asp:Button>
                                                                </small>
                                                            </div>
                                                            <p class="messageLabel font-italic text-muted mb-0 text-small"><%#: Eval("msg_text") %></p>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="fileDiv list-group-item list-group-item-action list-group-item-dark rounded-0" data-file-id='<%#: Eval("file_id") %>' style="<%#: ((Eval("msg_file").ToString() == "") ? "display:none;" : "") %>">
                                                <i class="fa fa-download" style="color: dodgerblue;"></i>&nbsp;<asp:LinkButton CausesValidation="false" ID="btnDownloadFile" runat="server" CommandArgument='<%#: (Eval("file_id") + "|" + Eval("group_id")) %>' CommandName="GetFile" CssClass="fileLink" Text='<%#: Eval("msg_file") %>'></asp:LinkButton>
                                            </div>
                                            <asp:HiddenField ID="hdnVisibility" runat="server" Value='<%#: Eval("visible") %>' />
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3 border">
                            <div class="row">
                                <div style="height: 70vh;">
                                    <div id="divNestsUserIsIn" class="list-group" style="overflow-y: auto; max-height: 70vh;" runat="server">
                                        <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="rptUsers" EventName="ItemDataBound" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:Repeater ID="rptSelectedUserNests" runat="server" OnItemCommand="rptSelectedUserNests_ItemCommand" OnItemDataBound="rptSelectedUserNests_ItemDataBound">
                                                    <ItemTemplate>
                                                        <div class='list-group-item list-group-item-action flex-column align-items-start color-red'>
                                                            <div class="d-flex w-100 justify-content-between overflow-hidden">
                                                                <h6 style="margin-top: auto; margin-bottom: auto; margin-right: 105px;">
                                                                    <%#: Eval("group_name") %>
                                                                </h6>
                                                                <small><%#: Eval("group_id") %></small>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:View>

            <asp:View ID="adminSearchAndViewGroups" runat="server">
                <div class="container">
                    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
                        <i class="fa fa-search" style="color: black;"></i>
                        <asp:TextBox ID="SearchBox" CssClass="mx-auto" Style="border-radius: 10px; width: 40%;" runat="server"></asp:TextBox>
                    </asp:Panel>
                    <asp:Button Style="display: none;" ID="btnSearch" runat="server" CssClass="fa fa-search" Text="" OnClick="btnSearch_Click" UseSubmitBehavior="false" />
                    <asp:Label ID="lblSearchError" Text="" runat="server" Style="position: absolute; right: 25%"></asp:Label>
                    <br>
                    <br />

                    <div class="row">
                        <div class="col-sm-3 col-md-4 col-lg-3">
                            Nest Search
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-6">
                            Active Searched Nest Messages
                        </div>
                        <div class="col-sm-3 col-md-4 col-lg-3">
                            Active Searched Nest Members
                        </div>
                    </div>
                    <div id="scrolly" class="row">
                        <div class="col-sm-3 col-md-4 col-lg-3 p-2 border align-self-start overflow-auto" style="max-height: 500px;">
                            <asp:Repeater ID="rptSearchResult" runat="server" OnItemCommand="rptSearchResult_ItemCommand">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnSearchUsername" runat="server" Style="text-decoration: none; color: black;" CommandArgument='<%#: Eval("group_id") %>' CommandName="viewMessages">
                                            <div style="display:inline;" class="list-group-item list-group-item-action flex-column align-items-start d-flex w-100">
                                                <div class="d-flex" style="display:inline;">
                                                    <h5 style="margin-top:auto;margin-bottom:auto;margin-left:15px;" class="mb-1"> <div><%#: "Group Id: " + Eval("group_id") %></div><div><%#: "Group Name: " + Eval("group_name") %></div><%#: "Group Visibility: " + Eval("visible") %></h5>
                                                </div>
                                            </div>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-6 p-2 border align-self-start overflow-auto" style="max-height: 500px;">
                            <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="GroupMessages" EventName="ItemDataBound" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Repeater ID="GroupMessages" OnItemCommand="rptGroupMessages_ItemCommand" OnItemDataBound="GroupMessages_ItemDataBound" runat="server">
                                        <ItemTemplate>
                                            <div data-msg-id="<%#: Eval("text_id") %>" class="messageContent">
                                                <a class="list-group-item list-group-item-action list-group-item-light rounded-0">
                                                    <div class="media">
                                                        <i class="fa fa-user"></i>
                                                        <div class="media-body ml-4">
                                                            <div class="d-flex align-items-center justify-content-between mb-1">
                                                                <h6 id="msgUserName" class="mb-0"><%#: Eval("user_name") %></h6>
                                                                <small class="small font-weight-bold"><%#: Eval("time_sent") %>
                                                                    &nbsp;
                                                                    <asp:Button class="btn btn-danger" CausesValidation="false" ID="DeleteMessage" runat="server" Text="Delete" CommandName="DeleteMessage" CommandArgument='<%#: (Eval("text_id") + "|" + Eval("group_id")) %>' UseSubmitBehavior="false"></asp:Button>
                                                                </small>
                                                            </div>
                                                            <p class="messageLabel font-italic text-muted mb-0 text-small"><%#: Eval("msg_text") %></p>
                                                        </div>
                                                    </div>
                                                </a>
                                            </div>
                                            <div class="fileDiv list-group-item list-group-item-action list-group-item-dark rounded-0" data-file-id='<%#: Eval("file_id") %>' style="<%#: ((Eval("msg_file").ToString() == "") ? "display:none;" : "") %>">
                                                <i class="fa fa-download" style="color: dodgerblue;"></i>&nbsp;<asp:LinkButton CausesValidation="false" ID="btnDownloadFile" runat="server" CommandArgument='<%#: (Eval("file_id") + "|" + Eval("group_id")) %>' CommandName="GetFile" CssClass="fileLink" Text='<%#: Eval("msg_file") %>'></asp:LinkButton>
                                            </div>
                                            <asp:HiddenField ID="hdnVisibility" runat="server" Value='<%#: Eval("visible") %>' />
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="col-sm-4 col-md-4 col-lg-3 p-2 border align-self-start" style="overflow-y: auto; overflow-x: hidden; height: 500px;">
                            <div class="row">
                                <div>
                                    <div id="UsersInNest" class="list-group  overflow-y-auto overflow-x-hidden" style="max-height: 500px;" runat="server">
                                        <h1 id="H1" runat="server" class="display-4"></h1>
                                        <small></small>
                                        <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="rptUsersInNest" EventName="ItemDataBound" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:Repeater ID="rptUsersInNest" runat="server" OnItemCommand="rptUsersInNest_ItemCommand" OnItemDataBound="rptUsersInNest_ItemDataBound">
                                                    <ItemTemplate>
                                                        <div class='list-group-item list-group-item-action flex-column align-items-start color-red <%#: Eval("css_class") %>'>
                                                            <div class="d-flex w-100 justify-content-between overflow-hidden">
                                                                <h5 style="margin-top: auto; margin-bottom: auto; margin-right: 105px;">
                                                                    <%#: Eval("user_name") %>
                                                                    <asp:Button Style="margin-right: 20px; right: 0; position: absolute;" ID="btnBanUser" class="btn btn-danger" Visible="false" runat="server" UseSubmitBehavior="false" CausesValidation="false" Text="Ban" CommandArgument='<%#: (Eval("user_name") + "|" + Eval("group_id")) %>' CommandName="BanMember" />
                                                                    <asp:Button Style="margin-right: 20px; right: 0; position: absolute;" ID="btnUnBanUser" class="btn btn-info" Visible="false" runat="server" UseSubmitBehavior="false" CausesValidation="false" Text="UnBan" CommandArgument='<%#: (Eval("user_name") + "|" + Eval("group_id")) %>' CommandName="UnBanMember" />
                                                                    <asp:HiddenField ID="hdnBannedStatus" runat="server" Value='<%#: Eval("banned") %>' />
                                                                </h5>
                                                            </div>
                                                            <small>
                                                                <%#: Eval("role_string") %>
                                                                <div><%#: "User Id: " + Eval("user_id") %></div>
                                                            </small>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:View>

            <asp:View ID="adminMessageStatistics" runat="server">
                <div class="container">
                    <div class="row g-3">
                        <div class="card text-dark border-dark">
                            <div class="card-header">
                                <h5>Messages sent during entered time period. Please enter a range.</h5>
                            </div>
                            <div class="card-body">
                                <asp:TextBox ID="startDate" TextMode="date" runat="server"></asp:TextBox>
                                <asp:TextBox ID="startTime" TextMode="time" runat="server"></asp:TextBox>
                                <a>&mdash; to &mdash;</a>
                                <asp:TextBox ID="endDate" TextMode="date" runat="server"></asp:TextBox>
                                <asp:TextBox ID="endTime" TextMode="time" runat="server"></asp:TextBox>
                                <br />
                                <br />
                                <asp:Button ID="btnMessageCount" runat="server" Text="Find" Class="btn btn-outline-primary" OnClick="btnMessageCount_Click" UseSubmitBehavior="false" />
                                <asp:Button ID="btnResetMessageCount" runat="server" Text="Reset" Class="btn btn-outline-primary" OnClick="btnResetMessagesCount_Click" UseSubmitBehavior="false" />
                            </div>
                            <div class="card-footer text-muted">
                                <asp:GridView ID="MessageCountTimeSpanGridView" runat="server" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px"
                                    CellPadding="4" DataKeyNames="cnt">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Messages Sent">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox" runat="server" Text='<%# Bind("cnt") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label" runat="server" Text='<%# Bind("cnt") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>

                        <div class=" card text-dark  border-dark">
                            <div class="card-header">
                                <h5>Most active group by messages sent during entered time period. Please enter a range.</h5>
                            </div>
                            <div class="card-body">
                                <asp:TextBox ID="startDate2" TextMode="date" runat="server"></asp:TextBox>
                                <asp:TextBox ID="startTime2" TextMode="time" runat="server"></asp:TextBox>
                                <a>&mdash; to &mdash;</a>
                                <asp:TextBox ID="endDate2" TextMode="date" runat="server"></asp:TextBox>
                                <asp:TextBox ID="endTime2" TextMode="time" runat="server"></asp:TextBox>
                                <br />
                                <br />
                                <asp:Button ID="btnGroupMostMessagesCount" runat="server" Text="Find" Class="btn btn-outline-primary" OnClick="btnGroupMostMessagesCount_Click" UseSubmitBehavior="false" />
                                <asp:Button ID="btnResetGroupMostMessagesCount" runat="server" Text="Reset" Class="btn btn-outline-primary" OnClick="btnResetGroupMostMessagesCount_Click" UseSubmitBehavior="false" />
                            </div>
                            <div class="card-footer text-muted">
                                <asp:GridView ID="MostMessagesGroupGridView" runat="server" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px"
                                    CellPadding="4" DataKeyNames="group_id">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Group Name">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("group_name") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("group_name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Group ID">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("group_id") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("group_id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Message Count">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("cnt") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("cnt") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>

                        <div class="card text-dark border-dark">
                            <div class="card-header">
                                <h5>Most active user by messages sent during entered time period. Please enter a range.</h5>
                            </div>
                            <div class="card-body">
                                <asp:TextBox ID="startDate3" TextMode="date" runat="server"></asp:TextBox>
                                <asp:TextBox ID="startTime3" TextMode="time" runat="server"></asp:TextBox>
                                <a>&mdash; to &mdash;</a>
                                <asp:TextBox ID="endDate3" TextMode="date" runat="server"></asp:TextBox>
                                <asp:TextBox ID="endTime3" TextMode="time" runat="server"></asp:TextBox>
                                <br />
                                <br />
                                <asp:Button ID="btnUserMostMessagesCount" runat="server" Text="Find" Class="btn btn-outline-primary" OnClick="btnUserMostMessagesCount_Click" UseSubmitBehavior="false" />
                                <asp:Button ID="btnResetUserMostMessagesCount" runat="server" Text="Reset" Class="btn btn-outline-primary" OnClick="btnResetUserMostMessagesCount_Click" UseSubmitBehavior="false" />
                            </div>
                            <div class="card-footer text-muted">
                                <asp:GridView ID="MostMessagesUserGridView" runat="server" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px"
                                    CellPadding="4" DataKeyNames="user_id">
                                    <Columns>
                                        <asp:TemplateField HeaderText="User Name">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("user_name") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("user_name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User ID">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("user_id") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("user_id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Message Count">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("cnt") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("cnt") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>

                        <div class=" card text-dark border-dark">
                            <div class="card-header">
                                <h5>Search a user's message count sent during entered time period. Please enter a range.</h5>
                            </div>
                            <div class="card-body">
                                <asp:TextBox ID="startDate4" TextMode="date" runat="server"></asp:TextBox>
                                <asp:TextBox ID="startTime4" TextMode="time" runat="server"></asp:TextBox>
                                <a>&mdash; to &mdash;</a>
                                <asp:TextBox ID="endDate4" TextMode="date" runat="server"></asp:TextBox>
                                <asp:TextBox ID="endTime4" TextMode="time" runat="server"></asp:TextBox>
                                <asp:TextBox ID="userID" runat="server" Operator="DataTypeCheck" Type="Integer" Width="5%" MaxLength="4" ControlToValidate="ValueTextBox" ErrorMessage="Value must be a whole number"></asp:TextBox>
                                <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage="User ID only" ControlToValidate="userID" ValidationExpression="^\d+$" runat="server" />
                                <br />
                                <br />
                                <asp:Button ID="btnSearchUserMessagesCount" runat="server" Text="Find" Class="btn btn-outline-primary" OnClick="btnSearchUserMessagesCount_Click" UseSubmitBehavior="false" />
                                <asp:Button ID="btnResetSearchUserMessagesCount" runat="server" Text="Reset" Class="btn btn-outline-primary" OnClick="btnResetSearchUserMessagesCount_Click" UseSubmitBehavior="false" />
                            </div>
                            <div class="card-footer text-muted">
                                <asp:GridView ID="SearchUserMessageCountGridView" runat="server" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px"
                                    CellPadding="4" DataKeyNames="user_id">
                                    <Columns>
                                        <asp:TemplateField HeaderText="User Name">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("user_name") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("user_name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User ID">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("user_id") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("user_id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Message Count">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("cnt") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("cnt") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>

                        <div class="card text-dark border-dark">
                            <div class="card-header">
                                <h5>Search a group's message count sent during entered time period. Please enter a range.</h5>
                            </div>
                            <div class="card-body">
                                <asp:TextBox ID="startDate5" TextMode="date" runat="server"></asp:TextBox>
                                <asp:TextBox ID="startTime5" TextMode="time" runat="server"></asp:TextBox>
                                <a>&mdash; to &mdash;</a>
                                <asp:TextBox ID="endDate5" TextMode="date" runat="server"></asp:TextBox>
                                <asp:TextBox ID="endTime5" TextMode="time" runat="server"></asp:TextBox>
                                <asp:TextBox ID="groupID" runat="server" Operator="DataTypeCheck" Type="Integer" Width="5%" Display="Dynamic" ControlToValidate="ValueTextBox" ErrorMessage="Value must be a whole number"></asp:TextBox>
                                <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage="User ID only" ControlToValidate="userID" ValidationExpression="^\d+$" runat="server" />
                                <br />
                                <br />
                                <asp:Button ID="btnSearchGroupMessagesCount" runat="server" Text="Find" Class="btn btn-outline-primary" OnClick="btnSearchGroupMessagesCount_Click" UseSubmitBehavior="false" />
                                <asp:Button ID="btnResetSearchGroupMessagesCount" runat="server" Text="Reset" Class="btn btn-outline-primary" OnClick="btnResetSearchGroupMessagesCount_Click" UseSubmitBehavior="false" />
                            </div>
                            <div class="card-footer text-muted">
                                <asp:GridView ID="SearchGroupMessageCountGridView" runat="server" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px"
                                    CellPadding="4" DataKeyNames="group_id">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Group Name">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("group_name") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("group_name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User ID">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("group_id") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("group_id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Message Count">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("cnt") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("cnt") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>
    </section>
</asp:Content>
