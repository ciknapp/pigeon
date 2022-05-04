<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PigeonMaster.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="CS414_Team2.ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <section class="login-section">
        <header class="masthead">
            <div class="container align-items-top justify-content-center" style="height: 100%;">
                <div class="d-flex justify-content-center align-self-start" style="height: 100%;">
                    <div class="text-center col-md-6 col-lg-4" style="margin-top: auto; margin-bottom: auto;">
                        <img src="Resources/Images/FullLogo.png" alt="Pigeon" width="275" class="mx-auto my-0" />
                        <br />
                        <br />
                        <h2 class="text-black-50 mx-auto">Forgot your Password?</h2>
                        <asp:MultiView ID="loginMultiView" ActiveViewIndex="0" runat="server">
                            <asp:View ID="codeNotSentView" runat="server">
                                <div class="login-wrap p-0">
                                    <div class="input-group">
                                        <p>Enter the email and username used at registration to receive a one use code to reset your account password.</p>
                                        <div class="input-group">
                                            <asp:TextBox ID="forgottenPasswordEmail" type="email" class="form-control" placeholder="Email" name="email" runat="server" />
                                        </div>
                                        <asp:RequiredFieldValidator ValidationGroup="forgotPasswordValidators" Display="Dynamic" ID="forgotPasswordEmailValidator" ErrorMessage="Please enter your email address." ControlToValidate="forgottenPasswordEmail" runat="server" />
                                        <div class="input-group">
                                            <asp:TextBox ID="forgottenPasswordUsername" type="username" class="form-control" placeholder="Username" name="username" runat="server" />
                                        </div>
                                        <asp:RequiredFieldValidator ValidationGroup="forgotPasswordValidators" Display="Dynamic" ID="forgotPasswordUsernameValidator" ErrorMessage="Please enter your username." ControlToValidate="forgottenPasswordUsername" runat="server" />
                                        <div class="w-100" style="color: red">
                                            <asp:Label ID="lblForgottenPasswordError" Text="" runat="server" />
                                        </div>
                                        <asp:Button ID="btnForgotPassword" UseSubmitBehavior="true" CausesValidation="true" ValidationGroup="forgotPasswordValidators" Text="Send Code" class="form-control btn btn-primary submit px-5" runat="server" OnClick="btn_ForgotPassword_Click" />
                                        <p class="w-100 text-center pt-3">&mdash; Remember your password? &mdash;<asp:Button type="button" class="form-control btn  btn btn-primary submit px-5" Text="Return to Login" OnClick="btnToSignIn_Click" runat="server" /></p>
                                    </div>
                                </div>
                            </asp:View>
                            <asp:View ID="codeSentView" runat="server">
                                <div class="left login-wrap p-0">
                                    <div class="input-group">
                                        <p>Enter your New password, then confirm your new password, and enter the passcode sent to your email to reset your password to the new password.</p>
                                        <div class="input-group">
                                            <asp:TextBox ID="resetPasswordUsername" type="username" class="form-control" placeholder="Username" name="username" runat="server" />
                                        </div>
                                        <asp:RequiredFieldValidator ValidationGroup="resetPasswordValidators" Display="Dynamic" ID="resetPasswordUsernameValidator" ErrorMessage="Please enter your username." ControlToValidate="resetPasswordUsername" runat="server" />
                                        <div class="left">
                                            <div class="input-group">
                                                <asp:TextBox ID="resetPasswordNewPassword" TextMode="Password" CssClass="form-control" placeholder="New Password" runat="server" />
                                            </div>
                                            <asp:RequiredFieldValidator ValidationGroup="resetPasswordValidators" Display="Dynamic" ID="resetPasswordNewPasswordValidator" ErrorMessage="Please enter an eight to twenty-five character password." ControlToValidate="resetPasswordNewPassword" runat="server" />
                                            <asp:RegularExpressionValidator ValidationGroup="resetPasswordValidators" Display="Dynamic" ID="resetPasswordNewPasswordValidator2" runat="server" ControlToValidate="resetPasswordNewPassword" ErrorMessage="A password is limited between 8-128 characters, and may not contain '|'" ValidationExpression="^([^|\n]){8,128}$" />
                                        </div>
                                        <div class="left">
                                            <div class="input-group">
                                                <asp:TextBox ID="resetPasswordConfirmNewPassword" TextMode="Password" CssClass="form-control" placeholder="Confirm Password" runat="server" />
                                            </div>
                                            <asp:CompareValidator Display="Dynamic" ErrorMessage="Passwords do not match." ControlToValidate="resetPasswordConfirmNewPassword" ControlToCompare="resetPasswordNewPassword" Operator="Equal" Type="String" runat="server" ValidationGroup="resetPasswordValidators" />
                                            <asp:RequiredFieldValidator ValidationGroup="resetPasswordValidators" Display="Dynamic" ID="resetPasswordConfirmNewPasswordValidator" ErrorMessage="Please enter an eight to twenty-five character password." ControlToValidate="resetPasswordConfirmNewPassword" runat="server" />
                                            <asp:RegularExpressionValidator ValidationGroup="resetPasswordValidators" Display="Dynamic" ID="resetPasswordConfirmNewPasswordValidator2" runat="server" ControlToValidate="resetPasswordConfirmNewPassword" ErrorMessage="A password is limited between 8-128 characters, and may not contain '|'" ValidationExpression="^([^|\n]){8,128}$" />
                                        </div>
                                        <div class="left">
                                            <div class="input-group">
                                                <asp:TextBox ID="resetPasswordPasscode" type="username" CssClass="form-control" placeholder="Passcode" runat="server" />
                                            </div>
                                            <asp:RequiredFieldValidator ValidationGroup="resetPasswordValidators" Display="Dynamic" ID="resetPasswordPasscodeValidator" ErrorMessage="Please enter an eight digit passcode." ControlToValidate="resetPasswordPasscode" runat="server" />
                                            <asp:RegularExpressionValidator ValidationGroup="resetPasswordValidators" Display="Dynamic" ID="resetPasswordPasscodeValidator2" runat="server" ControlToValidate="resetPasswordPasscode" ErrorMessage="Use 8 characters for a passcode." ValidationExpression="[\w]{8,8}" />
                                        </div>
                                        <div class="w-100" style="color: red">
                                            <asp:Label ID="lblResetPasswordError" Text="" runat="server" />
                                        </div>
                                        <asp:Button ID="btnResetPassword" UseSubmitBehavior="true" CausesValidation="true" ValidationGroup="resetPasswordValidators" Text="Submit Change" class="form-control btn btn-primary submit px-5" runat="server" OnClick="btn_ResetPassword_Click" />
                                        <p class="w-100 text-center">&mdash;Already remeber your password? &mdash;</p>
                                        <asp:Button type="button" class="form-control btn  btn btn-primary submit px-5" Text="Return to Login" OnClick="btnToSignIn_Click" runat="server" />
                                    </div>
                                </div>
                            </asp:View>
                            <asp:View ID="afterPasswordReset" runat="server">
                                <p>Your Password was reset! Click the link to return to login.</p>
                                <asp:Button type="button" class="form-control btn  btn btn-primary submit px-5" Text="Return to Login" OnClick="btnToSignIn_Click" runat="server" />
                            </asp:View>
                        </asp:MultiView>
                    </div>
                </div>
            </div>
        </header>
    </section>
</asp:Content>
