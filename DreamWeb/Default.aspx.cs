using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DreamLib;
using MySql.Data.MySqlClient;

namespace DreamWeb
{
    public partial class _Default : Page
    {
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnLogin.Enabled = false;

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
                    btnLogin.Enabled = true;
                }

                string sEmail = Request.QueryString["email"];
                txtUserID.Text = (sEmail == null) ? txtUserID.Text = "" : txtUserID.Text = sEmail;
                txtPassword.Text = "";
            }

            lblMessage.Text = "";
        }



        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string sUserID = txtUserID.Text;
            if (sUserID == "")
            {
                lblMessage.Text ="Please Fill In UserID";
            }
            else
            {
                string sPswd = txtPassword.Text;
                MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                CMember member = new CMember(sUserID, conn);
                if (member.IsEmpty())
                {
                    lblMessage.Text = "UserID/Email has not been registered. Please SignUp to register.";
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
                        lblMessage.Text = "Wrong password! Try again or click Forget Password to reset your password.";
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
            string sEmail = txtUserID.Text;
            string url = "NotifResetPass.aspx?email=" + sEmail;
            Response.Redirect(url);
        }


        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            string url = "SignUp.aspx?status=new";
            Response.Redirect(url);
        }


    }
}