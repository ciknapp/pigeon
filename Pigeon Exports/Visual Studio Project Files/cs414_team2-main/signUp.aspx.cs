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
    public partial class signUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // If the user succesfully signs up, redirect them to the login page
            if (PigeonMaster.CurrentUser.IsLoggedIn)
            {
                Response.Redirect("nest.aspx");
            }
        }
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                if (registerEmailAddress.Text.Trim().EndsWith("pcci.edu"))
                {
                    DataProvider.AddUser(registerUsername.Text, registerPassword.Text, registerEmailAddress.Text);
                    SendMail.SendConfirmationMail(registerEmailAddress.Text, registerUsername.Text);
                    Session["UsernameForSignup"] = Encrypt.EncryptString(registerUsername.Text);
                    CookieJar.Put("EmailForSignup", registerEmailAddress.Text);
                    Response.Redirect("ConfirmEmail.aspx");
                }
            }
            catch (OracleException oEx)
            {
                switch (oEx.Number)
                {
                    case 20100:
                        lblRegisterError.Text = "A user by that name exists already.";
                        break;
                    case 20101:
                        lblRegisterError.Text = "That email is in use by another user.";
                        break;
                    case 20102:
                        lblRegisterError.Text = "Please enter a valid email address.";
                        break;
                    default:
                        throw oEx;
                }
            }
        }

        protected void SignInToIndex_Click(object sender, EventArgs e)
        {
            Response.Redirect("index.aspx");
        }
    }
}