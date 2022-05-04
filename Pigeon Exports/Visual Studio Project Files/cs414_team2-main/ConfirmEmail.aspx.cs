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
    public partial class ConfirmEmail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string email = CookieJar.Take("EmailForSignup");

                if (email != string.Empty)
                {
                    confirmEmail.Text = email;
                    confirmEmail.Visible = confirmEmail.Enabled = false;
                }
                else
                {
                    confirmEmail.Text = string.Empty;
                    confirmEmail.Visible = confirmEmail.Enabled = true;
                }
            }
        }
        protected void btn_ConfirmEmail_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    DataProvider.ConfirmEmailAddress(confirmEmail.Text, confirmPasscode.Text);
                    CookieJar.Eat("EmailForSignup");

                    if(Session["UsernameForSignup"] != null)
                    {
                        string username = Encrypt.DecryptString(Session["UsernameForSignup"].ToString());
                        PigeonMaster.CurrentUser.Login(username, emailJustConfirmed: true);
                        Response.Redirect("nest.aspx");
                    }
                    else
                    {
                        loginMultiView.SetActiveView(afterConfirmEmail);
                    }
                }
                catch (OracleException oEx)
                {
                    switch (oEx.Number)
                    {
                        case 20003:
                            lblConfirmEmailError.Text = "Email is not correct.";
                            break;
                        case 20501:
                            lblConfirmEmailError.Text = "Passcode is not correct.";
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