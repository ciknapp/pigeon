<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PigeonMaster.Master" AutoEventWireup="true" CodeBehind="ConfirmEmail.aspx.cs" Inherits="CS414_Team2.ConfirmEmail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <section class="login-section">
        <header class="masthead">
            <div class="container align-items-top justify-content-center" style="height: 100%;">
                <div class="d-flex justify-content-center align-self-start" style="height: 100%;">
                    <div class="text-center col-md-6 col-lg-4" style="margin-top: auto; margin-bottom: auto;">
                        <img src="Resources/Images/FullLogo.png" alt="Pigeon" width="275" class="mx-auto my-0" />
                        <h2 class="text-black-50 mx-auto">Validate Account</h2>
                        <asp:MultiView ID="loginMultiView" ActiveViewIndex="0" runat="server">
                            <asp:View ID="emailSent" runat="server">
                                <div class="left login-wrap p-0">
                                    <div class="input-group">
                                        <p style="text-align: center;">To start messaging with Pigeon, please validate your account by confirming your email address and providing us with the passcode we sent to you!</p>
                                        <div class="input-group">
                                            <asp:TextBox ID="confirmEmail" class="form-control" placeholder="Email" runat="server" />
                                        </div>
                                        <asp:RequiredFieldValidator ValidationGroup="confirmEmailValidators" Display="Dynamic" ID="confirmEmailEmailValidator" ErrorMessage="Please enter your email." ControlToValidate="confirmEmail" runat="server" />
                                        <div class="left">
                                            <div class="input-group">
                                                <asp:TextBox ID="confirmPasscode" CssClass="form-control" placeholder="Verification Code" runat="server" />
                                            </div>
                                            <asp:RequiredFieldValidator ValidationGroup="confirmEmailValidators" Display="Dynamic" ID="confirmEmailPasscodeValidator" ErrorMessage="Please enter an eight digit passcode." ControlToValidate="confirmPasscode" runat="server" />
                                            <asp:RegularExpressionValidator ValidationGroup="confirmEmailValidators" Display="Dynamic" ID="confirmEmailPasscodeValidator2" runat="server" ControlToValidate="confirmPasscode" ErrorMessage="Use 8 characters for a passcode." ValidationExpression="[\w]{8,8}" />
                                        </div>
                                        <div class="w-100" style="color: red">
                                            <asp:Label ID="lblConfirmEmailError" Text="" runat="server" />
                                        </div>
                                        <asp:Button ID="btnConfirmEmail" UseSubmitBehavior="true" CausesValidation="true" ValidationGroup="confirmEmailValidators" Text="Confirm Email" class="form-control btn btn-primary submit px-5" runat="server" OnClick="btn_ConfirmEmail_Click" />
                                    </div>
                                </div>
                            </asp:View>
                            <asp:View ID="afterConfirmEmail" runat="server">
                                <p>Your email was confirmed! Click the link to return to login.</p>
                                <asp:Button type="button" class="form-control btn  btn btn-primary submit px-5" Text="Return to Login" OnClick="btnToSignIn_Click" runat="server" />
                            </asp:View>
                        </asp:MultiView>
                    </div>
                </div>
            </div>
        </header>
    </section>
</asp:Content>
