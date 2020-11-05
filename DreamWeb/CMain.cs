using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using DreamLib;
using MySql.Data.MySqlClient;
using System.Net;
using System.Net.Mail;
using MessagingToolkit.QRCode.Codec;
using System.Globalization;

namespace DreamWeb
{
    public class CMain
    {
        public const int STOREID = 1; //set storeid=1 as demo
        public const int OUTLETID = 1; //set outletid=1 as demo
        public const int SALESTYPEID = 1;
        public const string TABLENO = "01";

        public static MySqlConnection GetConnection(string sDBName)
        {
            string strConn = "Server=localhost;Port=;User Id=root;Password=;Database=" + sDBName + ";Connection Timeout=300000;";
            return new MySqlConnection(strConn);
        }

        public static CStore GetStoreRecord(int iStoreID)
        {
            return new CStore(iStoreID, GetConnection("dreamweb"));
        }

        public static void InitParams(int iOutletID)
        {
            ApplicationSession.OutletID = iOutletID;
            MySqlConnection conn = GetConnection(ApplicationSession.DBName);
            ApplicationSession.SettingRecord = new CSetting(conn, ApplicationSession.StoreID, iOutletID);
            ApplicationSession.SalesType = new CSalesType();
            ApplicationSession.SalesMaster = new CSalesMaster();
        }

        public static bool HasNumber(string input)
        {
            return input.Any(x => char.IsDigit(x));
        }

        public static bool HasLetter(string input)
        {
            return input.Any(x => char.IsLetter(x));
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        public static string ChangePassword(string sName, string sEmail, bool bySystem)
        {
            string sResult = "";
            MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
            CMember member = new CMember(sEmail, conn);
            if (member.IsEmpty())
            {
                sResult = "UserID/Email has not been registered. Please SignUp to register.";
            }
            else
            {
                string sNewPassword = CKeyGenerator.GetUniqueKey(8);
                bool bln = member.ChangePassword(conn, member.ID, sEmail, sNewPassword, bySystem);
                if (bln)
                {
                    string sSubject = "DreamWeb Reset Password";
                    string sMessage = "Hi " + sName + ", ";
                    sMessage += "<br /> <br /> Here is your temporary password: " + sNewPassword;
                    sMessage += "<br /> Please, use this password to login.";
                    sMessage += "<br /> <br /> Thanks and Regards, <br /> <br /> <br /> DreamPosWeb Support";

                    sResult = SendEmail(sName, sEmail, sSubject, sMessage);
                }
                else
                {
                    sResult = "Fail to update password. Please try again.";
                }
            }

            return sResult;
        }


        public static string SendEmail(string sName, string sEmail, string sSubject, string sMessage)
        {
            string sResult = "";
            using (MailMessage msg = new MailMessage())
            {
                msg.From = new MailAddress("dreamposwebs@gmail.com", "DreamWeb Support");
                msg.To.Add(sEmail);
                msg.Subject = sSubject;
                msg.Body = sMessage;
                msg.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;

                NetworkCredential NetworkCred = new NetworkCredential("dreamposwebs@gmail.com", "dr3amp0sw3b");
                client.UseDefaultCredentials = true;
                client.Credentials = NetworkCred;
                client.Port = 587;

                try
                {
                    client.Send(msg);
                }
                catch (Exception ex)
                {
                    sResult = ex.Message;
                }
            }
            return sResult;
        }

        public static Bitmap CreateQRCode(string str)
        {
            Bitmap img = null;
            try
            {
                QRCodeEncoder QRGenerator = new QRCodeEncoder();
                img = QRGenerator.Encode(str);
            }
            catch { }
            return img;
        }

        public static DateTime ConvertString_ToDate(string str)
        {
            DateTime dt;
            bool isDate = DateTime.TryParseExact(str, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (isDate)
            {
                string s = dt.ToString("yyyy-MM-dd") + " " + str;
                DateTime dtOrder;
                isDate = DateTime.TryParseExact(s, "yyyy-MM-dd HH\\:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtOrder);
                if (isDate)
                {
                    dt = dtOrder;
                }
            }
            return dt;
        }
    }
}