<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PigeonMaster.Master" AutoEventWireup="true" CodeBehind="signUp.aspx.cs" Inherits="CS414_Team2.signUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Register</title>
    <script>
        function validateNoProfanity(source, args) {
            let message = args.Value.replace(/ /, "%20");

            const settings = {
                "async": false,
                "crossDomain": true,
                "url": "https://community-purgomalum.p.rapidapi.com/containsprofanity?text=" + message,
                "method": "GET",
                "headers": {
                    "X-RapidAPI-Host": "community-purgomalum.p.rapidapi.com",
                    "X-RapidAPI-Key": "f92a68b7fcmsh3bd4be4696625b8p1b10ebjsn938ff5c85120"
                }
            };

            $.ajax(settings).done(function (response) {
                args.IsValid = response == "false";
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <section class="login-section">
        <header class="masthead">
            <div class="container align-items-top justify-content-center" style="height: 100%;">
                <div class="d-flex justify-content-center align-self-start" style="height: 100%;">
                    <div class="text-center col-md-6 col-lg-4" style="margin-top: auto; margin-bottom: auto;">
                        <img src="Resources/Images/FullLogo.png" alt="Pigeon" width="275" class="mx-auto my-0" />
                        <asp:MultiView ID="loginMultiView" ActiveViewIndex="0" runat="server">
                            <asp:View ID="notSignedUpView" runat="server">
                                <div class="login-wrap p-0">
                                    <div class="input-group">
                                        <div class="input-group ">
                                            <asp:TextBox ID="registerUsername" class="form-control" placeholder="Username" runat="server" ValidationGroup="registrationValidators"/>
                                        </div>
                                        <asp:RequiredFieldValidator ValidationGroup="registrationValidators" Display="Dynamic" ID="rfvUsername" ErrorMessage="Please choose a username." ControlToValidate="registerUsername" runat="server" />
                                        <asp:CustomValidator ErrorMessage="Username can not contain profanity." Display="Dynamic" ControlToValidate="registerUsername" ClientValidationFunction="validateNoProfanity" runat="server" ValidationGroup="registrationValidators"/>
                                        <asp:RegularExpressionValidator ValidationGroup="registrationValidators" Display="Dynamic" ErrorMessage="Usernames can not contain punctuation or spaces." ControlToValidate="registerUsername" ValidationExpression="^([A-Za-z0-9_]){4,25}$" runat="server" />
                                        <div class="input-group">
                                            <asp:TextBox ID="registerEmailAddress" type="email" class="form-control" placeholder="Email" ValidationGroup="registrationValidators" name="email" runat="server" />
                                        </div>
                                        <asp:RegularExpressionValidator Display="Dynamic" ErrorMessage="Emails must end with a pcci.edu domain. Contact a system administrator if you are a guest." ValidationExpression="^[A-Za-z0-9._%+-]+@(students\.)?pcci.edu$" ControlToValidate="registerEmailAddress" runat="server" ValidationGroup="registrationValidators"/>
                                        <asp:RequiredFieldValidator ValidationGroup="registrationValidators" Display="Dynamic" ID="rfvEmail" ErrorMessage="Please enter your email address." ControlToValidate="registerEmailAddress" runat="server" />
                                        <div class="input-group">
                                            <asp:TextBox ID="registerPassword" type="password" ValidationGroup="registrationValidators" class="form-control" placeholder="Password" name="pswd" runat="server" />
                                        </div>
                                        <asp:RequiredFieldValidator ValidationGroup="registrationValidators" Display="Dynamic" ID="rfvPassword" ErrorMessage="Please create a password." ControlToValidate="registerPassword" runat="server" />
                                        <asp:RegularExpressionValidator ValidationGroup="registrationValidators" Display="Dynamic" ID="revPassword" runat="server" ControlToValidate="registerPassword" ErrorMessage="A password is limited between 8-128 characters, and may not contain '|'" ValidationExpression="^([^|\n]){8,128}$" />
                                        <div class="input-group">
                                            <asp:TextBox ValidationGroup="registrationValidators" ID="confirmRegisterPassword" type="password" class="form-control" placeholder="Confirm Password" name="cnfrmpswd" runat="server" />
                                        </div>
                                        <asp:CompareValidator Display="Dynamic" ErrorMessage="Passwords do not match." ControlToValidate="confirmRegisterPassword" ControlToCompare="registerPassword" Operator="Equal" Type="String" runat="server" ValidationGroup="registrationValidators" />
                                        <asp:RequiredFieldValidator ValidationGroup="registrationValidators" Display="Dynamic" ID="resetPasswordConfirmNewPasswordValidator" ErrorMessage="Please enter an eight to twenty-five character password." ControlToValidate="confirmRegisterPassword" runat="server" />
                                        <asp:RegularExpressionValidator ValidationGroup="registrationValidators" Display="Dynamic" ID="resetPasswordConfirmNewPasswordValidator2" runat="server" ControlToValidate="confirmRegisterPassword" ErrorMessage="A password is limited between 8-128 characters, and may not contain '|'" ValidationExpression="^([^|\n]){8,128}$" />
                                        <div class="w-100" style="color: red">
                                            <asp:Label ID="lblRegisterError" Text="" runat="server" />
                                        </div>
                                        <asp:Button ID="btnRegister" UseSubmitBehavior="true" CausesValidation="true" ValidationGroup="registrationValidators" Text="Sign Up" class="form-control btn  btn btn-primary submit px-5" runat="server" OnClick="btnRegister_Click" />
                                        <p class="w-100 pt-3 text-center">&mdash; Already have an account? &mdash;<asp:Button type="button" ID="SignInToIndex" OnClick="SignInToIndex_Click" class="form-control btn  btn btn-primary submit px-5" Text="Sign In" UseSubmitBehavior="false" CausesValidation="false" runat="server" /></p>
                                    </div>

                                </div>
                            </asp:View>
                            <asp:View ID="signedUpView" runat="server">
                                <p>Thank you for signing up!</p>
                                <a href="Index.aspx">Click here to sign in to your account.</a>
                            </asp:View>
                        </asp:MultiView>
                    </div>
                </div>
            </div>
        </header>
    </section>
</asp:Content>
