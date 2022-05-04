using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CS414_Team2
{
    public static class CookieJar
    {
        public static void Put(string name, string value, DateTime? expires = null, bool httpOnly = true)
        {
            string processedName = Convert.ToBase64String(Encoding.UTF8.GetBytes(name));

            HttpContext context = HttpContext.Current;

            HttpCookie cookie = new HttpCookie(name, Encrypt.EncryptString(value));

            if (expires.HasValue)
            {
                cookie.Expires = expires.Value;
            }

            cookie.HttpOnly = httpOnly;

            //context.Response.Cookies.Add(cookie);

            context.Session[name] = Encrypt.EncryptString(value);
        }

        public static string Take(string name)
        {
            string processedName = Convert.ToBase64String(Encoding.UTF8.GetBytes(name));

            HttpContext context = HttpContext.Current;

            string value = string.Empty;

            if (context.Request.Cookies[name] != null)
            {
                value = context.Request.Cookies[name].Value;
            }

            if (context.Session[name] != null)
            {
                value = Encrypt.DecryptString(context.Session[name].ToString());
            }

            return value;
        }

        public static void Eat(string name)
        {
            string processedName = Convert.ToBase64String(Encoding.UTF8.GetBytes(name));

            HttpContext context = HttpContext.Current;

            HttpCookie cookie = new HttpCookie(name, string.Empty);

            cookie.Expires = DateTime.Now.AddYears(-100);

            cookie.HttpOnly = true;

            //context.Response.Cookies.Add(cookie);

            context.Session[name] = string.Empty;
        }
    }
}