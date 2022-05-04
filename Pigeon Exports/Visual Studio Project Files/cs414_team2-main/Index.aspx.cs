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
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (PigeonMaster.CurrentUser.IsLoggedIn)
            {
                Response.Redirect("nest.aspx");
            }
            else
            {
                Page.Title += " - Sign in or Sign up";
                loginMultiView.SetActiveView(notLoggedInView);
            }
        }

        protected void signinSubmit_Click(object sender, EventArgs e)
        {
            if (PigeonMaster.CurrentUser.IsLoggedIn == false)
            {
                try
                {
                    PigeonMaster.CurrentUser.Login(signinUsername.Text.Trim(), signinPassword.Text, /*rememberMe.Checked*/ false);
                    Response.Redirect("nest.aspx");
                }
                catch (OracleException oEx)
                {
                    switch (oEx.Number)
                    {
                        case 20002: // Username does not exist
                        case 20200: // Combination wrong
                            lblSignInError.Text = "Incorrect username and/or password. Please try again.";
                            break;
                        case 20201:
                            lblSignInError.Text = "This user is already logged in.";
                            break;
                        case 20202:
                            lblSignInError.Text = "Please confirm your email.";
                            break;
                        case 20203:
                            lblSignInError.Text = "You have been banned. Please contact Pigeon Support for further details. ";
                            break;
                        default:
                            throw oEx;
                    }
                }
            }
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            if (PigeonMaster.CurrentUser.IsLoggedIn)
            {
                PigeonMaster.CurrentUser.Logout();
                Response.Redirect("Index.aspx");
            }
            else
            {
                loginMultiView.SetActiveView(notLoggedInView);
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("signUp.aspx");
        }
        protected void btn_forgot_pass_Click(object sender, EventArgs e)
        {
            Response.Redirect("ForgotPassword.aspx");
        }
    }
}
