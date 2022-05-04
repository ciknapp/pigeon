using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CS414_Team2.Data;
using Oracle.DataAccess.Client;

namespace CS414_Team2.Pigeon
{
    public class User
    {
        public bool IsLoggedIn
        {
            get
            {
                bool loggedIn = false;

                if (UserName != string.Empty && AuthToken != string.Empty)
                {
                    try
                    {
                        loggedIn = DataProvider.ValidToken(UserName, AuthToken);
                    }
                    catch (OracleException oEx)
                    {
                        CookieJar.Put("EXCEPTION", oEx.Message);
                    }
                }

                CookieJar.Put("loggedin", loggedIn.ToString());
                CookieJar.Put("Remembered", Remembered.ToString());

                if (!loggedIn)
                {
                    if (!Remembered)
                    {
                        CookieJar.Eat("Username");
                    }

                    CookieJar.Eat("AuthToken");
                }

                return loggedIn;
            }
        }

        public string AuthToken
        {
            get
            {
                string token = string.Empty;

                try
                {
                    token = CookieJar.Take("AuthToken");
                }
                catch (OracleException oEx)
                {
                    CookieJar.Put("EXCEPTION", oEx.Message);
                }

                return token;
            }
        }

        public string UserName
        {
            get
            {
                string name = string.Empty;

                try
                {
                    name = CookieJar.Take("Username");
                }
                catch (OracleException oEx)
                {
                    CookieJar.Put("EXCEPTION", oEx.Message);
                }

                return name;
            }
        }

        public int UserID
        {
            get
            {
                int ID;

                if (!int.TryParse(CookieJar.Take("UserID"), out ID))
                {
                    ID = DataProvider.GetUserID(UserName);
                    CookieJar.Put("UserID", ID.ToString());
                }

                return ID;
            }
        }

        public int CurrentNestID
        {
            get
            {
                int ID;

                if (!int.TryParse(CookieJar.Take("NestID"), out ID))
                {
                    ID = -999;
                }

                return ID;
            }
            set
            {
                CookieJar.Put("NestID", value.ToString());
            }
        }

        public int? CurrentNestRole
        {
            get
            {
                int role;

                if (!int.TryParse(CookieJar.Take("NestRole"), out role))
                {
                    if (CurrentNestID != -999)
                    {
                        role = DataProvider.GetUserRole(UserID, CurrentNestID);
                        CookieJar.Put("NestRole", role.ToString());
                    }
                    else
                    {
                        return null;
                    }
                }

                return role;
            }
        }

        public bool Remembered
        {
            get
            {
                bool remembered = false;

                try
                {
                    remembered = CookieJar.Take("Remembered") == "True";
                }
                catch (OracleException oEx)
                {
                    CookieJar.Put("EXCEPTION", oEx.Message);
                }

                return remembered;
            }
        }

        public string UserIconUrl
        {
            get
            {
                string iconUrl = string.Empty;

                iconUrl = CookieJar.Take("UserIconURL");

                if (iconUrl == string.Empty)
                {
                    iconUrl = DataProvider.GetUserIcon(UserID);
                    CookieJar.Put("UserIconURL", iconUrl);
                }

                return iconUrl;
            }
        }

        public void Login(string username, string password = "", bool remember = false, bool emailJustConfirmed = false)
        {
            CookieJar.Put("Remembered", remember.ToString());

            if (remember)
            {
                CookieJar.Put("AuthToken", DataProvider.LoginUser(username.Trim(), password.Trim(), EmailJustConfirmed: emailJustConfirmed), DateTime.Now.AddDays(1));
                CookieJar.Put("Username", username.Trim(), DateTime.Now.AddYears(100));
                CookieJar.Put("Password", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password.Trim())), DateTime.Now.AddYears(100));
            }
            else
            {
                CookieJar.Put("AuthToken", DataProvider.LoginUser(username.Trim(), password.Trim(), EmailJustConfirmed: emailJustConfirmed));
                CookieJar.Put("Username", username.Trim());
            }
        }

        public void Logout()
        {
            DataProvider.LogoutUser(UserName);

            if (!Remembered)
            {
                CookieJar.Eat("Username");
                CookieJar.Eat("Password");
            }

            CookieJar.Eat("Username");
            CookieJar.Eat("AuthToken");
            CookieJar.Eat("UserID");
            CookieJar.Eat("NestID");
            CookieJar.Eat("UserIconURL");
            CookieJar.Eat("NestRole");
        }
    }
}