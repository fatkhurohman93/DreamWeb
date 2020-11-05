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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnLogin.CausesValidation = false;

                int iStoreID = 0;
                string strID = Request.QueryString["id"];
                if (strID == null)
                {
                    iStoreID = ApplicationSession.StoreID;
                }
                else
                {
                    bool isNumeric = int.TryParse(strID, out int iID);
                    if (isNumeric) { iStoreID = iID; }
                }

                if (iStoreID == 0) { iStoreID = CMain.STOREID; }

                CStore store = CMain.GetStoreRecord(iStoreID);
                if (store.IsEmpty())
                {
                    MessageBox.Show("Store Record is not found");
                }
                else
                {
                    ApplicationSession.DBName = store.DBName;
                    ApplicationSession.StoreID = iStoreID;
                    ApplicationSession.QRcode = "";
                    btnLogin.CausesValidation = true;
                }

                string sEmail = Request.QueryString["email"];
                //txtUserID.Text = (sEmail == null) ? txtUserID.Text = "" : txtUserID.Text = sEmail;
                //txtPassword.Text = "";
            }

            //lblMessage.Text = "";
        }


        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string sUserID = txtUserID.Value;
            if (sUserID == "")
            {
                //lblMessage.Text = "Please Fill In UserID";
            }
            else
            {
                string sPswd = txtPswd.Value;
                MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                CMember member = new CMember(sUserID, conn);
                if (member.IsEmpty())
                {
                    //lblMessage.Text = "UserID/Email has not been registered. Please SignUp to register.";
                }
                else
                {
                    if (member.IsPasswordValid(sUserID, sPswd))
                    {
                        ApplicationSession.member = new CMiniMember(member.Name, member.ID, sUserID);

                        if (member.IsUsingTemporaryPassword())
                        {
                            Response.Redirect("ChangePasswordPage.aspx");
                        }
                        else
                        {
                            GoToOptionPages(conn);
                        }

                    }
                    else
                    {
                        //lblMessage.Text = "Wrong password! Try again or click Forget Password to reset your password.";
                    }
                }
            }
        }

        private void GoToOptionPages(MySqlConnection conn)
        {
            List<int> lst = CStore.ListOfOutletIDs(conn, ApplicationSession.StoreID);
            switch (lst.Count)
            {
                case 0:
                    MessageBox.Show("No Record of Outlet");
                    break;

                case 1:
                    CMain.InitParams(lst[0]);

                    //lgs masuk ke itemgroup page:
                    string url = "SalesTypePage.aspx";
                    Response.Redirect(url);
                    break;

                default:
                    Response.Redirect("HomePage.aspx");
                    break;
            }
        }

        protected void btnForgetPass_Click(object sender, EventArgs e)
        {
            string sEmail = txtUserID.Value;
            string url = "NotifResetPass.aspx?email=" + sEmail;
            Response.Redirect(url);
        }


        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            string url = "SignUp.aspx?status=new";
            Response.Redirect(url);
        }

        protected void btnSendPassword_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
            string sEmail = emailInput.Value;
            CMember member = new CMember(sEmail, conn);
            if (member.IsEmpty())
            {
                MessageBox.Show("UserID/Email has not been registered. Please Register");
            }
            else
            {
                string errMsg = CMain.ChangePassword(member.Name, sEmail, true);
                if (errMsg == "")
                {
                    ApplicationSession.cnt += 1;
                    string msg = (ApplicationSession.cnt > 1) ? "resent" : "sent";

                    MessageBox.Show("A temporary password has been " + msg + " to " + sEmail);
                    //lblStatus.Text = "Have not received it? Please click here to";
                    //linkBtn.Text = "resend";
                }
                else
                {
                    MessageBox.Show("Fail to send email. Please try again");
                    // lblStatus.Text = "";
                    //linkBtn.Text = "";
                }
            }

            
        }
    }
}