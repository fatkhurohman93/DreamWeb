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
    public partial class SignUp : System.Web.UI.Page
    {
        bool _isDirty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _isDirty = false;
                ClearScreen();
                string sStatus = Request.QueryString["status"];
                if (sStatus != null)
                {
                   switch (sStatus)
                    {
                        case "new":
                            string email = Request.QueryString["email"];
                            if (email != null)
                            {
                                if (CMain.IsValidEmail(email))
                                {
                                    txtEmail.Text = email;
                                }
                            }
                            lblTitle.InnerText = "Account Registration";
                            btnReg.Text = "Create Account";
                            btnReg.CommandName = "create";
                            btnReg.CommandArgument = "0";
                            txtPass.Visible = true;
                            break;

                        case "member":
                            MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                            CMember member = new CMember(ApplicationSession.member.Email, conn);
                            DisplayMember(member);

                            lblTitle.InnerText = "Member Info";
                            btnReg.Text = "Update Info";
                            btnReg.CommandName = "update";
                            btnReg.CommandArgument = member.ID.ToString();

                            txtPass.Visible = false;
                            break;
                    }
                }

            }
        }

        private void ClearScreen()
        {
            txtName.Text = "";
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtAddr.Text = "";
            txtCity.Text = "";
            txtCountry.Text = "";
            txtDoB.Text = "";
            txtPhone.Text = "";
            txtZipCode.Text = "";
            txtPass.Text = "";
        }

        private void DisplayMember(CMember member)
        {          
            txtName.Text = member.Name;
            txtFullName.Text = member.FullName;
            txtEmail.Text = member.Email;
            txtAddr.Text = member.Address;
            txtCity.Text = member.City;
            txtCountry.Text = member.Country;

            DateTime? dtDOB = member.DOB;
            if (dtDOB is null) { txtDoB.Text = ""; } else { txtDoB.Text = member.DOB.Value.ToShortDateString(); }
            
            txtPhone.Text = member.Phone;
            txtZipCode.Text = member.ZipCode;

            if (member.Sex == "F")
            {
                rbtnFemale.Checked = true;
            }
            else if (member.Sex == "M")
            {
                rbtnMale.Checked = true;
            }
        }

        protected void btnReg_Click(object sender, EventArgs e)
        {
            string sName = txtName.Text.Trim();
            if (sName == "")
            {
                MessageBox.Show("Please fill in Name");
            }
            else
            {
                string sEmail = txtEmail.Text.Trim();
                if (sEmail == "")
                {
                    MessageBox.Show("Please fill in Email");
                }
                else
                {
                    if (!CMain.IsValidEmail(sEmail))
                    {
                        MessageBox.Show("Email is not in correct format");
                    }
                    else
                    {
                        string sPhone = txtPhone.Text.Trim();
                        if (sPhone == "")
                        {
                            MessageBox.Show("Please fill in Phone Number");
                        }
                        else
                        {
                            switch (btnReg.CommandName)
                            {
                                case "create":
                                    string sPswd = txtPass.Text.Trim();
                                    if (PasswordInputOK(sPswd))
                                    {
                                        //save to table Member:
                                        CMember member = new CMember();
                                        DateTime dt = CMain.ConvertString_ToDate(txtDoB.Text);
                                        MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                                        bool bln = member.Insert(conn, sName, txtFullName.Text.Trim(), sEmail, sPswd,
                                                                 sPhone, txtAddr.Text.Trim(), txtCity.Text.Trim(), txtZipCode.Text.Trim(),
                                                                 txtCountry.Text.Trim(), dt, rbtnFemale.Checked ? "F" : "M", ApplicationSession.StoreID);
                                        if (bln)
                                        {
                                            MessageBox.Show("Registration successfull. Thank you!");

                                            ApplicationSession.member = new CMiniMember(member.Name, member.ID, member.Email);
                                            //masuk ke outlet screen
                                            Response.Redirect("HomePage.aspx");
                                        }
                                        else
                                        {
                                            MessageBox.Show("Registration is not successfull. Please try again.");
                                        }

                                    }

                                    break;

                                case "update":
                                    if (_isDirty)
                                    {
                                        bool bln = int.TryParse(btnReg.CommandArgument, out int iID);
                                        if (bln && iID > 0)
                                        {
                                            CMember member = new CMember();
                                            MySqlConnection conn = CMain.GetConnection(ApplicationSession.DBName);
                                            DateTime dt = CMain.ConvertString_ToDate(txtDoB.Text);
                                            bln = member.Update(conn, iID, sName, txtFullName.Text.Trim(), sEmail, sPhone, txtAddr.Text.Trim(), 
                                                                txtCity.Text.Trim(), txtZipCode.Text.Trim(), txtCountry.Text.Trim(), dt, 
                                                                rbtnFemale.Checked ? "F" : "M");
                                            if (bln)
                                            {
                                                MessageBox.Show("Update successfull. Thank you!");

                                                ApplicationSession.member = new CMiniMember(member.Name, member.ID, member.Email);
                                                //masuk ke outlet screen
                                                Response.Redirect("HomePage.aspx");
                                            }
                                            else
                                            {
                                                MessageBox.Show("Fail to update info. Please try again.");
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Fail to update info. Please try again.");
                                        }
                                    }
                                    else
                                    {
                                        Response.Redirect("HomePage.aspx");
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private bool PasswordInputOK(string sPswd)
        {
            bool bln = false;
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
                        bln = true;
                    }
                }
            }
            return bln;
        }

        protected void Info_TextChanged(object sender, EventArgs e)
        {
            _isDirty = true;
        }

        

    }
}