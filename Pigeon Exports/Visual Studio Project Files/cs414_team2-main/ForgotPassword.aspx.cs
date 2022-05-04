using CS414_Team2.Data;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;

namespace CS414_Team2
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btn_ForgotPassword_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string EmailSent = SendMail.SendResetPasswordMail(forgottenPasswordEmail.Text, forgottenPasswordUsername.Text);
                    if (EmailSent == null)
                    {
                        loginMultiView.SetActiveView(codeSentView);
                        resetPasswordUsername.Text = forgottenPasswordUsername.Text;
                        resetPasswordUsername.Visible = false;
                    }
                    else
                    {
                        lblForgottenPasswordError.Text = EmailSent;
                    }
                }
                catch (OracleException oEx)
                {
                    switch (oEx.Number)
                    {
                        case 20002:
                            lblForgottenPasswordError.Text = "There is no account associated with the given username.";
                            break;
                        case 20003:
                            lblForgottenPasswordError.Text = "There is no account associated with the given email address.";
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        protected void btn_ResetPassword_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string old_password = "";
                    DataProvider.ChangePassword(resetPasswordUsername.Text, resetPasswordNewPassword.Text, old_password, resetPasswordPasscode.Text);
                    loginMultiView.SetActiveView(afterPasswordReset);
                }
                catch (OracleException oEx)
                {
                    switch (oEx.Number)
                    {
                        case 20002:
                            lblResetPasswordError.Text = "Username is not correct.";
                            break;
                        case 20003:
                            lblResetPasswordError.Text = "Email is not correct.";
                            break;
                        case 20501:
                            lblResetPasswordError.Text = "Passcode is not correct.";
                            break;
                        default:
                            throw oEx;
                    }
                }
            }
        }
        protected void btnToSignIn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Index.aspx");
        }
    }
}