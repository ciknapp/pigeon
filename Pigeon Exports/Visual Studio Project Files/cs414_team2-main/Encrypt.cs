using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CS414_Team2
{
    public static class Encrypt
    {
        private const string PUB = "DT04VB7u";
        private const string PRI = "GfHTegKT";

        public static string EncryptString(string textToEncrypt)
        {
            string response = string.Empty;

            if (textToEncrypt != string.Empty)
            {
                byte[] privateKeyBytes = { };
                privateKeyBytes = Encoding.UTF8.GetBytes(PRI);
                byte[] publicKeyBytes = { };
                publicKeyBytes = Encoding.UTF8.GetBytes(PUB);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = Encoding.UTF8.GetBytes(textToEncrypt);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(publicKeyBytes, privateKeyBytes), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    response = Convert.ToBase64String(ms.ToArray());
                }
            }

            return response;
        }

        public static string DecryptString(string textToDecrypt)
        {
            string response = string.Empty;

            if (textToDecrypt != string.Empty)
            {
                byte[] privatekeyByte = { };
                privatekeyByte = Encoding.UTF8.GetBytes(PRI);
                byte[] publickeybyte = { };
                publickeybyte = Encoding.UTF8.GetBytes(PUB);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    response = encoding.GetString(ms.ToArray());
                }
            }

            return response;
        }
    }
}