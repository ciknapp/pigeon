<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PigeonMaster.Master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="UserSettings.aspx.cs" Inherits="CS414_Team2.UserSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>User Settings</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <section style="height: 55px; color: white;">
        <nav style="background-color: #4A87C6;">
            <div class="container" style="width: 100%; margin: 0;">
                <a href="Nest.aspx" style="text-decoration: none; color: white;">
                    <img src="Resources/Images/FullLogo2.png" height="54" width="74" style="width: auto;" />
                    <i class="fa-solid fa-arrow-left"></i>
                    <b>&nbsp;Back to Nests</b>
                </a>
                <div class="create" style="position: absolute; right: 0; margin-right: 20px;">
                    <asp:Label ID="lblNavBarUsername" Text="" runat="server" />
                    <div>
                        <button type="button" class="btn btn-secondary dropdown-toggle dropdown-toggle-split" style="background-color: #4A87C6; border: none;" id="MenuReference" data-bs-toggle="dropdown" aria-expanded="false" data-bs-reference="parent">
                            <span class="visually-hidden">Toggle Dropdown</span>
                            <img class="profile-photo" style="border: 2px solid white;" id="imgNavProfilePicture" runat="server" src="Resources/Images/FullLogo.png" />
                        </button>
                        <div class="dropdown-menu" aria-labelledby="MenuReference">
                            <a class="dropdown-item" href="Team.aspx"><i class="fa fa-users" aria-hidden="true"></i>Our Team</a>
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

    <div class="container-flex">
        <div class="main-body" style="margin-left: 2%; margin-right: 2%;">
            <div class="row">
                <div class="col-lg-4">
                    <div class="card">
                        <div class="card-body">
                            <div class="d-flex flex-column align-items-center text-center">
                                <asp:Image ID="imgProfilePicture" runat="server" class="rounded-circle" Width="150" Style="border: 7px solid #4A87C6;" />
                                <div class="mt-3">
                                    <h4 id="hUserName" runat="server"></h4>
                                </div>
                            </div>
                            <br>
                            <ul class="list-group list-group-flush">
                                <li class="list-group-item d-flex justify-content-between align-items-center flex-wrap">
                                    <h6 class="mb-0">Pigeon Points</h6>
                                    <span class="text-secondary">
                                        <asp:Label ID="lblPigeonPoints" runat="server"></asp:Label><i class="fa-solid fa-crow"></i></span>
                                </li>
                            </ul>
                            <hr>
                            <br />
                            <br />
                            <h3 class="d-flex align-items-center mb-3">Update Password</h3>
                            <div class="row mb-3">
                                <div class="col-sm-3">
                                    <h6 class="mb-0">Current password</h6>
                                </div>
                                <div class="col-sm-9 text-secondary">
                                    <asp:TextBox ID="txtCurrentPassword" ValidationGroup="valPasswords" TextMode="Password" CssClass="form-control" runat="server" />
                                    <asp:RequiredFieldValidator ValidationGroup="valPasswords" Display="Dynamic" ErrorMessage="Please enter your password." ControlToValidate="txtCurrentPassword" runat="server" />
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-sm-3">
                                    <h6 class="mb-0">New password</h6>
                                </div>
                                <div class="col-sm-9 text-secondary">
                                    <asp:TextBox ID="txtNewPassword" ValidationGroup="valPasswords" TextMode="Password" CssClass="form-control" runat="server" />
                                    <asp:RequiredFieldValidator ValidationGroup="valPasswords" Display="Dynamic" ID="rfvPassword" ErrorMessage="Please create a password." ControlToValidate="txtNewPassword" runat="server" />
                                    <asp:RegularExpressionValidator ValidationGroup="valPasswords" Display="Dynamic" ID="revPassword" runat="server" ControlToValidate="txtNewPassword" ErrorMessage="A password is limited between 8-128 characters, and may not contain '|'" ValidationExpression="^([^|\n]){8,128}$" />
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-sm-3">
                                    <h6 class="mb-0">Confirm password</h6>
                                </div>
                                <div class="col-sm-9 text-secondary">
                                    <asp:TextBox ID="txtConfirmPassword" ValidationGroup="valPasswords" TextMode="Password" CssClass="form-control" runat="server" />
                                    <asp:CompareValidator Display="Dynamic" ErrorMessage="Passwords do not match." ControlToValidate="txtConfirmPassword" ControlToCompare="txtNewPassword" Operator="Equal" Type="String" runat="server" ValidationGroup="valPasswords" />
                                    <asp:RequiredFieldValidator ValidationGroup="valPasswords" Display="Dynamic" ErrorMessage="Please enter an eight to twenty-five character password." ControlToValidate="txtConfirmPassword" runat="server" />
                                    <asp:RegularExpressionValidator ValidationGroup="valPasswords" Display="Dynamic" runat="server" ControlToValidate="txtConfirmPassword" ErrorMessage="A password is limited between 8-128 characters, and may not contain '|'" ValidationExpression="^([^|\n]){8,128}$" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3"></div>
                                <div class="col-sm-9 text-secondary">
                                    <asp:Button ID="btnUpdatePassword" CssClass="btn btn-outline-primary px-4" Text="Update" runat="server" CausesValidation="true" ValidationGroup="valPasswords" OnClick="btnUpdatePassword_Click" />
                                    <asp:Label ID="lblUpdateStatus" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-lg-8">
                    <div class="card">
                        <div class="card-body">
                            <h3 class="d-flex align-items-center mb-3">Update Profile Picture</h3>
                            <p>Select to update</p>
                            <div class="bird">
                                <ul>
                                    <asp:Repeater ID="rptUserIconChoices" runat="server" OnItemCommand="rptUserIconChoices_ItemCommand">
                                        <ItemTemplate>
                                            <li>
                                                <asp:LinkButton ID="btnNewUserIcon" runat="server" CssClass="img-fluid" CommandArgument='<%#: Eval("icon_id") %>' CommandName="changeUserIcon">
                                                    <img src='<%#: Eval("image_url") %>' style="border:7px solid #4A87C6;border-radius:5px;"/>
                                                </asp:LinkButton>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
