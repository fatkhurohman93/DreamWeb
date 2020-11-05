using DreamLib;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DreamWeb
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ApplicationSession.cnt = 0;
                ClearScreen();
                string email = Request.QueryString["email"];
                if (email != null)
                {
                    if (CMain.IsValidEmail(email))
                    {
                        txtUserID.Text = email;
                    }
                }         
            }
           
        }

        private void ClearScreen(bool blnClearArgs = true)
        {
            lblNotif.Text = "";
            lblStatus.Text = "";
            linkBtn.Text = "";   
            if (blnClearArgs)
            {
                linkBtn.CommandName = "";
                linkBtn.CommandArgument = "";
            }
        }


        protected void linkBtn_Click(object sender, EventArgs e)
        {
            ClearScreen(false);
            string sEmail = txtUserID.Text.Trim();
            LinkButton lbtn = (LinkButton)sender;
            string cmd = lbtn.CommandName;
            switch (cmd)
            {
                case "register":
                    Response.Redirect("SignUp.aspx?email=" + sEmail);
                    break;

                case "resend":
                    string sName = lbtn.CommandArgument;
                    DoChangePassword(sName, sEmail);
                    break;
            }
            
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string sEmail = txtUserID.Text.Trim();
            if (!CMain.IsValidEmail(sEmail))
            {
                Response.Redirect("Default.aspx?email=" + sEmail);
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
                
        }


            protected void btnSend_Click(object sender, EventArgs e)
        {
            ClearScreen();
            string sEmail = txtUserID.Text.Trim();
            if (sEmail == "")
            {
                MessageBox.Show("Please key-in email address");
            }
            else
            {
                if (!CMain.IsValidEmail(sEmail))
                {
                    MessageBox.Show("Email address is not in the right format");
                }
                else
                {
                    MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                    CMember member = new CMember(sEmail, conn);
                    if (member.IsEmpty())
                    {
                        lblStatus.Text = "UserID/Email has not been registered. Please Register";
                        linkBtn.Text = "here";
                        linkBtn.CommandName = "register";
                    }
                    else
                    {
                        DoChangePassword(member.Name, sEmail);
                    }
                }
            }
                     
        }

        private void DoChangePassword(string sName, string sEmail)
        {
            ClearScreen();
            string errMsg = CMain.ChangePassword(sName, sEmail, true);
            if (errMsg == "")
            {
                ApplicationSession.cnt += 1;
                string msg = (ApplicationSession.cnt > 1)? "resent" : "sent";
               
                lblNotif.Text = "A temporary password has been " + msg + " to " + sEmail;
                lblStatus.Text = "Have not received it? Please click here to";
                linkBtn.Text = "resend";
                linkBtn.CommandName = "resend";
                linkBtn.CommandArgument = sName;
            }
            else
            {
                lblNotif.Text = "Fail to send email. Please try again";
                lblStatus.Text = "";
                linkBtn.Text = "";
                linkBtn.CommandName = "";
                linkBtn.CommandArgument = "";
            }
        }
    }
}