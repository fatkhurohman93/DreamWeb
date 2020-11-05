using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DreamLib;
using MySql.Data.MySqlClient;

namespace DreamWeb
{
    public partial class ChangePasswordPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
         
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("AccountPage.aspx");
        }


        protected void btnChange_Click(object sender, EventArgs e)
        {
            string sPswd = txtPass.Text.Trim();
            if (sPswd == "")
            {
                MessageBox.Show("Please fill in Password");
            }
            else
            {
                if (!CMain.HasLetter(sPswd))
                {
                    MessageBox.Show("Password must contain at least one letter [a-z]");
                }
                else
                {
                    if (!CMain.HasNumber(sPswd))
                    {
                        MessageBox.Show("Password must contain at least one number [0-9]");
                    }
                    else
                    {
                        string sConfirm = txtConfirm.Text.Trim();
                        if (sConfirm == "")
                        {
                            MessageBox.Show("Please confirm your new password");
                        }
                        else
                        {
                            if (sConfirm != sPswd)
                            {
                                MessageBox.Show("Confirmed password does not match");
                            }
                            else
                            {
                                string errMsg = DoChangePassword(sPswd);
                                if (errMsg == "")
                                {
                                    MessageBox.Show("Your password has been successfully changed");
                                    Response.Redirect("HomePage.aspx");
                                }
                            }
                        }                     
                    }
                }
            }
                      
        }

        private string DoChangePassword(string sPswd)
        {
            string errMsg = "";
            MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
            string sEmail = ApplicationSession.member.Email;
            CMember member = new CMember(sEmail, conn);
            if (member.IsEmpty())
            {
                errMsg = "UserID/Email has not been registered. Please SignUp to register";
            }
            else
            {
                bool bln = member.ChangePassword(conn, member.ID, sEmail, sPswd, false);
                if (!bln)
                {                  
                    errMsg = "Fail to update password. Please try again.";
                }
            }
            return errMsg;
        }
    }
}