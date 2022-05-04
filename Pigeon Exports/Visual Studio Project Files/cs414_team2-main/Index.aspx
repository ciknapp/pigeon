<%@ Page Language="C#" MasterPageFile="~/MasterPages/PigeonMaster.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="CS414_Team2.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="body">
        <section class="login-section">
            <header class="masthead">
                <div class="container align-items-top justify-content-center" style="height: 100%;">
                    <div class="d-flex justify-content-center align-self-start" style="height: 100%;">
                        <div class="text-center col-md-6 col-lg-4" style="margin-top: auto; margin-bottom: auto;">
                            <img src="Resources/Images/FullLogo.png" alt="Pigeon" width="275" class="mx-auto my-0" />
                            <asp:MultiView ID="loginMultiView" ActiveViewIndex="0" runat="server">
                                <asp:View ID="notLoggedInView" runat="server">
                                    <div class="login-wrap p-0" style="margin-top: 50px;">
                                        <div class="input-group">
                                            <asp:TextBox ID="signinUsername" CssClass="form-control" placeholder="Username" runat="server" />
                                        </div>
                                        <asp:RequiredFieldValidator ValidationGroup="signInValidators" Display="Dynamic" ErrorMessage="Please enter your username." ControlToValidate="signinUsername" runat="server" />
                                        <div class="input-group">
                                            <asp:TextBox ID="signinPassword" TextMode="Password" CssClass="form-control" placeholder="Password" runat="server" />
                                        </div>
                                        <asp:RequiredFieldValidator ValidationGroup="signInValidators" Display="Dynamic" ErrorMessage="Please enter your password." ControlToValidate="signinPassword" runat="server" />
                                        <div class="form-group">
                                            <asp:Button ID="signinSubmit" ValidationGroup="signInValidators" Text="Sign In" type="submit" class="form-control btn btn-primary submit px-5" runat="server" OnClick="signinSubmit_Click"></asp:Button>
                                            <div class="w-100" style="color: red">
                                                <asp:Label ID="lblSignInError" Text="" runat="server" />
                                            </div>
                                        </div>
                                        <div class="form-group d-md-flex">
                                            <div class="w-100 text-md-right">
                                                <%--change to w-50 when implementing remember me (after expo)--%>
                                                <asp:LinkButton ForeColor="black" runat="server" OnClick="btn_forgot_pass_Click" Text="Forgot Password" />
                                            </div>
                                        </div>
                                        <p class="w-100 pt-3 text-center">&mdash; Or create an account &mdash;<asp:Button ID="btnSignUp" CssClass="form-control btn  btn btn-primary submit px-5" runat="server" OnClick="btnRegister_Click" Text="Sign Up"></asp:Button></p>
                                    </div>
                                </asp:View>
                                <asp:View ID="loggedInView" runat="server">
                                    <asp:Label ID="lblLoggedInGreeting" Text="" runat="server" />
                                    <asp:Button ID="btnLogOut" Text="Log out" UseSubmitBehavior="false" CausesValidation="false" OnClick="btnLogOut_Click" runat="server" />
                                </asp:View>
                            </asp:MultiView>
                        </div>
                    </div>
                </div>
    </div>
</asp:Content>

